using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameplayManager : MonoBehaviour
{
    [SerializeField] TaskBarHandler _taskBarHandler;

    private Dictionary<PieceType, PieceLogic> _logicHandlers = new Dictionary<PieceType, PieceLogic>(6);

    private bool _gameOver;

    #region Singleton Pattern

    private static GameplayManager _instance;

    public static GameplayManager Instance => _instance;

    #endregion

    public bool GameOver => _gameOver;

    private void Awake()
    {
        _instance = this;

        SetupLogicHandling();
    }

    private void SetupLogicHandling()
    {
        _logicHandlers.Add(PieceType.Pawn, new PawnLogic());
        _logicHandlers.Add(PieceType.Knight, new KnightLogic());
        _logicHandlers.Add(PieceType.Bishop, new BishopLogic());
        _logicHandlers.Add(PieceType.Rook, new RookLogic());
        _logicHandlers.Add(PieceType.Queen, new QueenLogic());
        _logicHandlers.Add(PieceType.King, new KingLogic());
    }

    public PieceLogic GetPieceLogic(PieceType pieceType)
    {
        return _logicHandlers[pieceType];
    }

    public IEnumerator MoveSelectedPieceRoutine(Vector2Int newTileIndex)
    {
        PieceBehaviour currentSelection = PieceManager.Instance.CurrentSelection;
        if (!currentSelection.HasBeenMoved)
        {
            currentSelection.HasBeenMoved = true;
        }

        currentSelection.CurrentIndex = newTileIndex;
        currentSelection.SetValidMoves();

        Vector3 piecePos = new Vector3(newTileIndex.y, 0.001f, newTileIndex.x);
        yield return currentSelection.SetPositionRoutine(piecePos);

        BoardManager.Instance.ResetHighlightedTiles();
        PieceManager.Instance.MovedSelectedPiece();
    }

    public IEnumerator PerformCastlingRoutine(PieceBehaviour currentSelection)
    {
        Vector2Int currentIndex = currentSelection.CurrentIndex;
        Vector2Int rookIndex = currentIndex + new Vector2Int(0, 1);
        Tile rookTile = BoardManager.Instance.TileSet[rookIndex.x, rookIndex.y];
        PieceBehaviour rookPiece = rookTile.HeldPiece;
        rookTile.HeldPiece = null;

        Vector2Int rookMoveIndex = currentIndex + new Vector2Int(0, -1);
        Tile rookMoveTile = BoardManager.Instance.TileSet[rookMoveIndex.x, rookMoveIndex.y];
                rookMoveTile.HeldPiece = rookPiece;

        Vector3 rookMovePos = new Vector3(rookMoveTile.Index.y, 0.001f, rookMoveTile.Index.x);
        yield return rookPiece.SetPositionRoutine(rookMovePos);
        rookPiece.HasBeenMoved = true;

        EconomyManager.Instance.AddValue(currentSelection.PieceData.PlayerType, 20);
    }
    
    public void EnableEnPassant(PieceBehaviour currentSelection, Vector2Int currentIndex)
    {
        int direction;
        if (currentSelection.PieceData.PlayerType == PlayerType.White)
        {
            direction = -1;
        }
        else
        {
            direction = 1;
        }

        Vector2Int move = currentIndex + new Vector2Int(direction, 0);
        Tile tile = BoardManager.Instance.TileSet[move.x, move.y];
        tile.EnPassant = true;
    }
        
    public IEnumerator ProcessPromotionRoutine(PieceBehaviour pawn)
    {
        PowerupManager.Instance.ProcessingPawnPromotion = true;

        Coroutine<int> routine = this.StartCoroutine<int>(UIManager.Instance.ProcessPromotionUIRoutine());
        yield return routine.coroutine;
        int selectedIndex = routine.returnVal;

        PromotePawn(pawn, selectedIndex);
        PowerupManager.Instance.ProcessingPawnPromotion = false;
    }

    public void PromotePawn(PieceBehaviour pawn, int selectedIndex)
    {
        Vector2Int currentIndex = pawn.CurrentIndex;
        Tile tile = BoardManager.Instance.TileSet[currentIndex.x, currentIndex.y];

        PieceBehaviour promotedPiece = PieceManager.Instance.GeneratePromotionPiece(selectedIndex, pawn.PieceData.PlayerType);
        promotedPiece.CurrentIndex = pawn.CurrentIndex;
        promotedPiece.transform.position = pawn.transform.position;
        promotedPiece.transform.rotation = pawn.transform.rotation;
        tile.HeldPiece = promotedPiece;

        Destroy(pawn.gameObject);
    }

    public void CheckCapturedPiece(PieceData pieceData)
    {
        if (pieceData.PieceType == PieceType.King)
        {
            UIManager.Instance.DisplayGameOver(pieceData.PlayerType == PlayerType.Black);
            _gameOver = true;
        }
    }
}

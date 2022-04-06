using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameplayManager : MonoBehaviour
{
    private Dictionary<PieceType, PieceLogic> _logicHandlers = new Dictionary<PieceType, PieceLogic>(6);

    private List<Vector2Int> _whiteMoves = new List<Vector2Int>();
    private List<Vector2Int> _blackMoves = new List<Vector2Int>();

    #region Singleton Pattern

    private static GameplayManager _instance;

    public static GameplayManager Instance => _instance;

    #endregion
    
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

    public void PerformCastling()
    {
        Vector2Int currentIndex = PieceManager.Instance.CurrentSelection.CurrentIndex;
        Vector2Int rookIndex = currentIndex + new Vector2Int(0, 3);
        Tile rookTile = BoardManager.Instance.TileSet[rookIndex.x, rookIndex.y];
        PieceBehaviour rookPiece = rookTile.HeldPiece;
        rookTile.HeldPiece = null;

        Vector2Int rookMoveIndex = currentIndex + new Vector2Int(0, 1);
        Tile rookMoveTile = BoardManager.Instance.TileSet[rookMoveIndex.x, rookMoveIndex.y];
        rookMoveTile.HeldPiece = rookPiece;

        Vector3 rookMovePos = new Vector3(rookMoveTile.Index.y, 0.001f, rookMoveTile.Index.x);
        rookPiece.SetPosition(rookMovePos);
        rookPiece.HasBeenMoved = true;

        print(rookPiece);
    }
    
    public void EnableEnPassant(Vector2Int currentIndex)
    {
        int direction;
        if (PieceManager.Instance.CurrentSelection.PieceData.PlayerType == PlayerType.White)
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
}

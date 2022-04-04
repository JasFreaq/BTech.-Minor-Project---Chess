using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PieceManager : MonoBehaviour
{
    [SerializeField] private PieceBehaviour[] _piecePrefabs = new PieceBehaviour[12];

    [Header("Capture Parameters")] 
    [SerializeField] private float _maxCaptureZoneSpace = 1f;
    [SerializeField] private float _captureZoneSize = 6f;
    [SerializeField] private Transform _whiteZoneRef;
    [SerializeField] private Transform _blackZoneRef;
    [SerializeField] private Vector3 _capturedPieceScale;

    private PieceBehaviour _currentSelection;

    private List<PieceBehaviour> _playingWhites = new List<PieceBehaviour>();
    private List<PieceBehaviour> _playingBlacks = new List<PieceBehaviour>();
    
    private List<PieceBehaviour> _capturedWhites = new List<PieceBehaviour>();
    private List<PieceBehaviour> _capturedBlacks = new List<PieceBehaviour>();

    #region Singleton Pattern

    private static PieceManager _instance;

    public static PieceManager Instance => _instance;

    #endregion

    public PieceBehaviour CurrentSelection => _currentSelection;

    private void Awake()
    {
        _instance = this;
    }

    private void Start()
    {
        GeneratePieces();
    }

    public void GeneratePieces()
    {
        foreach (PieceBehaviour piecePrefab in _piecePrefabs)
        {
            foreach (Vector2Int startIndex in piecePrefab.StartIndices)
            {
                Vector3 piecePos = new Vector3(startIndex.x, 0.001f, startIndex.y);
                Quaternion pieceRot = Quaternion.identity;
                if (piecePrefab.PieceData.PlayerType == PlayerType.Black)
                    pieceRot = Quaternion.Euler(0, 180, 0);

                PieceBehaviour piece = Instantiate(piecePrefab, piecePos, pieceRot, transform);
                piece.CurrentIndex = new Vector2Int(startIndex.y, startIndex.x);
                
                BoardManager.Instance.TileSet[startIndex.y, startIndex.x].HeldPiece = piece;

                if (piece.PieceData.PlayerType == PlayerType.White)
                {
                    _playingWhites.Add(piece);
                }
                else
                {
                    _playingBlacks.Add(piece);
                }
            }
        }
    }

    public void SelectPiece(PieceBehaviour selectedPiece, Vector2Int tileIndex)
    {
        if (_currentSelection != null)
        {
            BoardManager.Instance.ResetHighlightedTiles();
        }

        _currentSelection = selectedPiece;
        BoardManager.Instance.HighlightPossibleMoves(_currentSelection.PossibleMoves);

    }

    public void UnselectPiece()
    {
        _currentSelection = null;
        BoardManager.Instance.ResetHighlightedTiles();
    }

    public void CapturePiece(PieceBehaviour capturedPiece)
    {
        float zMultiplier;
        Transform referenceTransform;
        List<PieceBehaviour> capturedPieces;
        if (capturedPiece.PieceData.PlayerType == PlayerType.White)
        {
            zMultiplier = 1f;
            _playingWhites.Remove(capturedPiece);
            _capturedWhites.Add(capturedPiece);
            capturedPieces = _capturedWhites;
            referenceTransform = _whiteZoneRef;
        }
        else
        {
            zMultiplier = -1f;
            _playingWhites.Remove(capturedPiece);
            _capturedBlacks.Add(capturedPiece);
            capturedPieces = _capturedBlacks;
            referenceTransform = _blackZoneRef;
        }

        int n = capturedPieces.Count;
        float zIncrement = Mathf.Min(_maxCaptureZoneSpace, _captureZoneSize / n);
        for (int i = 0; i < n; i++)
        {
            Vector3 pos = referenceTransform.position;
            pos.z += i * zIncrement * zMultiplier;
            capturedPieces[i].SetPosition(pos);
        }

        capturedPiece.SetScale(_capturedPieceScale);
    }
    
    public void MoveSelectedPiece(Vector2Int newTileIndex)
    {
        if (!_currentSelection.HasBeenMoved)
        {
            _currentSelection.HasBeenMoved = true;
        }

        _currentSelection.CurrentIndex = newTileIndex;
        _currentSelection.SetValidMoves();

        Vector3 piecePos = new Vector3(newTileIndex.y, 0.001f, newTileIndex.x);
        _currentSelection.SetPosition(piecePos);
        _currentSelection = null;

        BoardManager.Instance.ResetHighlightedTiles();
        SetPossibleMoves();
    }
    
    private void SetPossibleMoves()
    {
        foreach (PieceBehaviour piece in _playingWhites)
        {
            piece.SetPossibleMoves();
        }

        foreach (PieceBehaviour piece in _playingBlacks)
        {
            piece.SetPossibleMoves();
        }
    }

    public void PerformCastling()
    {
        Vector2Int rookIndex = _currentSelection.CurrentIndex + new Vector2Int(0, 3);
        Tile rookTile = BoardManager.Instance.TileSet[rookIndex.x, rookIndex.y];
        PieceBehaviour rookPiece = rookTile.HeldPiece;
        rookTile.HeldPiece = null;

        Vector2Int rookMoveIndex = _currentSelection.CurrentIndex + new Vector2Int(0, 1);
        Tile rookMoveTile = BoardManager.Instance.TileSet[rookMoveIndex.x, rookMoveIndex.y];
        rookMoveTile.HeldPiece = rookPiece;
        
        Vector3 rookMovePos = new Vector3(rookMoveTile.Index.y, 0.001f, rookMoveTile.Index.x);
        rookPiece.SetPosition(rookMovePos);
        rookPiece.HasBeenMoved = true;
    }
}

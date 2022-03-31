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

    private List<PieceBehaviour> _capturedWhites = new List<PieceBehaviour>();
    private List<PieceBehaviour> _capturedBlacks = new List<PieceBehaviour>();

    #region Singleton Pattern

    private static PieceManager _instance;

    public static PieceManager Instance => _instance;

    #endregion

    public PieceBehaviour CurrentSelection
    {
        get => _currentSelection;
    }

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
        _currentSelection.SetPossibleMoves();
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
            _capturedWhites.Add(capturedPiece);
            capturedPieces = _capturedWhites;
            referenceTransform = _whiteZoneRef;
        }
        else
        {
            zMultiplier = -1f;
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
    
    public void MoveSelectedPieceToTile(Vector2Int newTileIndex)
    {
        _currentSelection.CurrentIndex = newTileIndex;
        _currentSelection.SetValidMoves();

        Vector3 piecePos = new Vector3(newTileIndex.y, 0.001f, newTileIndex.x);
        _currentSelection.SetPosition(piecePos);
        
        _currentSelection = null;
        BoardManager.Instance.ResetHighlightedTiles();
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardManager : MonoBehaviour
{
    [Header("Tiles")]
    [SerializeField] private float _tileSize = 1f;
    [SerializeField] private Tile _tilePrefab;

    private Canvas _boardCanvas;

    private readonly Tile[,] _tileSet = new Tile[8, 8];
    private readonly Dictionary<int, Tile> _tileRefs = new Dictionary<int, Tile>(64);

    private List<Tile> _highlightedTiles = new List<Tile>();

    #region Singleton Pattern

    private static BoardManager _instance;

    public static BoardManager Instance => _instance;

    #endregion

    public Tile[,] TileSet => _tileSet;

    public IReadOnlyDictionary<int, Tile> TileRefs => _tileRefs;

    private void Awake()
    {
        _instance = this;

        _boardCanvas = GetComponentInChildren<Canvas>();
    }

    private void Start()
    {
        GenerateTileset();
    }

    private void GenerateTileset()
    {
        Transform tileParent = _boardCanvas.transform;

        for (int i = 0; i < 8; i++)
        {
            for (int j = 0; j < 8; j++)
            {
                Vector3 tilePos = new Vector3(j, i, 0.001f);

                Tile tile = Instantiate(_tilePrefab, tileParent);
                tile.transform.localPosition = tilePos;
                tile.transform.localRotation = Quaternion.identity;

                tile.name = $"{(char) (i + 97)}{j + 1}";
                tile.Index = new Vector2Int(i, j);
                
                _tileSet[i, j] = tile;
                _tileRefs.Add(tile.Collider.GetInstanceID(), tile);
            }
        }
    }

    public void HighlightPossibleMoves(PieceBehaviour selectedPiece)
    {
        foreach (Vector2Int possibleMove in selectedPiece.PossibleMoves)
        {
            Tile tile = _tileSet[possibleMove.y, possibleMove.x];
            if (tile.HeldPiece != null ||
                tile.EnPassant && selectedPiece.PieceData.PieceType == PieceType.Pawn) 
            {
                tile.SetCapturable(true);
            }
            else
            {
                tile.SetValid(true);
            }

            _highlightedTiles.Add(tile);
        }
    }

    public void ResetHighlightedTiles()
    {
        foreach (Tile tile in _highlightedTiles)
        {
            tile.ResetRepresentation();
        }

        _highlightedTiles.Clear();
    }
}

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

    #region Singleton Pattern

    private static BoardManager _instance;

    public static BoardManager Instance => _instance;

    #endregion

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
}
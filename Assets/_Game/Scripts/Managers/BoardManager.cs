using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardManager : MonoBehaviour
{
    [Header("Tiles")]
    [SerializeField] private float _tileSize = 1f;
    [SerializeField] private GameObject _tilePrefab;

    private Canvas _boardCanvas;
    private GameObject[,] _tileSet = new GameObject[8, 8];

    private static BoardManager _instance;

    public static BoardManager Instance
    {
        get
        {
            return _instance;
        }
    }

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
                Vector3 tilePos = new Vector3(j, i, 0);

                GameObject tile = Instantiate(_tilePrefab, tileParent);
                tile.transform.localPosition = tilePos;
                tile.transform.localRotation = Quaternion.identity;

                tile.name = $"{(char) (i + 97)}{j + 1}";
                
                _tileSet[i, j] = tile;
            }
        }
    }
}

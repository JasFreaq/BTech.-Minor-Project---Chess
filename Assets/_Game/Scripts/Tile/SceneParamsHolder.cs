using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneParamsHolder : MonoBehaviour
{
    [Header("Floats")] 
    [SerializeField] private float _pieceMoveSpeed = 1f;
    [SerializeField] private float _pieceScaleSpeed = 1f;
    
    [Header("Materials")]
    [SerializeField] private Material _hoverMat;
    [SerializeField] private Material _selectMat;

    #region Singleton Pattern

    private static SceneParamsHolder _instance;

    public static SceneParamsHolder Instance => _instance;

    #endregion

    public float PieceMoveSpeed => _pieceMoveSpeed;
    
    public float PieceScaleSpeed => _pieceScaleSpeed;
    
    public Material HoverMat => _hoverMat;

    public Material SelectMat => _selectMat;

    private void Awake()
    {
        _instance = this;
    }
}

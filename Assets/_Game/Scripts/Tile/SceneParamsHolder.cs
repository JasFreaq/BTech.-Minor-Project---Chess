using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneParamsHolder : MonoBehaviour
{
    [Header("Floats")] 
    [SerializeField] private float _pieceMoveTime = 1f;
    [SerializeField] private float _pieceScaleTime = 1f;
    
    [Header("Materials")]
    [SerializeField] private Material _hoverMat;
    [SerializeField] private Material _selectMat;
    [SerializeField] private Material _validMat;
    [SerializeField] private Material _captureMat;

    #region Singleton Pattern

    private static SceneParamsHolder _instance;

    public static SceneParamsHolder Instance => _instance;

    #endregion

    public float PieceMoveTime => _pieceMoveTime;
    
    public float PieceScaleTime => _pieceScaleTime;
    
    public Material HoverMat => _hoverMat;

    public Material SelectMat => _selectMat;
    
    public Material ValidMat => _validMat;

    public Material CaptureMat => _captureMat;

    private void Awake()
    {
        _instance = this;
    }
}

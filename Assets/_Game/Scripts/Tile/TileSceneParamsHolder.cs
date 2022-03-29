using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileSceneParamsHolder : MonoBehaviour
{
    [SerializeField] private Material _hoverMat;
    [SerializeField] private Material _selectMat;

    #region Singleton Pattern

    private static TileSceneParamsHolder _instance;

    public static TileSceneParamsHolder Instance => _instance;

    #endregion

    public Material HoverMat => _hoverMat;

    public Material SelectMat => _selectMat;

    private void Awake()
    {
        _instance = this;
    }
}

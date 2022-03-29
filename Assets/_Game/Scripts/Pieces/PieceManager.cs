using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PieceManager : MonoBehaviour
{
    #region Singleton Pattern

    private static PieceManager _instance;

    public static PieceManager Instance => _instance;

    #endregion

    private void Awake()
    {
        _instance = this;
    }
}

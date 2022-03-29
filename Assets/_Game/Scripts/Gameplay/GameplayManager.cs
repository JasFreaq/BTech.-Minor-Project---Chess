using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameplayManager : MonoBehaviour
{
    #region Singleton Pattern

    private static GameplayManager _instance;

    public static GameplayManager Instance => _instance;

    #endregion


    private void Awake()
    {
        _instance = this;
    }
}

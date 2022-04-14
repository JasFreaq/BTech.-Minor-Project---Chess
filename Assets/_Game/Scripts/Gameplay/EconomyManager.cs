using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EconomyManager : MonoBehaviour
{
    private int _whiteValue;
    private int _blackValue;

    #region Singleton Pattern

    private static EconomyManager _instance;

    public static EconomyManager Instance => _instance;

    #endregion

    public int WhiteValue => _whiteValue;

    public int BlackValue => _blackValue;

    private void Awake()
    {
        _instance = this;
    }

    public void AddValue(PieceData pieceData)
    {
        if (pieceData.PlayerType == PlayerType.White)
        {
            _blackValue += pieceData.PieceValue;
            UIManager.Instance.ChangeCurrencyText(_blackValue);
        }
        else
        {
            _whiteValue += pieceData.PieceValue;
            UIManager.Instance.ChangeCurrencyText(_whiteValue);
        }
    }
}

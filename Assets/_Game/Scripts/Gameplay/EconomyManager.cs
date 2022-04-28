using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EconomyManager : MonoBehaviour
{
    [SerializeField] private int[] _initialCosts = { 15, 50, 30, 120, 65, 45, 45 };

    private int _whiteValue = 200;
    private int _blackValue;

    private int[] _whiteCosts = new int[7];
    private int[] _blackCosts = new int[7];

    #region Singleton Pattern

    private static EconomyManager _instance;

    public static EconomyManager Instance => _instance;

    #endregion

    public int WhiteValue => _whiteValue;

    public int BlackValue => _blackValue;

    private void Awake()
    {
        _instance = this;

        for (int i = 0; i < _initialCosts.Length; i++) 
        {
            _whiteCosts[i] = _initialCosts[i];
            _blackCosts[i] = _initialCosts[i];
        }
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
    
    public void AddValue(PlayerType playerType, int pieceValue)
    {
        if (playerType == PlayerType.White)
        {
            _blackValue += pieceValue;
            UIManager.Instance.ChangeCurrencyText(_blackValue);
        }
        else
        {
            _whiteValue += pieceValue;
            UIManager.Instance.ChangeCurrencyText(_whiteValue);
        }
    }

    public int[] GetCosts(PlayerType player)
    {
        if (player == PlayerType.White)
        {
            return _whiteCosts;
        }

        return _blackCosts;
    }

    public bool ProcessCost(int index)
    {
        PlayerType player = TurnManager.Instance.CurrentPlayerType;
        if (player == PlayerType.White)
        {
            int cost = _whiteCosts[index];
            if (_whiteValue >= cost)
            {
                _whiteValue -= cost;
                _whiteCosts[index] *= 2;
                UIManager.Instance.ChangeCurrencyText(_whiteValue);
                return true;
            }
            
            return false;
        }
        else
        {
            int cost = _blackCosts[index];
            if (_blackValue >= cost)
            {
                _blackValue -= cost;
                _blackCosts[index] *= 2;
                UIManager.Instance.ChangeCurrencyText(_blackValue);
                return true;
            }
            
            return false;
        }
    }
}

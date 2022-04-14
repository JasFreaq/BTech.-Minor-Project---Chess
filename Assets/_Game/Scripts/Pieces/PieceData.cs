using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct PieceData
{
    [SerializeField] private PlayerType _playerType;
    [SerializeField] private PieceType _pieceType;
    [SerializeField] private int _pieceValue;
    
    public PlayerType PlayerType
    {
        get { return _playerType; }
    }

    public PieceType PieceType
    {
        get { return _pieceType; }
    }

    public int PieceValue
    {
        get { return _pieceValue; }
    }
}

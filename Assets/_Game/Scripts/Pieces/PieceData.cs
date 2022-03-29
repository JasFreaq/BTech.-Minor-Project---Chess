using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct PieceData
{
    [SerializeField] private PlayerType _playerType;

    [SerializeField] private PieceType _pieceType;

    public PieceData(PlayerType playerType, PieceType pieceType)
    {
        _playerType = playerType;
        _pieceType = pieceType;
    }

    public PlayerType PlayerType
    {
        get { return _playerType; }
    }

    public PieceType PieceType
    {
        get { return _pieceType; }
    }
}

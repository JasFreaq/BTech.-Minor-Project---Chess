using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SquareState
{
    public PieceData _heldPiece;

    public SquareState() {}

    public SquareState(PieceData pieceData)
    {
        _heldPiece = pieceData;
    }

    public PieceData HeldPiece
    {
        get { return _heldPiece; }
    }
}

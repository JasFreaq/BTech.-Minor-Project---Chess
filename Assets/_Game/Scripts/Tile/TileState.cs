using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TileState
{
    public PieceData _heldPiece;

    public TileState() {}

    public TileState(PieceData pieceData)
    {
        _heldPiece = pieceData;
    }

    public PieceData HeldPiece
    {
        get { return _heldPiece; }
    }
}

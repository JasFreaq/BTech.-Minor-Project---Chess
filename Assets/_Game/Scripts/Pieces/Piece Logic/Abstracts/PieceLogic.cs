using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PieceLogic
{
    public abstract void GenerateValidMoves(PieceBehaviour piece);

    public abstract void GeneratePossibleMoves(PieceBehaviour piece);
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PieceLogic
{
    public abstract void GenerateValidMoves(ref List<Vector2Int> validMoves, Vector2Int currentIndex, PlayerType playerType);

    public abstract void GeneratePossibleMoves(ref List<Vector2Int> possibleMoves, ref List<Vector2Int> validMoves, PlayerType playerType);
}

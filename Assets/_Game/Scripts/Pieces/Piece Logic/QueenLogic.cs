using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QueenLogic : MultidirectionalLogic
{
    public QueenLogic()
    {
        _degreesOfMovement = 8;
    }

    public override void GenerateValidMoves(ref List<Vector2Int> validMoves, Vector2Int currentIndex, PlayerType playerType)
    {
        PieceLogic bishopLogic = GameplayManager.Instance.GetPieceLogic(PieceType.Bishop);
        bishopLogic.GenerateValidMoves(ref validMoves, currentIndex, playerType);

        validMoves.Add(-Vector2Int.one);

        PieceLogic rookLogic = GameplayManager.Instance.GetPieceLogic(PieceType.Rook);
        rookLogic.GenerateValidMoves(ref validMoves, currentIndex, playerType);
    }
}

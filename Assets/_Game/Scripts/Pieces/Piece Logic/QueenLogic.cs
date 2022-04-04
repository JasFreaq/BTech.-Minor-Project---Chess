using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QueenLogic : MultidirectionalLogic
{
    public QueenLogic()
    {
        _degreesOfMovement = 8;
    }

    public override void GenerateValidMoves(PieceBehaviour piece)
    {
        PieceLogic bishopLogic = GameplayManager.Instance.GetPieceLogic(PieceType.Bishop);
        bishopLogic.GenerateValidMoves(piece);

        piece.ValidMoves.Add(-Vector2Int.one);

        PieceLogic rookLogic = GameplayManager.Instance.GetPieceLogic(PieceType.Rook);
        rookLogic.GenerateValidMoves(piece);
    }
}

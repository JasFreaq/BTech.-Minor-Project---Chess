using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KingLogic : PieceLogic
{
    public override void GenerateValidMoves(ref List<Vector2Int> validMoves, Vector2Int currentIndex, PlayerType playerType)
    {
        List<Vector2Int> candidateIndices = new List<Vector2Int>(8);

        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                if (i == j && i == 1)
                    continue;

                Vector2Int index = currentIndex + new Vector2Int(i - 1, j - 1);
                candidateIndices.Add(index);
            }
        }

        foreach (Vector2Int index in candidateIndices)
        {
            if (index.x >= 0 && index.x < 8 &&
                index.y >= 0 && index.y < 8)
            {
                validMoves.Add(new Vector2Int(index.y, index.x));
            }
        }
    }

    public override void GeneratePossibleMoves(ref List<Vector2Int> possibleMoves, ref List<Vector2Int> validMoves, PlayerType playerType)
    {
        foreach (Vector2Int validMove in validMoves)
        {
            PieceBehaviour piece = BoardManager.Instance.TileSet[validMove.y, validMove.x].HeldPiece;
            if (piece)
            {
                if (piece.PieceData.PlayerType != playerType)
                {
                    possibleMoves.Add(validMove);
                }
            }
            else
            {
                possibleMoves.Add(validMove);
            }
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PawnLogic : PieceLogic
{
    public override void GenerateValidMoves(ref List<Vector2Int> validMoves, Vector2Int currentIndex, PlayerType playerType)
    {
        int startIndex;
        int directionIncrement;
        if (playerType == PlayerType.White)
        {
            startIndex = 1;
            directionIncrement = 1;
        }
        else
        {
            startIndex = 6;
            directionIncrement = -1;
        }

        Vector2Int index = new Vector2Int(currentIndex.x + directionIncrement, currentIndex.y);
        if (index.x < 8)
        {
            validMoves.Add(new Vector2Int(index.y, index.x));

            index.x += directionIncrement;
            if (currentIndex.x == startIndex && index.x < 8)
            {
                validMoves.Add(new Vector2Int(index.y, index.x));
            }
        }
    }

    public override void GeneratePossibleMoves(ref List<Vector2Int> possibleMoves, ref List<Vector2Int> validMoves, PlayerType playerType)
    {
        foreach (Vector2Int validMove in validMoves)
        {
            if (BoardManager.Instance.TileSet[validMove.y, validMove.x].HeldPiece)
            {
                break;
            }

            possibleMoves.Add(validMove);
        }

        if (validMoves.Count > 0) 
        {
            if (validMoves[0].x > 0)
            {
                Vector2Int move = new Vector2Int(validMoves[0].x - 1, validMoves[0].y);
                PieceBehaviour piece = BoardManager.Instance.TileSet[move.y, move.x].HeldPiece;
                if (piece && piece.PieceData.PlayerType != playerType)
                {
                    possibleMoves.Add(move);
                }
            }

            if (validMoves[0].x < 7)
            {
                Vector2Int move = new Vector2Int(validMoves[0].x + 1, validMoves[0].y);
                PieceBehaviour piece = BoardManager.Instance.TileSet[move.y, move.x].HeldPiece;
                if (piece && piece.PieceData.PlayerType != playerType)
                {
                    possibleMoves.Add(move);
                }
            }
        }
    }
}

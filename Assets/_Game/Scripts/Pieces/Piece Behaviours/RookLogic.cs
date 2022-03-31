using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RookLogic : PieceLogic
{
    public override void GenerateValidMoves(ref List<Vector2Int> validMoves, Vector2Int currentIndex, PlayerType playerType)
    {
        Vector2Int index = currentIndex;
        while (index.x > 0)
        {
            index.x--;
            validMoves.Add(new Vector2Int(index.y, index.x));
        }
        validMoves.Add(-Vector2Int.one);

        index = currentIndex;
        while (index.x < 7)
        {
            index.x++;
            validMoves.Add(new Vector2Int(index.y, index.x));
        }
        validMoves.Add(-Vector2Int.one);

        index = currentIndex;
        while (index.y > 0)
        {
            index.y--;
            validMoves.Add(new Vector2Int(index.y, index.x));
        }
        validMoves.Add(-Vector2Int.one);

        index = currentIndex;
        while (index.y < 7)
        {
            index.y++;
            validMoves.Add(new Vector2Int(index.y, index.x));
        }
    }

    public override void GeneratePossibleMoves(ref List<Vector2Int> possibleMoves, ref List<Vector2Int> validMoves, PlayerType playerType)
    {
        bool foundPiece = false;
        int findCount = 0;
        foreach (Vector2Int validMove in validMoves)
        {
            if (validMove == -Vector2Int.one)
            {
                foundPiece = false;
                continue;
            }

            if (!foundPiece)
            {
                PieceBehaviour piece = BoardManager.Instance.TileSet[validMove.y, validMove.x].HeldPiece;
                if (piece)
                {
                    if (piece.PieceData.PlayerType != playerType)
                    {
                        possibleMoves.Add(validMove);
                    }

                    foundPiece = true;
                    findCount++;

                    if (findCount == 4)
                    {
                        break;
                    }
                }
                else
                {
                    possibleMoves.Add(validMove);
                }
            }
        }
    }
}

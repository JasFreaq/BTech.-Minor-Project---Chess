using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MultidirectionalLogic : PieceLogic
{
    protected int _degreesOfMovement;

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

                    if (findCount == _degreesOfMovement)
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

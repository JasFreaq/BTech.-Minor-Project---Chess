using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MultidirectionalLogic : PieceLogic
{
    protected int _degreesOfMovement;

    public override void GeneratePossibleMoves(PieceBehaviour piece)
    {
        bool foundPiece = false;
        int findCount = 0;
        foreach (Vector2Int validMove in piece.ValidMoves)
        {
            if (validMove == -Vector2Int.one)
            {
                foundPiece = false;
                continue;
            }

            if (!foundPiece)
            {
                PieceBehaviour heldPiece = BoardManager.Instance.TileSet[validMove.y, validMove.x].HeldPiece;
                if (heldPiece)
                {
                    if (heldPiece.PieceData.PlayerType != piece.PieceData.PlayerType)
                    {
                        piece.PossibleMoves.Add(validMove);
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
                    piece.PossibleMoves.Add(validMove);
                }
            }
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PawnLogic : PieceLogic
{
    public override void GenerateValidMoves(PieceBehaviour piece)
    {
        int startIndex;
        int directionIncrement;
        if (piece.PieceData.PlayerType == PlayerType.White)
        {
            startIndex = 1;
            directionIncrement = 1;
        }
        else
        {
            startIndex = 6;
            directionIncrement = -1;
        }

        Vector2Int index = new Vector2Int(piece.CurrentIndex.x + directionIncrement, piece.CurrentIndex.y);
        if (index.x < 8)
        {
            piece.ValidMoves.Add(new Vector2Int(index.y, index.x));

            index.x += directionIncrement;
            if (piece.CurrentIndex.x == startIndex && index.x < 8)
            {
                piece.ValidMoves.Add(new Vector2Int(index.y, index.x));
            }
        }
    }

    public override void GeneratePossibleMoves(PieceBehaviour piece)
    {
        foreach (Vector2Int validMove in piece.ValidMoves)
        {
            if (BoardManager.Instance.TileSet[validMove.y, validMove.x].HeldPiece)
            {
                break;
            }

            piece.PossibleMoves.Add(validMove);
        }

        if (piece.ValidMoves.Count > 0)
        {
            Vector2Int firstMove = piece.ValidMoves[0];
            if (firstMove.x > 0)
            {
                Vector2Int move = new Vector2Int(firstMove.x - 1, firstMove.y);
                PieceBehaviour heldPiece = BoardManager.Instance.TileSet[move.y, move.x].HeldPiece;
                if (heldPiece && heldPiece.PieceData.PlayerType != piece.PieceData.PlayerType)
                {
                    piece.PossibleMoves.Add(move);
                }
            }

            if (firstMove.x < 7)
            {
                Vector2Int move = new Vector2Int(firstMove.x + 1, firstMove.y);
                PieceBehaviour heldPiece = BoardManager.Instance.TileSet[move.y, move.x].HeldPiece;
                if (heldPiece && heldPiece.PieceData.PlayerType != piece.PieceData.PlayerType)
                {
                    piece.PossibleMoves.Add(move);
                }
            }
        }
    }
}

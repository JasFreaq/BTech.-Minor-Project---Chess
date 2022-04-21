using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class PawnLogic : PieceLogic
{
    public override void GenerateValidMoves(PieceBehaviour piece)
    {
        int startIndex;
        int direction;
        if (piece.PieceData.PlayerType == PlayerType.White)
        {
            startIndex = 1;
            direction = 1;
        }
        else
        {
            startIndex = 6;
            direction = -1;
        }

        Vector2Int index = new Vector2Int(piece.CurrentIndex.x + direction, piece.CurrentIndex.y);
        if (index.x < 8)
        {
            piece.ValidMoves.Add(new Vector2Int(index.y, index.x));

            index.x += direction;
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
            ProcessEnPassant(piece);
        }
    }

    private static void ProcessEnPassant(PieceBehaviour piece)
    {
        int enPassantRow;
        if (piece.PieceData.PlayerType == PlayerType.White)
        {
            enPassantRow = 5;
        }
        else
        {
            enPassantRow = 2;
        }

        Vector2Int firstMove = piece.ValidMoves[0];
        if (firstMove.x > 0)
        {
            Vector2Int move = firstMove + new Vector2Int(-1, 0);
            Tile tile = BoardManager.Instance.TileSet[move.y, move.x];
            PieceBehaviour heldPiece = tile.HeldPiece;
            if ((heldPiece && heldPiece.PieceData.PlayerType != piece.PieceData.PlayerType)
                || (firstMove.y == enPassantRow && tile.EnPassant))
            {
                piece.PossibleMoves.Add(move);
            }
        }

        if (firstMove.x < 7)
        {
            Vector2Int move = firstMove + new Vector2Int(1, 0);
            Tile tile = BoardManager.Instance.TileSet[move.y, move.x];
            PieceBehaviour heldPiece = tile.HeldPiece;
            if ((heldPiece && heldPiece.PieceData.PlayerType != piece.PieceData.PlayerType)
                || (firstMove.y == enPassantRow && tile.EnPassant))
            {
                piece.PossibleMoves.Add(move);
            }
        }
    }
}

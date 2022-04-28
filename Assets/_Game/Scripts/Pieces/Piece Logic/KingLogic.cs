using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KingLogic : PieceLogic
{
    public override void GenerateValidMoves(PieceBehaviour piece)
    {
        List<Vector2Int> candidateIndices = new List<Vector2Int>(8);

        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                if (i == j && i == 1)
                    continue;

                Vector2Int index = piece.CurrentIndex + new Vector2Int(i - 1, j - 1);
                candidateIndices.Add(index);
            }
        }

        foreach (Vector2Int index in candidateIndices)
        {
            if (index.x >= 0 && index.x < 8 &&
                index.y >= 0 && index.y < 8)
            {
                piece.ValidMoves.Add(new Vector2Int(index.y, index.x));
            }
        }
    }

    public override void GeneratePossibleMoves(PieceBehaviour piece)
    {
        foreach (Vector2Int validMove in piece.ValidMoves)
        {
            Tile tile = BoardManager.Instance.TileSet[validMove.y, validMove.x];
            PieceBehaviour heldPiece = tile.HeldPiece;
            if (heldPiece)
            {
                if (heldPiece.PieceData.PlayerType != piece.PieceData.PlayerType)
                {
                    if (!tile.BishopBarrierActive)
                    {
                        piece.PossibleMoves.Add(validMove);
                    }
                }
            }
            else if (!tile.RookWallActive)
            {
                piece.PossibleMoves.Add(validMove);
            }
        }

        if (!piece.HasBeenMoved)
        {
            Vector2Int checkIndex = new Vector2Int(piece.CurrentIndex.x, piece.CurrentIndex.y + 3);
            PieceBehaviour heldPiece = BoardManager.Instance.TileSet[checkIndex.x, checkIndex.y].HeldPiece;
            
            if (heldPiece)
            {
                if (piece.PieceData.PlayerType == heldPiece.PieceData.PlayerType &&
                    heldPiece.PieceData.PieceType == PieceType.Rook && !heldPiece.HasBeenMoved)
                {
                    Vector2Int moveIndex = new Vector2Int(piece.CurrentIndex.x, piece.CurrentIndex.y + 2);
                    Vector2Int rookMoveIndex = new Vector2Int(piece.CurrentIndex.x, piece.CurrentIndex.y + 1);
                    if(!BoardManager.Instance.TileSet[moveIndex.x, moveIndex.y].HeldPiece &&
                       !BoardManager.Instance.TileSet[rookMoveIndex.x, rookMoveIndex.y].HeldPiece)
                    {
                        BoardManager.Instance.TileSet[moveIndex.x, moveIndex.y].Castling = true;
                        piece.PossibleMoves.Add(new Vector2Int(moveIndex.y, moveIndex.x));
                    }
                }
            }
        }
    }
}

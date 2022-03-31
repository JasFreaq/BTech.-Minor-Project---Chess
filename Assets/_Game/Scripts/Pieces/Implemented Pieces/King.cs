using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class King : ChessPiece
{
    public override void GenerateValidMoves()
    {
        _validMoves.Clear();

        List<Vector2Int> candidateIndices = new List<Vector2Int>(8);

        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                if (i == j && i == 1) 
                    continue;

                Vector2Int index = _currentIndex + new Vector2Int(i - 1, j - 1);
                candidateIndices.Add(index);
            }
        }

        foreach (Vector2Int index in candidateIndices)
        {
            if (index.x >= 0 && index.x < 8 &&
                index.y >= 0 && index.y < 8)
            {
                _validMoves.Add(new Vector2Int(index.y, index.x));
            }
        }
    }

    public override void GeneratePossibleMoves()
    {
        _possibleMoves.Clear();

        foreach (Vector2Int validMove in _validMoves)
        {
            ChessPiece piece = BoardManager.Instance.TileSet[validMove.y, validMove.x].HeldPiece;
            if (piece)
            {
                if (piece.PieceData.PlayerType != _pieceData.PlayerType)
                {
                    _possibleMoves.Add(validMove);
                }
            }
            else
            {
                _possibleMoves.Add(validMove);
            }
        }
    }
}

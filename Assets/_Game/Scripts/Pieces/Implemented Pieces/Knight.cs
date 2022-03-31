using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knight : ChessPiece
{
    public override void GenerateValidMoves()
    {
        _validMoves.Clear();

        List<Vector2Int> candidateIndices = new List<Vector2Int>(8);
        
        Vector2Int increment = new Vector2Int(2, 1);
        for (int i = 0; i < 4; i++)
        {
            Vector2Int index = _currentIndex + increment;
            candidateIndices.Add(index);

            increment.y *= -1;
            if (i % 2 != 0)
                increment.x *= -1;
        }

        increment = new Vector2Int(1, 2);
        for (int i = 4; i < 8; i++)
        {
            Vector2Int index = _currentIndex + increment;
            candidateIndices.Add(index);

            increment.x *= -1;
            if (i % 2 != 0)
                increment.y *= -1;
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

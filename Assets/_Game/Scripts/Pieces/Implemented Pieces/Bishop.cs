using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bishop : ChessPiece
{
    public override void GenerateValidMoves()
    {
        _validMoves.Clear();

        Vector2Int index = _currentIndex;
        while (index.x > 0 && index.y > 0)
        {
            index.x--;
            index.y--;
            _validMoves.Add(new Vector2Int(index.y, index.x));
        }
        _validMoves.Add(-Vector2Int.one);

        index = _currentIndex;
        while (index.x < 7 && index.y > 0)
        {
            index.x++;
            index.y--;
            _validMoves.Add(new Vector2Int(index.y, index.x));
        }
        _validMoves.Add(-Vector2Int.one);

        index = _currentIndex;
        while (index.x > 0 && index.y < 7)
        {
            index.x--;
            index.y++;
            _validMoves.Add(new Vector2Int(index.y, index.x));
        }
        _validMoves.Add(-Vector2Int.one);
        
        index = _currentIndex;
        while (index.x < 7 && index.y < 7)
        {
            index.x++;
            index.y++;
            _validMoves.Add(new Vector2Int(index.y, index.x));
        }
    }

    public override void GeneratePossibleMoves()
    {
        _possibleMoves.Clear();

        bool foundPiece = false;
        int findCount = 0;
        foreach (Vector2Int validMove in _validMoves)
        {
            if (validMove == -Vector2Int.one)
            {
                foundPiece = false;
                continue;
            }

            if (!foundPiece) 
            {
                ChessPiece piece = BoardManager.Instance.TileSet[validMove.y, validMove.x].HeldPiece;
                if (piece)
                {
                    if (piece.PieceData.PlayerType != _pieceData.PlayerType)
                    {
                        _possibleMoves.Add(validMove);
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
                    _possibleMoves.Add(validMove);
                }
            }
        }
    }
}

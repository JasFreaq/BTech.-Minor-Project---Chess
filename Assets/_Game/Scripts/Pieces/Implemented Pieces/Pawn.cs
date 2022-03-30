using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pawn : ChessPiece
{
    public override void GenerateValidMoves(Vector2Int pieceIndex)
    {
        _validMoves.Clear();

        int startIndex;
        int directionIncrement;
        if (_pieceData.PlayerType == PlayerType.White)
        {
            startIndex = 1;
            directionIncrement = 1;
        }
        else
        {
            startIndex = 6;
            directionIncrement = -1;
        }

        _validMoves.Add(new Vector2Int(pieceIndex.y, pieceIndex.x + directionIncrement));
        if (pieceIndex.x == startIndex)
        {
            _validMoves.Add(new Vector2Int(pieceIndex.y, pieceIndex.x + directionIncrement * 2));
        }
    }

    public override void GeneratePossibleMoves()
    {
        _possibleMoves = new List<Vector2Int>();
        foreach (Vector2Int validMove in _validMoves)
        {
            if (BoardManager.Instance.TileSet[validMove.y, validMove.x].HeldPiece)
            {
                break;
            }

            _possibleMoves.Add(validMove);
        }

        if (_validMoves[0].x > 0)
        {
            Vector2Int move = new Vector2Int(_validMoves[0].x - 1, _validMoves[0].y);
            ChessPiece piece = BoardManager.Instance.TileSet[move.y, move.x].HeldPiece;
            if (piece && piece.PieceData.PlayerType != _pieceData.PlayerType)
            {
                _possibleMoves.Add(move);
            }
        }

        if (_validMoves[0].x < 7)
        {
            Vector2Int move = new Vector2Int(_validMoves[0].x + 1, _validMoves[0].y);
            ChessPiece piece = BoardManager.Instance.TileSet[move.y, move.x].HeldPiece;
            if (piece && piece.PieceData.PlayerType != _pieceData.PlayerType)
            {
                _possibleMoves.Add(move);
            }
        }
    }
}

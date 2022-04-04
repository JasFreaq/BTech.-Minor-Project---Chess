using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RookLogic : MultidirectionalLogic
{
    public RookLogic()
    {
        _degreesOfMovement = 4;
    }

    public override void GenerateValidMoves(PieceBehaviour piece)
    {
        Vector2Int index = piece.CurrentIndex;
        while (index.x > 0)
        {
            index.x--;
            piece.ValidMoves.Add(new Vector2Int(index.y, index.x));
        }
        piece.ValidMoves.Add(-Vector2Int.one);

        index = piece.CurrentIndex;
        while (index.x < 7)
        {
            index.x++;
            piece.ValidMoves.Add(new Vector2Int(index.y, index.x));
        }
        piece.ValidMoves.Add(-Vector2Int.one);

        index = piece.CurrentIndex;
        while (index.y > 0)
        {
            index.y--;
            piece.ValidMoves.Add(new Vector2Int(index.y, index.x));
        }
        piece.ValidMoves.Add(-Vector2Int.one);

        index = piece.CurrentIndex;
        while (index.y < 7)
        {
            index.y++;
            piece.ValidMoves.Add(new Vector2Int(index.y, index.x));
        }
    }
}

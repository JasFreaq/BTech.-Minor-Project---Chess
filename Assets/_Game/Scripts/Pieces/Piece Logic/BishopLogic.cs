using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BishopLogic : MultidirectionalLogic
{
    public BishopLogic()
    {
        _degreesOfMovement = 4;
    }

    public override void GenerateValidMoves(ref List<Vector2Int> validMoves, Vector2Int currentIndex, PlayerType playerType)
    {
        Vector2Int index = currentIndex;
        while (index.x > 0 && index.y > 0)
        {
            index.x--;
            index.y--;
            validMoves.Add(new Vector2Int(index.y, index.x));
        }
        validMoves.Add(-Vector2Int.one);

        index = currentIndex;
        while (index.x < 7 && index.y > 0)
        {
            index.x++;
            index.y--;
            validMoves.Add(new Vector2Int(index.y, index.x));
        }
        validMoves.Add(-Vector2Int.one);

        index = currentIndex;
        while (index.x > 0 && index.y < 7)
        {
            index.x--;
            index.y++;
            validMoves.Add(new Vector2Int(index.y, index.x));
        }
        validMoves.Add(-Vector2Int.one);

        index = currentIndex;
        while (index.x < 7 && index.y < 7)
        {
            index.x++;
            index.y++;
            validMoves.Add(new Vector2Int(index.y, index.x));
        }
    }
}

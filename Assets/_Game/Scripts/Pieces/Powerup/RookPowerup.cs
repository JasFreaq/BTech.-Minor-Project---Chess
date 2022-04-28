using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RookPowerup : PowerupBehaviour
{
    public override void ProcessPowerup()
    {
        bool foundPiece = false;
        int findCount = 0;
        Tile[,] tileSet = BoardManager.Instance.TileSet;
        foreach (Vector2Int validMove in _owningPiece.ValidMoves)
        {
            if (validMove == -Vector2Int.one)
            {
                foundPiece = false;
                continue;
            }

            if (!foundPiece)
            {
                Tile tile = tileSet[validMove.y, validMove.x];
                if (tile.HeldPiece)
                {
                    foundPiece = true;
                    findCount++;

                    if (findCount == 4)
                    {
                        break;
                    }
                }
                else
                {
                    tile.ToggleRookWall(true);
                }
            }
        }
    }
}

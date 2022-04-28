using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BishopPowerup : PowerupBehaviour
{
    public override void ProcessPowerup()
    {
        Tile[,] tileSet = BoardManager.Instance.TileSet;
        foreach (Vector2Int validMove in _owningPiece.ValidMoves)
        {
            if (validMove != -Vector2Int.one)
            {
                Tile tile = tileSet[validMove.y, validMove.x];
                if (tile.HeldPiece &&
                    tile.HeldPiece.PieceData.PlayerType == _owningPiece.PieceData.PlayerType)
                {
                    tile.ToggleBishopBarrier(true);
                }
            }
        }
    }
}

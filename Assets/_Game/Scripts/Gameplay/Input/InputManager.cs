using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class InputManager : MonoBehaviour
{
    protected Tile _selectedTile;
    protected Vector2Int _selectedIndex;

    protected void SelectTile(Tile tile)
    {
        if (!_selectedTile)
        {
            _selectedTile = tile;
            _selectedIndex = _selectedTile.Index;
            _selectedTile.Select(true);
        }
        else
        {
            _selectedTile.Select(false);
            _selectedTile = null;
        }
    }
}

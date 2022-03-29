using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class InputManager : MonoBehaviour
{
    protected Tile _currentSelectedTile;
    protected Tile _previousSelectedTile;

    protected bool _currentlySelected;

    private void Update()
    {
        ProcessInput();
    }

    protected abstract void ProcessInput();

    protected void SetTileSelection(Tile tile)
    {
        if (_currentSelectedTile)
        {
            if (_currentSelectedTile == tile)
            {
                PieceManager.Instance.CurrentSelection = null;
                UnselectTile();
            }
            else
            {
                _previousSelectedTile = _currentSelectedTile;
                UnselectTile();
                SelectTile(tile);
            }
        }
        else
        {
            SelectTile(tile);
        }

        if (PieceManager.Instance.CurrentSelection)
        {
            MoveSelectedPiece();
            UnselectTile();
        }
        else
        {
            PieceManager.Instance.CurrentSelection = _currentSelectedTile.HeldPiece;
        }
    }

    private void MoveSelectedPiece()
    {
        if (_currentSelectedTile.HeldPiece)
        {
            PieceManager.Instance.CapturePiece(_currentSelectedTile.HeldPiece);
        }

        _previousSelectedTile.HeldPiece = null;
        _currentSelectedTile.HeldPiece = PieceManager.Instance.CurrentSelection;

        PieceManager.Instance.MoveCurrentSelectedPiece(_currentSelectedTile.Index);
    }

    protected void SelectTile(Tile tile)
    {
        _currentSelectedTile = tile;
        _currentSelectedTile.SetSelection(true);
        _currentlySelected = true;
    }

    protected void UnselectTile()
    {
        _currentSelectedTile.SetSelection(false);
        _currentSelectedTile = null;
        _currentlySelected = false;
    }
}

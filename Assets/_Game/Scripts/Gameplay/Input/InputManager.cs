using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class InputManager : MonoBehaviour
{
    protected Tile _currentSelectedTile;
    protected Tile _previousSelectedTile;

    protected bool _isPieceSelected;

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
                PieceManager.Instance.UnselectPiece();
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
            PieceManager.Instance.SelectPiece(_currentSelectedTile.HeldPiece, _currentSelectedTile.Index);
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

        PieceManager.Instance.MoveSelectedPieceToTile(_currentSelectedTile.Index);
        BoardManager.Instance.ResetHighlightedTiles();
    }

    protected void SelectTile(Tile tile)
    {
        _currentSelectedTile = tile;
        _currentSelectedTile.SetSelection(true);
        _isPieceSelected = true;
    }

    protected void UnselectTile()
    {
        _currentSelectedTile.SetSelection(false);
        _currentSelectedTile = null;
        _isPieceSelected = false;
    }
}

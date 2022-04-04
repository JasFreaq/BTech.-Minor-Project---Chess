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
            if (_currentSelectedTile.HeldPiece)
            {
                if (PieceManager.Instance.CurrentSelection.PieceData.PlayerType ==
                    _currentSelectedTile.HeldPiece.PieceData.PlayerType)
                {
                    PieceManager.Instance.SelectPiece(_currentSelectedTile.HeldPiece, _currentSelectedTile.Index);
                }
                else
                {
                    MoveSelectedPiece();
                }
            }
            else
            {
                MoveSelectedPiece();
            }
        }
        else if (_currentSelectedTile) 
        {
            PieceManager.Instance.SelectPiece(_currentSelectedTile.HeldPiece, _currentSelectedTile.Index);
        }
    }

    private void MoveSelectedPiece()
    {
        PieceBehaviour capturedPiece = _currentSelectedTile.HeldPiece;
        _previousSelectedTile.HeldPiece = null;
        _currentSelectedTile.HeldPiece = PieceManager.Instance.CurrentSelection;

        if (PieceManager.Instance.CurrentSelection.PieceData.PieceType == PieceType.King
            && _currentSelectedTile.Castling)
        {
            PieceManager.Instance.PerformCastling();
        }
        PieceManager.Instance.MoveSelectedPiece(_currentSelectedTile.Index);

        if (capturedPiece)
        {
            PieceManager.Instance.CapturePiece(capturedPiece);
        }
        
        BoardManager.Instance.ResetHighlightedTiles();
        UnselectTile(true);
    }

    protected void SelectTile(Tile tile)
    {
        _currentSelectedTile = tile;
        _currentSelectedTile.SetSelection(true);
        _isPieceSelected = true;
    }

    protected void UnselectTile(bool movedToTile = false)
    {
        if (movedToTile) 
            _currentSelectedTile.ResetStates();
        _currentSelectedTile.SetSelection(false);

        _currentSelectedTile = null;
        _isPieceSelected = false;
    }
}

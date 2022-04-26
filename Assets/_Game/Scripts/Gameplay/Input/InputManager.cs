using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class InputManager : MonoBehaviour
{
    [SerializeField] protected PlayerType _playerTeam;

    protected Tile _currentSelectedTile;
    protected Tile _previousSelectedTile;

    protected bool _isPieceSelected;

    private void Update()
    {
        if (!GameplayManager.Instance.GameOver)
        {
            ProcessInput();
        }
    }

    protected abstract void ProcessInput();

    public void SetTileSelection(Tile tile)
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
                    StartCoroutine(MoveSelectedPieceRoutine());
                }
            }
            else
            {
                StartCoroutine(MoveSelectedPieceRoutine());
            }
        }
        else if (_currentSelectedTile) 
        {
            PieceManager.Instance.SelectPiece(_currentSelectedTile.HeldPiece, _currentSelectedTile.Index);
        }
    }

    private IEnumerator MoveSelectedPieceRoutine()
    {
        PieceBehaviour capturedPiece;
        if (_currentSelectedTile.EnPassant &&
            PieceManager.Instance.CurrentSelection.PieceData.PieceType == PieceType.Pawn)
        {
            int direction;
            if (PieceManager.Instance.CurrentSelection.PieceData.PlayerType == PlayerType.White)
            {
                direction = -1;
            }
            else
            {
                direction = 1;
            }

            Vector2Int enPassantCaptureIndex = _currentSelectedTile.Index + new Vector2Int(direction, 0);
            Tile enPassantCaptureTile = BoardManager.Instance.TileSet[enPassantCaptureIndex.x, enPassantCaptureIndex.y];
            capturedPiece = enPassantCaptureTile.HeldPiece;
            enPassantCaptureTile.HeldPiece = null;

        }
        else
        {
            capturedPiece = _currentSelectedTile.HeldPiece;
        }

        _previousSelectedTile.HeldPiece = null;
        _currentSelectedTile.HeldPiece = PieceManager.Instance.CurrentSelection;

        if (capturedPiece)
        {
            PieceManager.Instance.CapturePiece(capturedPiece);
        }

        BoardManager.Instance.ResetHighlightedTiles();
        PieceBehaviour currentSelection = PieceManager.Instance.CurrentSelection;
        Tile currentSelectedTile = _currentSelectedTile;
        UnselectTile(true);

        yield return GameplayManager.Instance.MoveSelectedPieceRoutine(currentSelectedTile.Index);
        yield return ProcessSpecialMovesRoutine(currentSelection, currentSelectedTile);
        PieceManager.Instance.SetPossibleMoves();
        yield return TurnManager.Instance.EndTurnRoutine();
    }

    private IEnumerator ProcessSpecialMovesRoutine(PieceBehaviour currentSelection, Tile currentSelectedTile)
    {
        switch (currentSelection.PieceData.PieceType)
        {
            case PieceType.Pawn:
                yield return ProcessPawnSpecialMovesRoutine(currentSelection, currentSelectedTile);
                break;

            case PieceType.King:
                yield return ProcessKingSpecialMovesRoutine(currentSelection, currentSelectedTile);
                break;
        }
    }

    private IEnumerator ProcessPawnSpecialMovesRoutine(PieceBehaviour currentSelection, Tile currentSelectedTile)
    {
        Vector2Int previousIndex = _previousSelectedTile.Index;
        Vector2Int currentIndex = currentSelectedTile.Index;
        if (Mathf.Abs(previousIndex.x - currentIndex.x) == 2)
        {
            GameplayManager.Instance.EnableEnPassant(currentSelection, currentIndex);
        }

        if (currentIndex.x == 0 || currentIndex.x == 7)
        {
            yield return GameplayManager.Instance.ProcessPromotionRoutine(currentSelection);
        }
    }

    private IEnumerator ProcessKingSpecialMovesRoutine(PieceBehaviour currentSelection, Tile currentSelectedTile)
    {
        if (currentSelectedTile.Castling)
        {
            yield return GameplayManager.Instance.PerformCastlingRoutine(currentSelection);
        }
    }

    protected void SelectTile(Tile tile)
    {
        _currentSelectedTile = tile;
        _currentSelectedTile.SetSelection(true);
        _isPieceSelected = true;
    }

    public virtual void UnselectTile(bool movedToTile = false)
    {
        if (movedToTile) 
            _currentSelectedTile.ResetStates();
        _currentSelectedTile.SetSelection(false);

        _currentSelectedTile = null;
        _isPieceSelected = false;
    }
}

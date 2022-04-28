using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerInputManager : InputManager
{
    [SerializeField] private float _rayDistance = 20f;
    [SerializeField] private LayerMask _rayLayerMask;

    private Camera _mainCamera;
    
    private Tile _hoveredTile;

    private bool _overUI;

    public bool OverUI
    {
        set => _overUI = value;
    }

    void Start()
    {
        _mainCamera = Camera.main;
    }
    
    protected override void ProcessInput()
    {
        if (!_overUI)
            Process3DInput();
    }

    private void Process3DInput()
    {
        Ray ray = _mainCamera.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out RaycastHit hit, _rayDistance, _rayLayerMask))
        {
            bool foundValidMove = false;
            Tile currentHoveredTile = BoardManager.Instance.TileRefs[hit.collider.GetInstanceID()];

            if (PieceManager.Instance.CurrentSelection)
            {
                List<Vector2Int> possibleMoves = PieceManager.Instance.CurrentSelection.PossibleMoves;
                Vector2Int tileIndex = new Vector2Int(currentHoveredTile.Index.y, currentHoveredTile.Index.x);

                foreach (Vector2Int possibleMove in possibleMoves)
                {
                    if (tileIndex == possibleMove)
                    {
                        HoverTile(currentHoveredTile);
                        foundValidMove = true;
                        break;
                    }
                }

                if (!foundValidMove)
                    UnhoverTile();
            }

            if (currentHoveredTile.HeldPiece
                && currentHoveredTile.HeldPiece.PieceData.PlayerType == _playerTeam)
            {
                HoverTile(currentHoveredTile);
            }
            else if (!foundValidMove)
            {
                UnhoverTile();
            }
        }
        else
        {
            UnhoverTile();
        }

        if (Input.GetMouseButtonDown(0))
        {
            if (_hoveredTile)
            {
                SetTileSelection(_hoveredTile);
                _hoveredTile = null;
            }
            else if (PieceManager.Instance.CurrentSelection)
            {
                UnselectTile();
                PieceManager.Instance.UnselectPiece();
            }
        }
    }

    private void HoverTile(Tile currentHoveredTile)
    {
        if (_hoveredTile != currentHoveredTile)
        {
            if (_hoveredTile)
                _hoveredTile.SetHover(PieceManager.Instance.CurrentSelection, false);

            _hoveredTile = currentHoveredTile;
            _hoveredTile.SetHover(PieceManager.Instance.CurrentSelection, true);
        }
    }
    
    private void UnhoverTile()
    {
        if (_hoveredTile)
        {
            _hoveredTile.SetHover(PieceManager.Instance.CurrentSelection, false);
            _hoveredTile = null;
        }
    }

    public override void UnselectTile()
    {
        UnhoverTile();

        if (_currentSelectedTile) 
        {
            base.UnselectTile();
        }
    }
}

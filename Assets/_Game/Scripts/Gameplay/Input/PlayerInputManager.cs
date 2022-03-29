using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerInputManager : InputManager
{
    [SerializeField] private float _rayDistance = 20f;
    [SerializeField] private LayerMask _rayLayerMask;

    private Camera _mainCamera;

    private PlayerType _playerTeam;

    private Tile _hoveredTile;

    void Start()
    {
        _mainCamera = Camera.main;
    }
    
    protected override void ProcessInput()
    {
        Ray ray = _mainCamera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit, _rayDistance, _rayLayerMask))
        {
            Tile currentHoveredTile = BoardManager.Instance.TileRefs[hit.collider.GetInstanceID()];

            HoverTile(currentHoveredTile);
            //if (PieceManager.Instance.CurrentSelection)
            //{

            //}
            //else if (currentHoveredTile.HeldPiece 
            //    && currentHoveredTile.HeldPiece.PieceData.PlayerType == _playerTeam) 
            //{
            //    HoverTile(currentHoveredTile);
            //}
            //else
            //{
            //    UnhoverTile();
            //}
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
            else if (_currentlySelected)
            {
                UnselectTile();
                PieceManager.Instance.CurrentSelection = null;
            }
        }
    }

    private void HoverTile(Tile currentHoveredTile)
    {
        if (_hoveredTile != currentHoveredTile)
        {
            if (_hoveredTile)
                _hoveredTile.SetHover(_currentlySelected, false);

            _hoveredTile = currentHoveredTile;
            _hoveredTile.SetHover(_currentlySelected, true);
        }
    }
    
    private void UnhoverTile()
    {
        if (_hoveredTile)
        {
            _hoveredTile.SetHover(_currentlySelected, false);
            _hoveredTile = null;
        }
    }
}

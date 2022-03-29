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

    void Start()
    {
        _mainCamera = Camera.main;
    }

    void Update()
    {
        bool selectionMade = _selectedTile != null;
        Ray ray = _mainCamera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit, _rayDistance, _rayLayerMask))
        {
            Tile currentHoveredTile = BoardManager.Instance.TileRefs[hit.collider.GetInstanceID()];
            if (_hoveredTile != currentHoveredTile)
            {

                if (_hoveredTile)
                    _hoveredTile.Hover(selectionMade, false);

                _hoveredTile = currentHoveredTile;
                _hoveredTile.Hover(selectionMade, true);
            }

            if (Input.GetMouseButtonDown(0))
            {
                SelectTile(_hoveredTile);
                _hoveredTile = null;
            }
        }
        else if (_hoveredTile)
        {
            _hoveredTile.Hover(selectionMade, false);
            _hoveredTile = null;
        }
    }
}

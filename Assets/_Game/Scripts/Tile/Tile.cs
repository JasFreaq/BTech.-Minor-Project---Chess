using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tile : MonoBehaviour
{
    [SerializeField] private GameObject _highlightBorder;

    private BoxCollider _collider;
    private Image _image;

    private bool _isSelected;
    
    public BoxCollider Collider => _collider;

    public Vector2Int Index { get; set; }

    public ChessPiece HeldPiece { get; set; }

    private void Awake()
    {
        _collider = GetComponent<BoxCollider>();
        _image = GetComponent<Image>();
    }

    public void SetHover(bool selectionMade, bool hovered)
    {
        if (hovered)
        {
            if (!selectionMade)
            {
                _image.material = SceneParamsHolder.Instance.HoverMat;
                _image.color = new Color(0, 0, 0, 1);
            }

            _highlightBorder.SetActive(true);
        }
        else
        {
            if (!selectionMade)
            {
                _image.color = new Color(0, 0, 0, 0);
            }

            _highlightBorder.SetActive(false);
        }
    }
    
    public void SetSelection(bool selected)
    {
        _isSelected = selected;
        if (_isSelected)
        {
            Select();
        }
        else
        {
            _image.color = new Color(0, 0, 0, 0);
        }
    }

    private void Select()
    {
        _image.material = SceneParamsHolder.Instance.SelectMat;
        _image.color = new Color(0, 0, 0, 1);
    }
}

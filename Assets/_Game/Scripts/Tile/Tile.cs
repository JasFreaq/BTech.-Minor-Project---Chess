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

    private Material _hoverMat;
    private Material _selectMat;
    private Material _validMat;
    private Material _captureMat;

    public BoxCollider Collider => _collider;

    public Vector2Int Index { get; set; }

    public PieceBehaviour HeldPiece { get; set; }

    private void Awake()
    {
        _collider = GetComponent<BoxCollider>();
        _image = GetComponent<Image>();
    }

    private void Start()
    {
        _hoverMat = SceneParamsHolder.Instance.HoverMat;
        _selectMat = SceneParamsHolder.Instance.SelectMat;
        _validMat = SceneParamsHolder.Instance.ValidMat;
        _captureMat = SceneParamsHolder.Instance.CaptureMat;
    }

    public void SetHover(bool selectionMade, bool hovered)
    {
        if (hovered)
        {
            if (!selectionMade)
            {
                _image.material = _hoverMat;
                _image.color = new Color(0, 0, 0, 1);
            }

            _highlightBorder.SetActive(true);
        }
        else
        {
            if (!selectionMade)
            {
                Reset();
            }

            _highlightBorder.SetActive(false);
        }
    }
    
    public void SetSelection(bool selected)
    {
        _isSelected = selected;
        if (_isSelected)
        {
            _image.material = _selectMat;
            _image.color = new Color(0, 0, 0, 1);
        }
        else
        {
            Reset();
        }
    }

    public void SetValid(bool valid)
    {
        if (valid)
        {
            _image.material = _validMat;
            _image.color = new Color(0, 0, 0, 1);
        }
        else
        {
            Reset();
        }
    }

    public void SetCapturable(bool capturable)
    {
        if (capturable)
        {
            _image.material = _captureMat;
            _image.color = new Color(0, 0, 0, 1);
        }
        else
        {
            Reset();
        }
    }

    public void Reset()
    {
        _isSelected = false;
        _image.color = new Color(0, 0, 0, 0);
        _highlightBorder.SetActive(false);
    }
}

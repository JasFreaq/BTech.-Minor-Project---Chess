using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tile : MonoBehaviour
{
    private static readonly float VisibleAlpha = 0.6078f;
    
    [SerializeField] private GameObject _highlightBorder;

    private BoxCollider _collider;
    private Image _image;

    private bool _isSelected, _isValid, _isCapturable;
    private Material _hoverMat, _selectMat, _validMat, _captureMat;

    private bool _enPassant;
    private int _enPassantCounter;

    public BoxCollider Collider => _collider;

    public Vector2Int Index { get; set; }

    public PieceBehaviour HeldPiece { get; set; }

    public bool Castling { get; set; }

    public bool EnPassant
    {
        get => _enPassant;
        set
        {
            _enPassant = value;
            _enPassantCounter = 0;
        }
    }

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
        bool isMoveCandidate = _isSelected || _isValid || _isCapturable;
        if (hovered)
        {
            if (!isMoveCandidate) 
            {
                _image.material = _hoverMat;
                _image.color = new Color(0, 0, 0, VisibleAlpha);
            }

            _highlightBorder.SetActive(true);
        }
        else
        {
            if (!isMoveCandidate) 
            {
                ResetRepresentation();
            }

            if (!_isSelected)
                _highlightBorder.SetActive(false);
        }
    }
    
    public void SetSelection(bool selected)
    {
        _isSelected = selected;
        if (_isSelected)
        {
            _image.material = _selectMat;
            _image.color = new Color(0, 0, 0, VisibleAlpha);
        }
        else
        {
            ResetRepresentation();
        }
    }

    public void SetValid(bool valid)
    {
        _isValid = valid;
        if (valid)
        {
            _image.material = _validMat;
            _image.color = new Color(0, 0, 0, VisibleAlpha);
        }
        else
        {
            ResetRepresentation();
        }
    }

    public void SetCapturable(bool capturable)
    {
        _isCapturable = capturable;
        if (capturable)
        {
            _image.material = _captureMat;
            _image.color = new Color(0, 0, 0, VisibleAlpha);
        }
        else
        {
            ResetRepresentation();
        }
    }

    public void ResetStates()
    {
        _isSelected = false;
        _isValid = false;
        _isCapturable = false;
    }

    public void ResetRepresentation()
    {
        _image.color = new Color(0, 0, 0, 0);
        _highlightBorder.SetActive(false);
    }

    public void ProcessEnPassant()
    {
        if (_enPassantCounter > 0)
        {
            _enPassant = false;
        }

        if (_enPassant) 
        {
            _enPassantCounter++;
        }
    }
}

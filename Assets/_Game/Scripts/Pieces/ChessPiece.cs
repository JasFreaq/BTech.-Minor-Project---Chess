using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChessPiece : MonoBehaviour
{
    [SerializeField] protected PieceData _pieceData;
    [SerializeField] private Vector2Int[] _startIndices;

    private Vector3 _initialPosition, _initialScale;
    private Vector3 _desiredPosition, _desiredScale;
    private float _moveSpeed, _scaleSpeed;
    private float _moveStartTime, _scaleStartTime;
    private bool _currentlyMoving, _currentlyScaling;

    public PieceData PieceData => _pieceData;

    public Vector2Int[] StartIndices => _startIndices;

    private void Start()
    {
        _moveSpeed = SceneParamsHolder.Instance.PieceMoveSpeed;
        _scaleSpeed = SceneParamsHolder.Instance.PieceScaleSpeed;
    }

    private void Update()
    {
        UpdatePosition();
        UpdateScale();
    }

    private void UpdatePosition()
    {
        if (_currentlyMoving)
        {
            float t = (Time.time - _moveStartTime) * _moveSpeed;
            transform.position = Vector3.Lerp(_initialPosition, _desiredPosition, t);

            if (Vector3.Distance(transform.position, _desiredPosition) < Mathf.Epsilon)
            {
                transform.position = _desiredPosition;
                _currentlyMoving = false;
            }
        }
    }
    
    private void UpdateScale()
    {
        if (_currentlyScaling)
        {
            float t = (Time.time - _scaleStartTime) * _scaleSpeed;
            transform.localScale = Vector3.Lerp(_initialScale, _desiredScale, t);

            if ((transform.localScale - _desiredScale).magnitude < Mathf.Epsilon)
            {
                transform.localScale = _desiredScale;
                _currentlyScaling = false;
            }
        }
    }

    public virtual void SetPosition(Vector3 position, bool teleport = false)
    {
        _initialPosition = transform.position;
        _desiredPosition = position;
        _moveStartTime = Time.time;
        _currentlyMoving = true;

        if (teleport)
        {
            transform.position = _desiredPosition;
            _currentlyMoving = false;
        }
    }
    
    public virtual void SetScale(Vector3 scale, bool force = false)
    {
        _initialScale = transform.localScale;
        _desiredScale = scale;
        _scaleStartTime = Time.time;
        _currentlyScaling = true;

        if (force)
        {
            transform.localScale = _desiredScale;
            _currentlyScaling = false;
        }
    }
}

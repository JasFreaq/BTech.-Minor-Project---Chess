using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PieceBehaviour : MonoBehaviour
{
    [SerializeField] private PieceData _pieceData;
    [SerializeField] private Vector2Int[] _startIndices;

    private PieceLogic _pieceLogic;
    private Vector2Int _currentIndex;
    protected List<Vector2Int> _validMoves = new List<Vector2Int>();
    protected List<Vector2Int> _possibleMoves = new List<Vector2Int>();

    private Vector3 _initialPosition, _initialScale;
    private Vector3 _desiredPosition, _desiredScale;
    private float _moveTime, _scaleTime;
    private float _moveStartTime, _scaleStartTime;
    private bool _currentlyMoving, _currentlyScaling;

    public PieceData PieceData => _pieceData;

    public Vector2Int[] StartIndices => _startIndices;

    public Vector2Int CurrentIndex { set => _currentIndex = value; }

    public List<Vector2Int> PossibleMoves => _possibleMoves;

    private void Start()
    {
        _moveTime = SceneParamsHolder.Instance.PieceMoveTime;
        _scaleTime = SceneParamsHolder.Instance.PieceScaleTime;

        _pieceLogic = GameplayManager.Instance.GetPieceLogic(_pieceData.PieceType);
        SetValidMoves();
    }

    private void Update()
    {
        UpdatePosition();
        UpdateScale();
    }

    public void SetValidMoves()
    {
        _validMoves.Clear();
        _pieceLogic.GenerateValidMoves(ref _validMoves, _currentIndex, _pieceData.PlayerType);
    }

    public void SetPossibleMoves()
    {
        _possibleMoves.Clear();
        _pieceLogic.GeneratePossibleMoves(ref _possibleMoves, ref _validMoves, _pieceData.PlayerType);
    }

    private void UpdatePosition()
    {
        if (_currentlyMoving)
        {
            float t = (Time.time - _moveStartTime) / _moveTime;
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
            float t = (Time.time - _scaleStartTime) / _scaleTime;
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

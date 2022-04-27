using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PieceBehaviour : MonoBehaviour
{
    [SerializeField] private PieceData _pieceData;
    [SerializeField] private Vector2Int[] _startIndices;

    private PieceLogic _pieceLogic;
    private PowerupBehaviour _powerupBehaviour;
    protected List<Vector2Int> _validMoves = new List<Vector2Int>();
    protected List<Vector2Int> _possibleMoves = new List<Vector2Int>();

    private float _moveTime, _scaleTime;

    public PieceData PieceData => _pieceData;

    public Vector2Int[] StartIndices => _startIndices;

    public Vector2Int CurrentIndex { get; set; }

    public PowerupBehaviour PowerupBehaviour => _powerupBehaviour;

    public List<Vector2Int> ValidMoves => _validMoves;
    
    public List<Vector2Int> PossibleMoves => _possibleMoves;

    public bool HasBeenMoved { get; set; }

    private void Awake()
    {
        _powerupBehaviour = GetComponent<PowerupBehaviour>();
    }

    private void Start()
    {
        _moveTime = SceneParamsHolder.Instance.PieceMoveTime;
        _scaleTime = SceneParamsHolder.Instance.PieceScaleTime;

        _pieceLogic = GameplayManager.Instance.GetPieceLogic(_pieceData.PieceType);
        SetValidMoves();
        SetPossibleMoves();
    }
    
    public void SetValidMoves()
    {
        _validMoves.Clear();
        _pieceLogic.GenerateValidMoves(this);
    }

    public void SetPossibleMoves()
    {
        _possibleMoves.Clear();
        _pieceLogic.GeneratePossibleMoves(this);
    }
    
    public IEnumerator SetPositionRoutine(Vector3 position, bool teleport = false)
    {
        Vector3 desiredPosition = position;
        
        if (teleport)
        {
            transform.position = desiredPosition;
        }
        else
        {
            Vector3 initialPosition = transform.position;
            float moveStartTime = Time.time;
            bool currentlyMoving = true;

            while (currentlyMoving)
            {
                float t = (Time.time - moveStartTime) / _moveTime;
                transform.position = Vector3.Lerp(initialPosition, desiredPosition, t);

                yield return new WaitForEndOfFrame();

                if (Vector3.Distance(transform.position, desiredPosition) < Mathf.Epsilon)
                {
                    transform.position = desiredPosition;
                    currentlyMoving = false;
                }
            }
        }
    }
    
    public IEnumerator SetScaleRoutine(Vector3 scale, bool force = false)
    {
        Vector3 desiredScale = scale;

        if (force)
        {
            transform.localScale = desiredScale;
        }
        else
        {
            Vector3 initialScale = transform.localScale;
            float scaleStartTime = Time.time;
            bool currentlyScaling = true;

            while (currentlyScaling)
            {
                float t = (Time.time - scaleStartTime) / _scaleTime;
                transform.localScale = Vector3.Lerp(initialScale, desiredScale, t);

                yield return new WaitForEndOfFrame();

                if ((transform.localScale - desiredScale).magnitude < Mathf.Epsilon)
                {
                    transform.localScale = desiredScale;
                    currentlyScaling = false;
                }
            }
        }
    }
}

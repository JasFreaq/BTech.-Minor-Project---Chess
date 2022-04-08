using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

public class TurnManager : MonoBehaviour
{
    [SerializeField] private InputManager _whitePlayer;
    [SerializeField] private InputManager _blackPlayer;
    [SerializeField] private Transform _whiteCameraTransform;
    [SerializeField] private Transform _blackCameraTransform;
    [SerializeField] private float _lerpTime = 3;

    private Transform _mainCamera;
    private PlayerType _currentPlayerType = PlayerType.White;
    private InputManager _currentPlayer;

    #region Singleton Pattern

    private static TurnManager _instance;

    public static TurnManager Instance => _instance;

    #endregion

    private void Awake()
    {
        _instance = this;
    }

    private void Start()
    {
        _mainCamera = Camera.main.transform;
        
        _currentPlayer = _whitePlayer;
        _blackPlayer.enabled = false;
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.Space))
            EndTurn();
    }

    public void EndTurn()
    {
        StartCoroutine(EndTurnRoutine());

        BoardManager.Instance.ProcessEnPassant();
    }

    private IEnumerator EndTurnRoutine()
    {
        _currentPlayer.UnselectTile();
        _currentPlayer.enabled = false;

        Transform srcTransform;
        Transform destTransform;
        PlayerType nextTurnPlayerType;
        InputManager nextTurnPlayer;

        if (_currentPlayerType == PlayerType.White)
        {
            srcTransform = _whiteCameraTransform;
            destTransform = _blackCameraTransform;
            nextTurnPlayerType = PlayerType.Black;
            nextTurnPlayer = _blackPlayer;
        }
        else
        {
            srcTransform = _blackCameraTransform;
            destTransform = _whiteCameraTransform;
            nextTurnPlayerType = PlayerType.White;
            nextTurnPlayer = _whitePlayer;
        }

        yield return PanCameraRoutine(srcTransform, destTransform);

        _mainCamera.position = destTransform.position;
        _mainCamera.rotation = destTransform.rotation;
        _currentPlayerType = nextTurnPlayerType;

        _currentPlayer = nextTurnPlayer;
        _currentPlayer.enabled = true;
    }

    private IEnumerator PanCameraRoutine(Transform srcTransform, Transform destTransform)
    {
        float t = 0;
        while (_lerpTime - t > Mathf.Epsilon)
        {
            t += Time.deltaTime;

            _mainCamera.position =
                Vector3.Lerp(srcTransform.position, destTransform.position, t / _lerpTime);
            _mainCamera.rotation =
                Quaternion.Lerp(srcTransform.rotation, destTransform.rotation, t / _lerpTime);

            yield return new WaitForEndOfFrame();
        }
    }
}

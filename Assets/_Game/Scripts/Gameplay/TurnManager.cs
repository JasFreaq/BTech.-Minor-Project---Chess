using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.UI;

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

    public PlayerType CurrentPlayerType => _currentPlayerType;

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
        
    public IEnumerator EndTurnRoutine()
    {
        if (!GameplayManager.Instance.GameOver)
        {
            UIManager.Instance.TogglePieceButtons(false);
            BoardManager.Instance.ProcessEnPassant();

            _currentPlayer.UnselectTile();
            _currentPlayer.enabled = false;

            Transform srcTransform, destTransform;
            Color srcColor, destColor;

            PlayerType nextTurnPlayerType;
            InputManager nextTurnPlayer;

            if (_currentPlayerType == PlayerType.White)
            {
                srcTransform = _whiteCameraTransform;
                destTransform = _blackCameraTransform;
                srcColor = Color.white;
                destColor = Color.black;

                nextTurnPlayerType = PlayerType.Black;
                nextTurnPlayer = _blackPlayer;
            }
            else
            {
                srcTransform = _blackCameraTransform;
                destTransform = _whiteCameraTransform;
                srcColor = Color.black;
                destColor = Color.white;

                nextTurnPlayerType = PlayerType.White;
                nextTurnPlayer = _whitePlayer;
            }

            _mainCamera.position = destTransform.position;
            _mainCamera.rotation = destTransform.rotation;
            _currentPlayerType = nextTurnPlayerType;

            UIManager.Instance.FadeColor(srcColor, destColor, _lerpTime);
            yield return PanCameraRoutine(srcTransform, destTransform);

            _currentPlayer = nextTurnPlayer;
            _currentPlayer.enabled = true;
        }
    }

    

    private IEnumerator PanCameraRoutine(Transform srcTransform, Transform destTransform)
    {
        float time = 0;
        while (_lerpTime - time > Mathf.Epsilon)
        {
            time += Time.deltaTime;
            float t = time / _lerpTime;

            _mainCamera.position = Vector3.Lerp(srcTransform.position, destTransform.position, t);
            _mainCamera.rotation = Quaternion.Lerp(srcTransform.rotation, destTransform.rotation, t);
            
            yield return new WaitForEndOfFrame();
        }
    }
}

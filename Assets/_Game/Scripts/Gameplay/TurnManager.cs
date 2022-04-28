using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TurnManager : MonoBehaviour
{
    [SerializeField] private PlayerInputManager _whitePlayer;
    [SerializeField] private PlayerInputManager _blackPlayer;
    [SerializeField] private Transform _whiteCameraTransform;
    [SerializeField] private Transform _blackCameraTransform;
    [SerializeField] private float _lerpTime = 3;
    
    private Transform _mainCamera;
    private PlayerInputManager _currentPlayer;
    private PlayerType _currentPlayerType = PlayerType.White;

    private Action _onTurnOver;

    #region Singleton Pattern

    private static TurnManager _instance;

    public static TurnManager Instance => _instance;

    #endregion

    public InputManager CurrentPlayer => _currentPlayer;

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
        
    public void ToggleCurrentPlayerInput(bool toggle)
    {
        _currentPlayer.OverUI = toggle;
    }

    public IEnumerator EndTurnRoutine(float pauseTime = 0)
    {
        if (!GameplayManager.Instance.GameOver)
        {
            UIManager.Instance.TogglePieceButtonsWrapper(false);
            BoardManager.Instance.ProcessConditions();

            _currentPlayer.UnselectTile();
            _currentPlayer.enabled = false;

            Transform srcTransform, destTransform;
            Color srcColor, destColor;

            PlayerType nextTurnPlayerType;
            PlayerInputManager nextTurnPlayer;

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

            _currentPlayerType = nextTurnPlayerType;

            _onTurnOver.Invoke();

            yield return new WaitForSeconds(pauseTime);

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

        _mainCamera.position = destTransform.position;
        _mainCamera.rotation = destTransform.rotation;
    }

    public void AssignOnTurnOver(Action action)
    {
        _onTurnOver += action;
    }
    
    public void UnassignOnTurnOver(Action action)
    {
        _onTurnOver -= action;
    }
}

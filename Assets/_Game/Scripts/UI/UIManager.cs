using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [Header("Economy")]
    [SerializeField] private Text _currencyText;
    [SerializeField] private Image _taskBarImage;

    [Header("Game Over")]
    [SerializeField] private GameObject _whiteWinPanel;
    [SerializeField] private GameObject _blackWinPanel;
    [SerializeField] private GameObject _gameOverOptionsPanel;

    [Header("Button Icons")] 
    [SerializeField] private Image _powerKingImage;
    [SerializeField] private Image _shieldRookImage;
    [SerializeField] private Image _barrierBishopImage;

    [SerializeField] private Sprite _whitePowerKingSprite;
    [SerializeField] private Sprite _blackPowerKingSprite;
    [SerializeField] private Sprite _whiteShieldRookSprite;
    [SerializeField] private Sprite _blackShieldRookSprite;
    [SerializeField] private Sprite _whiteBarrierBishopSprite;
    [SerializeField] private Sprite _blackBarrierBishopSprite;

    [Header("Piece Buttons")] 
    [SerializeField] private GameObject _piecesButton;
    [SerializeField] private GameObject _pieceButtonsHolder;
    [SerializeField] private ImageSwitchButton[] _imageSwitchButtons = new ImageSwitchButton[5];

    #region Singleton Pattern

    private static UIManager _instance;

    public static UIManager Instance => _instance;

    #endregion

    private void Awake()
    {
        _instance = this;
    }

    public void FadeColor(Color srcColor, Color destColor, float lerpTime)
    {
        StartCoroutine(FadeColorRoutine(srcColor, destColor, lerpTime));
    }

    private IEnumerator FadeColorRoutine(Color srcColor, Color destColor, float lerpTime)
    {
        float time = 0;
        bool fadedAmount = false;
        while (lerpTime - time > Mathf.Epsilon)
        {
            time += Time.deltaTime;
            float t = time / lerpTime;

            _currencyText.color = Color.Lerp(destColor, srcColor, t);
            _taskBarImage.color = Color.Lerp(srcColor, destColor, t);

            if (Mathf.Abs(t - 0.5f) <= Time.deltaTime && !fadedAmount)
            {
                SwitchIcons();
                fadedAmount = true;
            }
            
            yield return new WaitForEndOfFrame();
        }
    }

    private void SwitchIcons()
    {
        if (TurnManager.Instance.CurrentPlayerType == PlayerType.White)
        {
            _currencyText.text = EconomyManager.Instance.WhiteValue.ToString();
            _powerKingImage.sprite = _whitePowerKingSprite;
            _shieldRookImage.sprite = _whiteShieldRookSprite;
            _barrierBishopImage.sprite = _whiteBarrierBishopSprite;
        }
        else
        {
            _currencyText.text = EconomyManager.Instance.BlackValue.ToString();
            _powerKingImage.sprite = _blackPowerKingSprite;
            _shieldRookImage.sprite = _blackShieldRookSprite;
            _barrierBishopImage.sprite = _blackBarrierBishopSprite;
        }
    }

    public void ChangeCurrencyText(int value)
    {
        _currencyText.text = value.ToString();
    }

    public void TogglePieceButtons(bool toggle)
    {
        if (toggle) 
        {
            foreach (ImageSwitchButton switchButton in _imageSwitchButtons)
            {
                switchButton.SetButtonType();
            }
        }

        _piecesButton.SetActive(!toggle);
        _pieceButtonsHolder.SetActive(toggle);
    }
    
    public void DisplayGameOver(bool whiteWin)
    {
        if (whiteWin)
        {
            _whiteWinPanel.SetActive(true);
        }
        else
        {
            _blackWinPanel.SetActive(true);
        }

        _gameOverOptionsPanel.SetActive(true);
    }
}

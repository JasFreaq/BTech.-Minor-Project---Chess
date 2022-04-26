using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] TaskBarHandler _taskBarHandler;

    [Header("Economy")]
    [SerializeField] private Text _currencyText;
    [SerializeField] private Image _taskBarImage;

    [Header("Game Over")]
    [SerializeField] private GameObject _whiteWinPanel;
    [SerializeField] private GameObject _blackWinPanel;
    [SerializeField] private GameObject _gameOverOptionsPanel;

    [Header("Button Icons")]
    [SerializeField] private TaskbarButton[] _powerButtons = new TaskbarButton[3];
    [SerializeField] Text[] _costTexts = new Text[7];
    [SerializeField] private GameObject _piecesButton;
    [SerializeField] private GameObject _pieceButtonsHolder;
    [SerializeField] private PieceButton[] _pieceButtons = new PieceButton[4];
    [SerializeField] ImageSwitchButton _closeButton;

    #region Singleton Pattern

    private static UIManager _instance;

    public static UIManager Instance => _instance;

    #endregion

    private void Awake()
    {
        _instance = this;
    }

    private void Start()
    {
        UpdateCost();
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
                UpdateCost();
                fadedAmount = true;
            }
            
            yield return new WaitForEndOfFrame();
        }
    }

    private void SwitchIcons()
    {
        _currencyText.text = TurnManager.Instance.CurrentPlayerType == PlayerType.White ?
            EconomyManager.Instance.WhiteValue.ToString() : EconomyManager.Instance.BlackValue.ToString();

        foreach (TaskbarButton powerButton in _powerButtons)
        {
            powerButton.ChangeButtonType();
        }
    }
    
    public void TogglePieceButtonsWrapper(bool toggle)
    {
        TogglePieceButtons(toggle);
    }    

    private void TogglePieceButtons(bool toggle, bool deactivateText = false)
    {
        if (toggle) 
        {
            foreach (PieceButton pieceButton in _pieceButtons)
            {
                pieceButton.ChangeButtonType(deactivateText);
            }

            _closeButton.ChangeButtonType();
        }
                
        _piecesButton.SetActive(!toggle);
        _pieceButtonsHolder.SetActive(toggle);
    }
    
    public IEnumerator ProcessPromotionUIRoutine()
    {
        TogglePieceButtons(true, true);
        _taskBarHandler.OpenTaskbar();

        Coroutine<int> routine = this.StartCoroutine<int>(_taskBarHandler.ProcessPromotionSelectionRoutine());
        yield return routine.coroutine;
        yield return routine.returnVal;
    }

    public void ChangeCurrencyText(int value)
    {
        _currencyText.text = value.ToString();
    }

    public void UpdateCost()
    {
        int[] costs = EconomyManager.Instance.GetCosts(TurnManager.Instance.CurrentPlayerType);
        for (int i = 0; i < costs.Length; i++)
        {
            _costTexts[i].text = costs[i].ToString();
        }
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

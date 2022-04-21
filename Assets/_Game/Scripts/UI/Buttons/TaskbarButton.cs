using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TaskbarButton : ImageSwitchButton
{
    [Header("Cost")]
    [SerializeField] private Text _costText;
    [SerializeField] private Color _whiteColor;
    [SerializeField] private Color _blackColor;

    public void ChangeButtonType(bool deactivateText = false)
    {
        base.ChangeButtonType();

        if (deactivateText)
        {
            _costText.gameObject.SetActive(false);
        }
        else
        {
            _costText.gameObject.SetActive(true);
            _costText.color = TurnManager.Instance.CurrentPlayerType == PlayerType.White ?
                _whiteColor : _blackColor;
        }
    }
}

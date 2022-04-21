using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ImageSwitchButton : MonoBehaviour
{
    [Header("Icon")]
    [SerializeField] private Image _buttonImage;
    [SerializeField] private Sprite _whiteSprite;
    [SerializeField] private Sprite _blackSprite;

    public virtual void ChangeButtonType()
    {
        _buttonImage.sprite = TurnManager.Instance.CurrentPlayerType == PlayerType.White ?
            _whiteSprite : _blackSprite;
    }
}

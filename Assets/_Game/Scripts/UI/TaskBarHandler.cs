using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaskBarHandler : MonoBehaviour
{
    [SerializeField] private Vector2 _openPos;
    [SerializeField] private Vector2 _closePos;
    [SerializeField] private float _lerpSpeed;

    private float _t;
    private bool _lerping, _opening, _closing;
    private Vector2 _srcPos;
    private Vector2 _destPos;

    #region Singleton Pattern

    private static TaskBarHandler _instance;

    public static TaskBarHandler Instance => _instance;

    #endregion

    private void Awake()
    {
        _instance = this;
    }

    private void Update()
    {
        if (_lerping)
        {
            _t += Time.deltaTime;

            transform.localPosition = Vector2.Lerp(_srcPos, _destPos, _t / _lerpSpeed);

            if (Vector2.Distance(transform.localPosition, _destPos) < Mathf.Epsilon)
            {
                transform.localPosition = _destPos;
                _opening = false;
                _closing = false;
                _lerping = false;
            }
        }
    }

    public void OpenTaskbar()
    {
        if (!_opening) 
        {
            if (_lerping)
            {
                _t = 1 - _t / _lerpSpeed;
            }
            else
            {
                _t = 0;
            }

            _srcPos = _closePos;
            _destPos = _openPos;

            _opening = true;
            _closing = false;
            _lerping = true;
        }
    }

    public void CloseTaskbar()
    {
        if (!_closing)
        {
            if (_lerping)
            {
                _t = 1 - _t / _lerpSpeed;
            }
            else
            {
                _t = 0;
            }

            _srcPos = _openPos;
            _destPos = _closePos;

            _opening = false;
            _closing = true;
            _lerping = true;
        }
    }
}

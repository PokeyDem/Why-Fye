using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HotbarSlot : MonoBehaviour
{
    [SerializeField] Button button;
    [SerializeField] TextMeshProUGUI counterTextField;
    [SerializeField] GameObject frame;
    
    private RectTransform _rectTransform;
    private int _counter;

    private void Awake()
    {
        _rectTransform = GetComponent<RectTransform>();
    }

    public void SetScale(Vector3 newScale)
    {
        _rectTransform.localScale = newScale;
    }

    public void SetCounter(int newValue)
    {
        _counter = newValue;
        counterTextField.text = _counter.ToString();
    }

    public void DecreaseCounter()
    {
        if (_counter == 0)
            return;
        
        _counter--;
        counterTextField.text = _counter.ToString();
    }
}

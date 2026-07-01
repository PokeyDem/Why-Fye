using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TextPrinter : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI textField;
    [SerializeField] private float nextCharacterDelay;

    private string _currentTextToPrint;
    private bool _isPrinting;
    private int _counter;
    
    public event Action OnPrintFinished;

    public void Initialize(TextMeshProUGUI textField)
    {
        this.textField = textField;
    }
    
    public void PrintText(string text)
    {
        _currentTextToPrint = text;
        _isPrinting = true;
        _counter = 0;
        textField.text = "";
        StartCoroutine(PrintTextCoroutine());
    }

    private IEnumerator PrintTextCoroutine()
    {
        while (_isPrinting)
        {
            yield return new WaitForSeconds(nextCharacterDelay);
            
            if (textField.text.Length == _currentTextToPrint.Length - 1)
            {
                _isPrinting = false;
                OnPrintFinished?.Invoke();
            }
            
            textField.text += _currentTextToPrint[_counter++];
        }
        textField.text = _currentTextToPrint;
    }

    public void InterruptPrinting()
    {
        _isPrinting = false;
        OnPrintFinished?.Invoke();
    }

    public bool IsPrinting()
    {
        return _isPrinting;
    }

}

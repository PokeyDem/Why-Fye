using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
[CreateAssetMenu(fileName = "Print Text Command", menuName = "Tutorial Steps/Print Text Command")]
public class PrintTextCommand : TutorialCommand
{
    [SerializeField, TextArea(5,10)] private string textToPrint;
    private Action _onComplete;
    private TextPrinter _textPrinterRef;
    
    public override void Execute(TutorialContext tutorialContext, Action onActionComplete)
    {
        _onComplete = onActionComplete;
        _textPrinterRef = tutorialContext.textPrinter;
        
        _textPrinterRef.OnPrintFinished += OnPrintingFinished;
        
        _textPrinterRef.PrintText(textToPrint);
    }

    private void OnPrintingFinished()
    {
        _textPrinterRef.OnPrintFinished -= OnPrintingFinished;
        _onComplete?.Invoke();
    }
}

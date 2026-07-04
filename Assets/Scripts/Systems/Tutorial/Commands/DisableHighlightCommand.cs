using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Disable Highlight Object Command", menuName = "Tutorial Steps/Disable Highlight Object Command")]
public class DisableHighlightCommand : TutorialCommand
{
    private Action _onComplete;
    private ObjectHighlighter _objectHighlighterRef;
    
    public override void Execute(TutorialContext tutorialContext, Action onActionComplete)
    {
        _onComplete = onActionComplete;
        _objectHighlighterRef = tutorialContext.objectHighlighter;
        
        _objectHighlighterRef.OnHighlightComplete += OnHighlightComplete;
        
        _objectHighlighterRef.DisableHighlight();
    }

    private void OnHighlightComplete()
    {
        _objectHighlighterRef.OnHighlightComplete -= OnHighlightComplete;
        _onComplete?.Invoke();
    }
}

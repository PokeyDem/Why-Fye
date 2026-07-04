using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Enable Highlight Object Command", menuName = "Tutorial Steps/Enable Highlight Object Command")]
public class EnableHighlightCommand : TutorialCommand
{
    [SerializeField] private int objectToHighlightIndex;
    private Action _onComplete;
    private ObjectHighlighter _objectHighlighterRef;

    public override void Execute(TutorialContext tutorialContext, Action onActionComplete)
    {
        _onComplete = onActionComplete;
        _objectHighlighterRef = tutorialContext.objectHighlighter;

        _objectHighlighterRef.OnHighlightComplete += OnHighlightComplete;

        _objectHighlighterRef.HighlightObject(objectToHighlightIndex);
    }

    private void OnHighlightComplete()
    {
        _objectHighlighterRef.OnHighlightComplete -= OnHighlightComplete;
        _onComplete?.Invoke();
    }
}
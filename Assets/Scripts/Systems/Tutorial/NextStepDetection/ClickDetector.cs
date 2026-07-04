using System;
using UnityEngine;

public class ClickDetector : MonoBehaviour, INextStepDetector
{
    public event Action OnActionDetectedEvent;

    private NextTutorialStepDetectionMethod _nextTutorialStepDetectionMethod = NextTutorialStepDetectionMethod.OnScreenClick;
    
    
    public NextTutorialStepDetectionMethod GetNextStepDetectionMethod()
    {
        return _nextTutorialStepDetectionMethod;
    }

    private void OnEnable()
    {
        PlayerControls.OnScreenClickDetected += OnActionDetected;
    }

    private void OnDisable()
    {
        PlayerControls.OnScreenClickDetected -= OnActionDetected;
    }

    public void OnActionDetected()
    {
        OnActionDetectedEvent?.Invoke();
    }
}

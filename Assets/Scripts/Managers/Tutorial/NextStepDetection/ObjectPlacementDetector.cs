using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPlacementDetector : MonoBehaviour, INextStepDetector
{
    private NextTutorialStepDetectionMethod _nextTutorialStepDetectionMethod = NextTutorialStepDetectionMethod.OnObjectPlacement;
    
    public event Action OnActionDetectedEvent;
    
    public NextTutorialStepDetectionMethod GetNextStepDetectionMethod()
    {
        return _nextTutorialStepDetectionMethod;
    }

    private void OnEnable()
    {
        ObjectPlacementSystem.OnObjectPlaced += OnActionDetected;
    }
    
    private void OnDisable()
    {
        ObjectPlacementSystem.OnObjectPlaced -= OnActionDetected;
    }

    public void OnActionDetected()
    {
        OnActionDetectedEvent?.Invoke();
    }
}

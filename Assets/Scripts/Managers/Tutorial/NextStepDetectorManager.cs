using System;
using System.Collections.Generic;
using UnityEngine;

public class NextStepDetectorManager : MonoBehaviour
{
    
    private Dictionary<NextTutorialStepDetectionMethod, INextStepDetector> _nextStepDetectors = new Dictionary<NextTutorialStepDetectionMethod, INextStepDetector>();

    public static event Action OnNextStepDetected;

    private void Awake()
    {
        INextStepDetector[] nextStepDetectors = GetComponents<INextStepDetector>();

        foreach (var nextStepDetector in nextStepDetectors)
        {
            _nextStepDetectors.Add(nextStepDetector.GetNextStepDetectionMethod(), nextStepDetector);
            MonoBehaviour monoBehaviourComponent = (MonoBehaviour)nextStepDetector;
            monoBehaviourComponent.enabled = false;
        }
    }

    private void OnNextStep()
    {
        OnNextStepDetected?.Invoke();
    }

    public void SetDetectionMethod(NextTutorialStepDetectionMethod newDetectionMethod)
    {
        DisableAllDetectors();
        _nextStepDetectors[newDetectionMethod].OnActionDetectedEvent += OnNextStep;
        MonoBehaviour nextStepDetector = (MonoBehaviour)_nextStepDetectors[newDetectionMethod];
        nextStepDetector.enabled = true;
    }

    public void DisableAllDetectors()
    {
        foreach (var nextStepDetector in _nextStepDetectors)
        {
            nextStepDetector.Value.OnActionDetectedEvent -= OnNextStep;
            MonoBehaviour monoBehaviourComponent = (MonoBehaviour)nextStepDetector.Value;
            monoBehaviourComponent.enabled = false;
        }
    }
    
    
}

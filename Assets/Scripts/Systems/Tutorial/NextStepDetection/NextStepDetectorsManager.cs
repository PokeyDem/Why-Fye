using System;
using System.Collections.Generic;
using UnityEngine;

public class NextStepDetectorsManager : MonoBehaviour
{
    private Dictionary<NextTutorialStepDetectionMethod, INextStepDetector> _nextStepDetectors = new Dictionary<NextTutorialStepDetectionMethod, INextStepDetector>();
    private bool _detectionEnabled;
    public event Action OnActionDetected;

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
        if (!_detectionEnabled)
            return;
        
        OnActionDetected?.Invoke();
        Debug.Log("Next step detected");
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

    public void EnableDetection()
    {
        _detectionEnabled = true;
        Debug.Log("Detection enabled");
    }

    public void DisableDetection()
    {
        _detectionEnabled = false;
        Debug.Log("Detection disabled");
    }
}

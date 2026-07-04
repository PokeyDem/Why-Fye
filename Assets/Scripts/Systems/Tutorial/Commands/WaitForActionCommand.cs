using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
[CreateAssetMenu(fileName = "Wait For Action Command", menuName = "Tutorial Steps/Wait For Action Command")]
public class WaitForActionCommand : TutorialCommand
{
    [SerializeField] private NextTutorialStepDetectionMethod nextStepDetectionMethod;
    private Action _onComplete;
    private NextStepDetectorsManager _nextStepDetectorsManagerRef;
    
    public override void Execute(TutorialContext tutorialContext, Action onActionComplete)
    {
        _onComplete = onActionComplete;
        _nextStepDetectorsManagerRef = tutorialContext.nextStepDetectorsManager;
        
        _nextStepDetectorsManagerRef.SetDetectionMethod(nextStepDetectionMethod);
        _nextStepDetectorsManagerRef.OnActionDetected += OnActionComplete;
        
        _nextStepDetectorsManagerRef.EnableDetection();
    }

    private void OnActionComplete()
    {
        _nextStepDetectorsManagerRef.OnActionDetected -= OnActionComplete;
        
        _nextStepDetectorsManagerRef.DisableDetection();
        _nextStepDetectorsManagerRef.DisableAllDetectors();
        
        _onComplete?.Invoke();
    }
}

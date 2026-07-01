using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
[CreateAssetMenu(fileName = "Change Camera Pivot Command", menuName = "Tutorial Steps/Change Camera Pivot Command")]
public class ChangeCameraPivotCommand : TutorialCommand
{
    [SerializeField] private Vector3 newPivotPos;
    private Action _onComplete;
    private CameraPivotControl _cameraPivotControlRef;
    
    public override void Execute(TutorialContext tutorialContext, Action onActionComplete)
    {
        _onComplete = onActionComplete;
        _cameraPivotControlRef = tutorialContext.cameraPivotControl;

        _cameraPivotControlRef.OnCameraPivotChanged += OnChangingFinished;
        
        _cameraPivotControlRef.ChangePivotPositionTransform(newPivotPos);
    }

    private void OnChangingFinished()
    {
        _cameraPivotControlRef.OnCameraPivotChanged -= OnChangingFinished;
        _onComplete?.Invoke();
    }
}

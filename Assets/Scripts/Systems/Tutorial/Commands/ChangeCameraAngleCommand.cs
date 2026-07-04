using System;
using UnityEngine;

[CreateAssetMenu(fileName = "Change Camera Angle Command", menuName = "Tutorial Steps/Change Camera Angle Command")]
public class ChangeCameraAngleCommand : TutorialCommand
{
    [SerializeField] private Quaternion newAngle;
    private Action _onComplete;
    private CameraOrbit _cameraOrbitRef;
    public override void Execute(TutorialContext tutorialContext, Action onActionComplete)
    {
        _onComplete = onActionComplete;
        _cameraOrbitRef = tutorialContext.cameraOrbit;

        _cameraOrbitRef.OnRotationFinished += OnChangeComplete;
        _cameraOrbitRef.SetCameraRotation(newAngle);
    }

    private void OnChangeComplete()
    {
        _cameraOrbitRef.OnRotationFinished -= OnChangeComplete;
        
        _onComplete?.Invoke();
    }
}

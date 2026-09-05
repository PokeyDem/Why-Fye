using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
[CreateAssetMenu(fileName = "Update Device Amount Command", menuName = "Tutorial Steps/Update Device Amount Command")]
public class UpdateDeviceAmountCommand : TutorialCommand
{
    [SerializeField] private List<int> _newAmount;
    private DeviceAmountActualizer _deviceAmountActualizerRef;
    
    public override void Execute(TutorialContext tutorialContext, Action onActionComplete)
    {
        Debug.Log("Updating device amount UpdateCommand.Execute()");
        _deviceAmountActualizerRef = tutorialContext.deviceAmountActualizer;
        _deviceAmountActualizerRef.UpdateDeviceAmount(_newAmount);
        onActionComplete?.Invoke();
    }

    // private void OnValidate()
    // {
    //     if (_newAmount.Count > 3)
    //     {
    //         int rangeToRemove = _newAmount.Count - 3;
    //         _newAmount.RemoveRange(3, rangeToRemove);
    //     }
    // }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeviceAmountActualizer : MonoBehaviour
{
    public event Action<List<int>> OnDeviceAmountUpdate;

    public void UpdateDeviceAmount(List<int> newAmount)
    {
        Debug.Log("Actualizer calling OnDeviceAmountUpdate");
        OnDeviceAmountUpdate?.Invoke(newAmount);
    }
}

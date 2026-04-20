using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LevelData", menuName = "Level Data/Level Data")]
public class LevelData : ScriptableObject
{
    public List<DeviceOnLevel> devicesData =  new List<DeviceOnLevel>();
}

[Serializable]
public struct DeviceOnLevel
{
    public DeviceType deviceType;
    public int deviceAmount;
}
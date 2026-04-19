using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Placeable Item", menuName = "Placement System/Placeable Item")]
public class PlaceableDeviceData : ScriptableObject
{
    [Header("Prefabs")] 
    public GameObject devicePrefab;
    public GameObject previewPrefab;
    
    public DeviceType deviceType;

    [Header("Placement Rules")] 
    public bool onlyOnWalls;
}

public enum DeviceType
{
    Router,
    Extender
}

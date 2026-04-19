using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ItemCatalog", menuName = "Placement System/Item Catalog")]
public class DeviceCatalog : ScriptableObject
{
    public List<PlaceableDeviceData> allAvailableDevices = new List<PlaceableDeviceData>();
}

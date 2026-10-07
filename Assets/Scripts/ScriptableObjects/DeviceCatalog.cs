using System.Collections.Generic;
using UnityEngine;

namespace ScriptableObjects
{
    [CreateAssetMenu(fileName = "ItemCatalog", menuName = "Placement System/Item Catalog")]
    public class DeviceCatalog : ScriptableObject
    {
        public List<PlaceableDeviceData> allAvailableDevices = new List<PlaceableDeviceData>();
    }
}

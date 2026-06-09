using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlacedDeviceData
{
    public GameObject deviceObject;
    public bool isReceiving;
    
    public bool isSending => sendingTo.Count > 0;
    
    public PlacedDeviceData receivingFrom;
    
    public List<PlacedDeviceData> sendingTo;
    public int maxOutgoingConnections;
    
    public DeviceType deviceType;

    public PlacedDeviceData(GameObject obj, DeviceType type)
    {
        deviceObject = obj;
        deviceType = type;
        isReceiving = false;
        receivingFrom = null;
        sendingTo = new  List<PlacedDeviceData>();

        if (deviceType == DeviceType.Splitter)
        {
            maxOutgoingConnections = 2;
        }else if (deviceType == DeviceType.Receiver)
        {
            maxOutgoingConnections = 0;
        }
        else
        {
            maxOutgoingConnections = 1;
        }
    }
}

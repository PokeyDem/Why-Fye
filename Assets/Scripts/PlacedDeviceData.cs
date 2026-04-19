using UnityEngine;

[System.Serializable]
public class PlacedDeviceData
{
    public GameObject deviceObject;
    public bool isReceiving;
    public bool isSending;
    public bool isRouter;
    public PlacedDeviceData receivingFrom;
    public PlacedDeviceData sendingTo;
    public DeviceType deviceType;

    public PlacedDeviceData(GameObject obj)
    {
        deviceObject = obj;
        isReceiving = false;
        isSending = false;
        isRouter = false;
        receivingFrom = null;
        sendingTo = null;
    }
}

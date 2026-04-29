using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReceiverController : MonoBehaviour
{
    [SerializeField] private PhoneScreenVisualsController phoneScreenVisualsController;
    [SerializeField] private NPCBehaviour npcBehaviour;

    public void DeviceConnected()
    {
        phoneScreenVisualsController.ConnectDevice();
        npcBehaviour.ConnectDevice();
    }

    public void DeviceDisconnected()
    {
        phoneScreenVisualsController.DisconnectDevice();
        npcBehaviour.DisconnectDevice();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReceiverController : MonoBehaviour
{
    [SerializeField] private ScreenVisualsController screenVisualsController;
    [SerializeField] private NPCBehaviour npcBehaviour;

    public void DeviceConnected()
    {
        screenVisualsController.ConnectDevice();
        npcBehaviour.ConnectDevice();
    }

    public void DeviceDisconnected()
    {
        screenVisualsController.DisconnectDevice();
        npcBehaviour.DisconnectDevice();
    }
}

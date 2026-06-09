using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReceiverController : MonoBehaviour
{
    [SerializeField] private NPCBehaviour npcBehaviour;
    private PhoneScreenVisualsController _phoneScreenVisualsController;

    private void Start()
    {
        _phoneScreenVisualsController = GetComponent<PhoneScreenVisualsController>();
    }

    public void DeviceConnected()
    {
        _phoneScreenVisualsController.ConnectDevice();
        npcBehaviour.ConnectDevice();
    }

    public void DeviceDisconnected()
    {
        _phoneScreenVisualsController.DisconnectDevice();
        npcBehaviour.DisconnectDevice();
    }
}

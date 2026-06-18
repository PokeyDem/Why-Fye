using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class ConnectionsManager : MonoBehaviour
{
    [SerializeField] private List<PlacedDeviceData> allPlacedDevices = new List<PlacedDeviceData>();
    [SerializeField] private float sensorHeightOffset;
    [SerializeField] private string receiverTagName;
    [SerializeField] private LayerMask connectionMask;

    public static Action OnCompletion;
    public static Action OnCompletionRevoke;
    public void LinkNewDevice(GameObject newDevice, DeviceType deviceType, PlacedDeviceData newDeviceData = null)
    {
        if (newDeviceData == null)
        {
            newDeviceData = new PlacedDeviceData(newDevice, deviceType);
            newDeviceData.deviceType = deviceType;

            if (deviceType == DeviceType.Router)
            {
                newDeviceData.isReceiving = true;
            }
        
            allPlacedDevices.Add(newDeviceData);
        }

        List<PlacedDeviceData> validConnections = FindValidConnections(newDevice.transform);

        validConnections = validConnections.OrderBy(d => Vector3.Distance(newDevice.transform.position, d.deviceObject.transform.position)).ToList();

        PlacedDeviceData closestSender = validConnections.FirstOrDefault(d => 
            d.isReceiving && d.sendingTo.Count < d.maxOutgoingConnections);
        
        if (closestSender != null)
        {
            AssignConnection(closestSender, newDeviceData);
        }
    
        if (newDeviceData.isReceiving || newDeviceData.deviceType == DeviceType.Router) 
        {
         
            PlacedDeviceData closestReceiver = validConnections.FirstOrDefault(d => !d.isReceiving);
            if (closestReceiver != null && !newDeviceData.isSending) 
            {
                AssignConnection(newDeviceData, closestReceiver);
            }
            
            PropagateSignal(newDeviceData);
        }
    }

    public void FindNewReceivers()
    {
        ClearReceivers();

        foreach (GameObject receiver in ReceiverComponent.ActiveReceivers)
        {
            allPlacedDevices.Add(new PlacedDeviceData(receiver, DeviceType.Receiver));
        }
    }

    private void ClearReceivers()
    {
        foreach (var placedDeviceData in allPlacedDevices.ToList())
        {
            if (placedDeviceData.deviceType == DeviceType.Receiver)
                allPlacedDevices.Remove(placedDeviceData);
        }
    }

    private void PropagateSignal(PlacedDeviceData newDeviceData)
    {
        foreach (var receiver in newDeviceData.sendingTo.ToList())
        {
            PropagateSignal(receiver);
        }
        
        int availableSlots = newDeviceData.maxOutgoingConnections - newDeviceData.sendingTo.Count;
        if (availableSlots <= 0) return;

        while (availableSlots > 0)
        {
            List<PlacedDeviceData> validConnections = FindValidConnections(newDeviceData.deviceObject.transform);
            validConnections = validConnections.OrderBy(d => 
                Vector3.Distance(newDeviceData.deviceObject.transform.position, d.deviceObject.transform.position)).ToList();
        
            PlacedDeviceData nextReceiver = validConnections.FirstOrDefault(d => !d.isReceiving);

            if (nextReceiver != null)
            {
                AssignConnection(newDeviceData, nextReceiver);
                PropagateSignal(nextReceiver);
            }
            availableSlots--;
        }
        
    }
    private List<PlacedDeviceData> FindValidConnections(Transform originDevice)
    {
        List<PlacedDeviceData> validConnections = new  List<PlacedDeviceData>();

        Vector3 originPoint = originDevice.position + (originDevice.up * sensorHeightOffset);
        foreach (var placedDeviceData in allPlacedDevices)
        {
            Transform targetTransform = placedDeviceData.deviceObject.transform;

            if (targetTransform == originDevice) continue;
            
            Vector3 targetPoint = targetTransform.position + (targetTransform.up * sensorHeightOffset);

            Vector3 directionToTarget = targetPoint - originPoint;
            float distanceToTarget = directionToTarget.magnitude;

            RaycastHit[] hits = Physics.RaycastAll(originPoint, directionToTarget, distanceToTarget);
            
            Debug.DrawRay(originDevice.position, directionToTarget.normalized * distanceToTarget, Color.red, 200f);

            bool isViewBlocked = false;

            foreach (RaycastHit hit in hits)
            {
                if (hit.transform.gameObject.CompareTag("Obstacle"))
                {
                    isViewBlocked = true;
                    break;
                }
            }
            
            if (!isViewBlocked)
                validConnections.Add(placedDeviceData);
        }
        
        return validConnections;
    }
    
    private void AssignConnection(PlacedDeviceData sender, PlacedDeviceData receiver)
    {
        if (sender.sendingTo.Count >= sender.maxOutgoingConnections)
        {
            var oldestConnection = sender.sendingTo[0];
            oldestConnection.receivingFrom = null;
            oldestConnection.isReceiving = false;
            sender.sendingTo.RemoveAt(0);
        }
        
        if (receiver.receivingFrom != null) 
        {
            receiver.receivingFrom.sendingTo.Remove(receiver);
        }
        
        sender.sendingTo.Add(receiver);
        receiver.isReceiving = true;
        receiver.receivingFrom = sender;

        if (receiver.deviceType == DeviceType.Receiver)
        {
            receiver.deviceObject.GetComponent<ReceiverController>().DeviceConnected();
        }
        
        ConnectionStream[] streams = sender.deviceObject.GetComponentsInChildren<ConnectionStream>();
     
        foreach (var stream in streams)
        {
            if (!stream.IsConnected() || sender.deviceType == DeviceType.Router)
            {
                stream.ConnectToReceiver(receiver.deviceObject.transform);
                break;
            }
        }
        
        AreAllReceiversConnected();
    }

    private void AreAllReceiversConnected()
    {
        Debug.Log("Level completion check started");
        foreach (var connection in allPlacedDevices)
        {
            if (connection.deviceType == DeviceType.Receiver && !connection.isReceiving)
            {
                OnCompletionRevoke?.Invoke();
                Debug.Log("Level completion check failed");
                return;
            }
        }
        OnCompletion?.Invoke();
        Debug.Log("Level completion check succeeded");
    }

    public void ResetDevices()
    {
        foreach (var placedDeviceData in allPlacedDevices.ToList())
        {
            if (placedDeviceData.deviceType == DeviceType.Receiver)
            {
                placedDeviceData.isReceiving = false;
                placedDeviceData.receivingFrom = null;
                placedDeviceData.deviceObject.GetComponentInChildren<ReceiverController>().DeviceDisconnected();
            }
            else
            {
                LineRenderer[] renderers = placedDeviceData.deviceObject.GetComponentsInChildren<LineRenderer>();
                foreach (var lr in renderers)
                {
                    lr.enabled = false;
                }
                
                Destroy(placedDeviceData.deviceObject);
                allPlacedDevices.Remove(placedDeviceData);
            }
        }
    }

    public void RemoveDevice(GameObject device)
    {
        foreach (var placedDeviceData in allPlacedDevices.ToList())
        {
            if (placedDeviceData.deviceObject != device)
                continue;

            if (placedDeviceData.isReceiving && placedDeviceData.deviceType != DeviceType.Router && placedDeviceData.receivingFrom.sendingTo.Count > 0)
            {
                placedDeviceData.deviceObject.GetComponentInChildren<ConnectionStream>().CloseConnection();
                placedDeviceData.receivingFrom.sendingTo.Remove(placedDeviceData);
            }
            
            ConnectionStream[] streams = placedDeviceData.deviceObject.GetComponentsInChildren<ConnectionStream>();
            foreach (var stream in streams)
            {
                stream.CloseConnection();
            }

            foreach (var receiver in placedDeviceData.sendingTo.ToList())
            {
                DisconnectDevice(receiver);
            }
            
            allPlacedDevices.Remove(placedDeviceData);
        }
    }

    public void DisconnectDevice(PlacedDeviceData deviceData)
    {
        deviceData.isReceiving = false;
        
        ConnectionStream[] streams = deviceData.deviceObject.GetComponentsInChildren<ConnectionStream>();
        foreach (var stream in streams)
        {
            stream.CloseConnection();
        }
        
        LineRenderer[] renderers = deviceData.deviceObject.GetComponentsInChildren<LineRenderer>();
        foreach (var lr in renderers)
        {
            Vector3[] pos = { lr.GetPosition(0) };
            lr.SetPositions(pos);
        }

        if (deviceData.deviceType == DeviceType.Receiver)
        {
            deviceData.isReceiving = false;
            deviceData.deviceObject.GetComponent<ReceiverController>().DeviceDisconnected();
            AreAllReceiversConnected();
            return;
        }
       
        foreach (var connection in deviceData.sendingTo)
        {
            DisconnectDevice(connection);
        }

        if (deviceData.sendingTo.Count == 0)
        {
            foreach (var connection in allPlacedDevices)
            {
                if (connection.deviceType == DeviceType.Router && !connection.isSending)
                {
                    LinkNewDevice(connection.deviceObject, connection.deviceType, connection);
                    break;
                }
            }
        }
        
        deviceData.sendingTo.Clear();
    }
    
    public DeviceType GetDeviceType(GameObject device)
    {
        foreach (var placedDeviceData in allPlacedDevices.ToList())
        {
            if (placedDeviceData.deviceObject == device)
            {
                return placedDeviceData.deviceType;
            }
        }
        
        return DeviceType.Router;
    } 
}

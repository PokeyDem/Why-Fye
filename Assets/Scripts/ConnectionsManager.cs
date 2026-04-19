using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class ConnectionsManager : MonoBehaviour
{
    [SerializeField] private List<PlacedDeviceData> allPlacedDevices = new List<PlacedDeviceData>();
    [SerializeField] private float sensorHeightOffset;

    public void LinkNewDevice(GameObject newDevice, DeviceType deviceType)
    {
        PlacedDeviceData newDeviceData = new PlacedDeviceData(newDevice);
        newDeviceData.deviceType = deviceType;

        if (deviceType == DeviceType.Router)
        {
            newDeviceData.isRouter = true;
            newDeviceData.isReceiving = true;
        }
        
        allPlacedDevices.Add(newDeviceData);

        List<PlacedDeviceData> validConnections = FindValidConnections(newDevice.transform);

        validConnections = validConnections.OrderBy(d => Vector3.Distance(newDevice.transform.position, d.deviceObject.transform.position)).ToList();

        PlacedDeviceData closestSender = validConnections.FirstOrDefault(d => d.isReceiving && !d.isSending);
        if (closestSender != null)
        {
            AssignConnection(closestSender, newDeviceData);
        }
    
        if (newDeviceData.isReceiving || newDeviceData.isRouter) 
        {
         
            PlacedDeviceData closestReceiver = validConnections.FirstOrDefault(d => !d.isReceiving);
            if (closestReceiver != null && !newDeviceData.isSending) 
            {
                AssignConnection(newDeviceData, closestReceiver);
            }
            
            PropagateSignal(newDeviceData);
        }
    }

    private void PropagateSignal(PlacedDeviceData newDeviceData)
    {
        if (newDeviceData.isSending && newDeviceData.sendingTo != null)
        {
            PropagateSignal(newDeviceData.sendingTo);
        }
        
        List<PlacedDeviceData> validConnections = FindValidConnections(newDeviceData.deviceObject.transform);
        validConnections = validConnections.OrderBy(d => 
            Vector3.Distance(newDeviceData.deviceObject.transform.position, d.deviceObject.transform.position)).ToList();
        
        PlacedDeviceData nextReceiver = validConnections.FirstOrDefault(d => !d.isReceiving);

        if (nextReceiver != null)
        {
            AssignConnection(newDeviceData, nextReceiver);
            PropagateSignal(nextReceiver);
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
                Debug.Log("Collider hit: " +  hit.collider.gameObject.name);
                if (hit.collider.transform != targetTransform && hit.collider.transform != originDevice)
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
        if (sender.sendingTo != null) sender.sendingTo.receivingFrom = null;
        if (receiver.receivingFrom != null) receiver.receivingFrom.sendingTo = null;

        sender.isSending = true;
        sender.sendingTo = receiver;

        receiver.isReceiving = true;
        receiver.receivingFrom = sender;
        
        sender.deviceObject.GetComponentInChildren<ConnectionStream>().ConnectToReceiver(receiver.deviceObject.transform);
        Debug.Log("Connection assigned");
    }

    private bool AreAllReceiversConnected()
    {
        foreach (var connection in allPlacedDevices)
        {
            if (connection.deviceType == DeviceType.Receiver && !connection.isReceiving)
            {
                return false;
            }
        }

        return true;
    }

    private float IsDeviceVisible(GameObject device, GameObject target)
    {
        Ray ray = new Ray(device.transform.position, target.transform.position - device.transform.position);
        RaycastHit hit;
        
        if (Physics.Raycast(ray, out hit))
        {
            if (hit.collider.gameObject == target)
                return hit.distance;
            else
                return -1f;
        }
        
        return -1f;
    }
}

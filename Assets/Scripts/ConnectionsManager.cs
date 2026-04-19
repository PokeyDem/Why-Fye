using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class ConnectionsManager : MonoBehaviour
{
    [SerializeField] private List<PlacedDeviceData> allPlacedDevices = new List<PlacedDeviceData>();

    public void LinkNewDevice(GameObject newDevice, DeviceType deviceType)
    {
        PlacedDeviceData newDeviceData = new PlacedDeviceData(newDevice);

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

        foreach (var placedDeviceData in allPlacedDevices)
        {
            Transform targetTransform = placedDeviceData.deviceObject.transform;

            if (targetTransform == originDevice) continue;

            Vector3 directionToTarget = targetTransform.position - originDevice.position;
            float distanceToTarget = directionToTarget.magnitude;

            RaycastHit[] hits = Physics.RaycastAll(originDevice.position, directionToTarget, distanceToTarget);
            Debug.DrawRay(originDevice.position, directionToTarget.normalized * distanceToTarget, Color.red, 2f);

            bool isViewBlocked = false;

            foreach (RaycastHit hit in hits)
            {
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
        
        sender.deviceObject.GetComponent<ConnectionStream>().ConnectToReceiver(receiver.deviceObject.transform);
    }

    private void AddFinalReceiver(GameObject receiver)
    {
        
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

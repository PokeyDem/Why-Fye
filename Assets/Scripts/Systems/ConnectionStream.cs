using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConnectionStream : MonoBehaviour
{
    [SerializeField] private float speed = 2f;
    [SerializeField] private float tileResolution = 1f;
    [SerializeField] private Transform receiverTarget;
    [SerializeField, TextArea] private string debugLogField;
 
    private LineRenderer _lineRenderer;
    private Transform _mountPoint;
    private Transform _targetMountPoint;
    private Material _lineMaterial;
    private float _currentOffset;

    private void Awake()
    {
        _lineRenderer = GetComponent<LineRenderer>();
        _lineMaterial = _lineRenderer.material;
        _lineRenderer.positionCount = 2;
        _mountPoint = gameObject.transform.GetChild(0).transform;
    }

    private void Update()
    {

        if (receiverTarget == null || _targetMountPoint == null)
        {
            _lineRenderer.enabled = false;
            return;
        }
        
        debugLogField = receiverTarget.name;

        if (_lineRenderer.enabled)
        {
            UpdateLinePositions();
            AnimateStripes();
        }
        // if (receiverTarget != null)
        // {
        //     _lineRenderer.enabled = true;
        //     UpdateLinePositions();
        //     AnimateStripes();
        // }
        // else
        // {
        //     _lineRenderer.enabled = false;
        // }
    }

    private void UpdateLinePositions()
    {
        
        _lineRenderer.SetPosition(0, _mountPoint.position);
        _lineRenderer.SetPosition(1, _targetMountPoint.position);
        
        float distance = Vector3.Distance(transform.position, receiverTarget.position);
        
        // _lineMaterial.mainTextureScale = new Vector2((maxTilingDistance - distance) * tileResolution, 1f);
        
    }

    private void AnimateStripes()
    {
        _currentOffset -= Time.deltaTime * speed;
        _lineMaterial.mainTextureOffset = new Vector2(_currentOffset, 0f);
    }

    public void ConnectToReceiver(Transform receiver)
    {
        Debug.Log("Called connect to receiver: " + receiver.gameObject.name);
        _lineRenderer.enabled = true;
        receiverTarget = receiver;
        _targetMountPoint = receiver.gameObject.transform.GetChild(0).transform;
    }

    public void CloseConnection()
    {
        receiverTarget = null;
        _lineRenderer.enabled = false;
        
    }

    public bool IsConnected()
    {
        Debug.Log("IsConnected receiver: " + receiverTarget);
        return receiverTarget !=null;
    }
}

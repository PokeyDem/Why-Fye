using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraOrbit : MonoBehaviour
{
    [SerializeField] private float rotationSpeed = 5f;
    [SerializeField] private float minVerticalAngle = -20f;
    [SerializeField] private float maxVerticalAngle = 40f;

    private float _currentYaw;
    private float _currentPitch;

    private void Start()
    {
        Vector3 angles = transform.eulerAngles;
        _currentPitch = angles.x;
        _currentYaw = angles.y;
    }

    private void Update()
    {
        if (Input.GetMouseButton(2))
        {
            RotateCamera();
        }
    }
    
    private void RotateCamera()
    {
        float mouseX = Input.GetAxis("Mouse X") * rotationSpeed;
        float mouseY = Input.GetAxis("Mouse Y") * rotationSpeed;
        
        _currentYaw += mouseX;
     
        _currentPitch -= mouseY; 
        
        _currentPitch = Mathf.Clamp(_currentPitch, minVerticalAngle, maxVerticalAngle);
        
        transform.rotation = Quaternion.Euler(0f, _currentYaw, _currentPitch);
    }
}
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraOrbit : MonoBehaviour
{
    [SerializeField] private float rotationSpeed = 5f;
    [SerializeField] private float minVerticalAngle = -20f;
    [SerializeField] private float maxVerticalAngle = 40f;
    [SerializeField] private PlayerControls playerControls;

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
        if (playerControls.IsOrbiting)
        {
            RotateCameraNew();
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

    private void RotateCameraNew()
    {
        float mouseX = playerControls.LookDelta.x * rotationSpeed * Time.deltaTime;
        float mouseY = playerControls.LookDelta.y * rotationSpeed * Time.deltaTime;
        
        _currentYaw += mouseX;
        
        _currentPitch -= mouseY; 
        
        _currentPitch = Mathf.Clamp(_currentPitch, minVerticalAngle, maxVerticalAngle);
        
        transform.rotation = Quaternion.Euler(0f, _currentYaw, _currentPitch);
    }
}
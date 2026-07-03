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
    [SerializeField] private float autoRotationSpeed;

    private float _currentYaw;
    private float _currentPitch;
    private bool _autoRotation;
    private Quaternion _autoRotationTarget;

    public event Action OnRotationFinished;

    private void Start()
    {
        Vector3 angles = transform.eulerAngles;
        _currentPitch = angles.x;
        _currentYaw = angles.y;
    }

    private void Update()
    {
        if (playerControls.IsOrbiting && !_autoRotation)
        {
            RotateCamera();
        }
        
        if (_autoRotation)
            AutoRotateCamera();
            
    }

    private void RotateCamera()
    {
        float mouseX = playerControls.LookDelta.x * rotationSpeed * Time.deltaTime;
        float mouseY = playerControls.LookDelta.y * rotationSpeed * Time.deltaTime;
        
        _currentYaw += mouseX;
        
        _currentPitch -= mouseY; 
        
        _currentPitch = Mathf.Clamp(_currentPitch, minVerticalAngle, maxVerticalAngle);
        
        transform.rotation = Quaternion.Euler(0f, _currentYaw, _currentPitch);
    }

    public void SetCameraRotation(Quaternion rotation)
    {
        _autoRotationTarget = rotation;
        EnableAutoRotation();
    }
    
    private void AutoRotateCamera()
    {
        Quaternion rotation;
        rotation = Quaternion.Slerp(transform.rotation, _autoRotationTarget, Time.deltaTime * autoRotationSpeed);
        transform.rotation = rotation;

        if (Quaternion.Angle(transform.rotation, _autoRotationTarget) < 0.01f)
        {
            transform.rotation = _autoRotationTarget;
            _currentYaw = _autoRotationTarget.eulerAngles.y;
            _currentPitch = _autoRotationTarget.eulerAngles.z;
            DisableAutoRotation();
        }
    }

    private void EnableAutoRotation()
    {
        _autoRotation = true;
    }

    private void DisableAutoRotation()
    {
        _autoRotation = false;
    }
}
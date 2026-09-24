using System;
using UnityEngine;

/// <summary>
/// Allows the camera to be rotated around the scene (pivot point)
/// Also controls the camera's vertical angle
/// </summary>
public class CameraOrbit : MonoBehaviour
{
    
    [Header("Dependencies")]
    [SerializeField] private PlayerControls playerControls;
    
    [Header("Camera Rotation Settings")]
    [SerializeField] private float rotationSpeed = 5f;
    [SerializeField] private float minVerticalAngle = -20f;
    [SerializeField] private float maxVerticalAngle = 40f;
    [SerializeField] private float autoRotationSpeed;
    
    [SerializeField, Tooltip("Slerp interpolation value threshold before assigning the actual target value")] 
    private float snapThreshold;

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

    private void LateUpdate()
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
        
        // Pitch (x) and Yaw (y) axes are switched due to the camera parent x,y axis orientation being inverted
        transform.rotation = Quaternion.Euler(0f, _currentYaw, _currentPitch);
    }

    
    /// <summary>
    /// Sets the rotation target for a camera
    /// </summary>
    /// <param name="rotation">Rotation target represented by Quaternion</param>
    public void SetCameraRotation(Quaternion rotation)
    {
        _autoRotationTarget = rotation;
        EnableAutoRotation();
    }
    
    private void AutoRotateCamera()
    {
        Quaternion rotation = Quaternion.Slerp(transform.rotation, _autoRotationTarget, Time.deltaTime * autoRotationSpeed);
        transform.rotation = rotation;

        if (Quaternion.Angle(transform.rotation, _autoRotationTarget) < snapThreshold)
        {
            transform.rotation = _autoRotationTarget;
            _currentYaw = _autoRotationTarget.eulerAngles.y;
            _currentPitch = _autoRotationTarget.eulerAngles.z;
            OnRotationFinished?.Invoke();
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
using System;
using UnityEngine;
using UnityEngine.InputSystem.EnhancedTouch;
using Touch = UnityEngine.InputSystem.EnhancedTouch.Touch;

public class CameraZoomManager : MonoBehaviour
{
    [SerializeField] private float zoomSpeed = 0.01f;
    [SerializeField] private float minZoom = 1.15f;
    [SerializeField] private float maxZoom = 2.4f;

    private Camera _camera;

    private void OnEnable()
    {
        EnhancedTouchSupport.Enable();
    }

    private void OnDisable()
    {
        EnhancedTouchSupport.Disable();
    }

    private void Start()
    {
        _camera = Camera.main;
    }

    private void Update()
    {
        if (Touch.activeTouches.Count != 2) return;
        
        Touch touchZero = Touch.activeTouches[0];
        Touch touchOne = Touch.activeTouches[1];

        Vector2 touchZeroPrevPos = touchZero.screenPosition - touchZero.delta;
        Vector2 touchOnePrevPos = touchOne.screenPosition - touchOne.delta;
        
        float prevMagnitude = (touchZeroPrevPos - touchOnePrevPos).magnitude;
        float curMagnitude = (touchZero.screenPosition - touchOne.screenPosition).magnitude;
        
        float difference = curMagnitude - prevMagnitude;

        ZoomCamera(difference * zoomSpeed);
    }

    private void ZoomCamera(float increment)
    {
        _camera.orthographicSize = Mathf.Clamp(_camera.orthographicSize - increment, minZoom, maxZoom);
    }
}

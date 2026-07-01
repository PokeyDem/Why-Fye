using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class CameraPivotControl : MonoBehaviour
{
    [SerializeField] private PlayerControls playerControls;
    [SerializeField] private float transitionSpeed;
    private Camera _camera;
    private Vector3 _newPos;
    private bool _isTransitioning;
    private bool _isTransitioningToTransform;
    
    public event Action OnCameraPivotChanged;

    private void Start()
    {
        _camera = Camera.main;
    }

    private void OnEnable()
    {
        playerControls.OnCameraPivotChanged += ChangePivotPositionRayCast;
    }

    private void OnDisable()
    {
        playerControls.OnCameraPivotChanged -= ChangePivotPositionRayCast;
    }

    private void Update()
    {
        if (_isTransitioning)
        {
            transform.position = Vector3.Lerp(transform.position, _newPos, transitionSpeed * Time.deltaTime);
        }

        if (Vector3.SqrMagnitude(_newPos - transform.position) < 0.0001f)
        {
            transform.position = _newPos;
            _isTransitioning = false;
            
            if (_isTransitioningToTransform)
            {
                _isTransitioningToTransform = false;
                OnCameraPivotChanged?.Invoke();
            }
        }
    }

    private void ChangePivotPositionRayCast()
    {
        if (PointerOverUIDetector.Instance.IsPointerOverUI()) 
            return;
        
        Ray ray = _camera.ScreenPointToRay(playerControls.OnScreenPosition);
            
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, math.INFINITY))
        {
            _newPos = hit.point;
            _isTransitioning = true;
        }
    }

    public void ChangePivotPositionTransform(Vector3 newPivot)
    {
        _newPos = newPivot;
        _isTransitioning = true;
        _isTransitioningToTransform = true;
    }
}

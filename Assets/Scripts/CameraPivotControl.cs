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

    private void Start()
    {
        _camera = Camera.main;
    }

    private void OnEnable()
    {
        playerControls.OnCameraPivotChanged += ChangePivotPosition;
    }

    private void OnDisable()
    {
        playerControls.OnCameraPivotChanged -= ChangePivotPosition;
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
        }
    }

    private void ChangePivotPosition()
    {
        Ray ray = _camera.ScreenPointToRay(playerControls.OnScreenPosition);
            
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, math.INFINITY))
        {
            _newPos = hit.point;
            _isTransitioning = true;
        }
    }
}

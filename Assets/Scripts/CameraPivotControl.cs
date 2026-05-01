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
    private Vector3 _startPosition;
    private Vector3 _newPos;
    private float _t;
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
            transform.position = Vector3.Lerp(_startPosition, _newPos, _t);
            _t += transitionSpeed * Time.deltaTime;
        }

        if (Mathf.Approximately(transform.position.x, _newPos.x) && 
            Mathf.Approximately(transform.position.y, _newPos.y) &&
            Mathf.Approximately(transform.position.z, _newPos.z))
        {
            _isTransitioning = false;
            _t = 0;
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
            _startPosition = transform.position;
        }
    }
}

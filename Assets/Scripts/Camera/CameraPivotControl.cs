using System;
using Unity.Mathematics;
using UnityEngine;

/// <summary>
/// Allow the player to change the camera pivot point position by double-clicking on the screen, using raycast to calculate a new position,
/// also allows an external system to change the pivot by sending the new position point directly.
/// </summary>
public class CameraPivotControl : MonoBehaviour
{
    [Header("Dependencies")]
    [SerializeField] private PlayerControls playerControls;
    
    [Header("Transition Settings")]
    [SerializeField] private float transitionSpeed;
    [SerializeField] private float snapThreshold = 0.0001f;
    
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

    private void LateUpdate()
    {
        if (_isTransitioning)
        {
            transform.position = Vector3.Lerp(transform.position, _newPos, transitionSpeed * Time.deltaTime);
        }

        if (Vector3.SqrMagnitude(_newPos - transform.position) < snapThreshold)
        {
            transform.position = _newPos;
            _isTransitioning = false;
            
            if (_isTransitioningToTransform)
            {
                _isTransitioningToTransform = false;
                
                //Action used to notify the tutorial system's ChangeCameraPivotCommand that transition is complete
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
    
    
    /// <summary>
    /// Allows changing the pivot position by setting a new position point directly using Vector3
    /// </summary>
    /// <param name="newPivot">The target world-space coordinates for the new pivot</param>
    public void ChangePivotPositionTransform(Vector3 newPivot)
    {
        _newPos = newPivot;
        _isTransitioning = true;
        _isTransitioningToTransform = true;
    }
}

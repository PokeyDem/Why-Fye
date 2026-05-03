using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[CreateAssetMenu(fileName = "Player Control", menuName = "Player Control/Player Control")]
public class PlayerControls : ScriptableObject, PlayerInputActions.IPlayerControlActions
{
    [SerializeField] private float doubleTapTimeWindow = 0.5f;
    [SerializeField] private float doubleTapDistanceTolerance = 0.05f;
    
    [NonSerialized] public Vector2 LookDelta;
    [NonSerialized] public bool IsOrbiting;
    [NonSerialized] public Vector2 OnScreenPosition;
    [NonSerialized] public bool IsPositioning;
    
    [NonSerialized] private float _lastTapTime;
    [NonSerialized] private Vector2 _lastTapPosition;
    
    private PlayerInputActions _playerInput;
    
    public event Action<int> OnSlotSelected;
    
    public event Action OnObjectPlaced;

    public event Action OnCameraPivotChanged;

    private int _currentSlot = 0;
    
    private void OnEnable()
    {
        PrepareInputSystem();
    }
    
    private void OnDisable()
    {
        IsOrbiting = false;
        IsPositioning = false;
        _playerInput.PlayerControl.Disable();
    }
    
    private void OnDestroy()
    {
        _playerInput.Dispose();
    }

    private void PrepareInputSystem()
    {
        _playerInput = new PlayerInputActions();
        _playerInput.PlayerControl.SetCallbacks(this);
        _playerInput.PlayerControl.Enable();
    }

    public void OnPlaceObject(InputAction.CallbackContext context)
    {
        if (context.canceled)
        {
            OnObjectPlaced?.Invoke();
        }
    }

    public void SelectSlot(int slot)
    {
        OnSlotSelected?.Invoke(slot);
        _currentSlot = slot;
    }

    public void OnSelectFirstSlot(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            OnSlotSelected?.Invoke(0);
            _currentSlot = 0;
        }
    }

    public void OnSelectSecondSlot(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            OnSlotSelected?.Invoke(1);
            _currentSlot = 1;
        }
    }

    public void OnSelectThirdSlot(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            OnSlotSelected?.Invoke(2);
            _currentSlot = 2;
        }
    }

    public void OnIncreaseSlotIndex(InputAction.CallbackContext context)
    {
        if (context.performed && _currentSlot < 2)
        {
            _currentSlot++;
            OnSlotSelected?.Invoke(_currentSlot);
        }
    }

    public void OnDecreseSlotIndex(InputAction.CallbackContext context)
    {
        if (context.performed && _currentSlot > 0)
        {
            _currentSlot--;
            OnSlotSelected?.Invoke(_currentSlot);
        }
    }

    public void OnRotation(InputAction.CallbackContext context)
    { 
        LookDelta = context.ReadValue<Vector2>();
    }

    public void OnCameraOrbitContact(InputAction.CallbackContext context)
    {
        if (context.started)
            IsOrbiting = true;
        
        if (context.canceled)
            IsOrbiting = false;
    }
    
    public void OnPointerPosition(InputAction.CallbackContext context)
    {
        OnScreenPosition = context.ReadValue<Vector2>();
    }

    public void OnDoubleTap(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            Debug.Log("Context started");
            float maxDistancePixels = Screen.width * doubleTapDistanceTolerance;
            
            float timeSinceLastTap = Time.time - _lastTapTime;
            float distance = Vector2.Distance(OnScreenPosition, _lastTapPosition);

            if (timeSinceLastTap <= doubleTapTimeWindow && distance <= maxDistancePixels)
            {
                Debug.Log("Double tapped");
                OnCameraPivotChanged?.Invoke();
                _lastTapTime = 0f;
            }
            else
            {
                Debug.Log("Doublen't tapped: " + timeSinceLastTap + " | " + doubleTapTimeWindow);
                _lastTapTime =  Time.time;
                _lastTapPosition = OnScreenPosition;
            }
        }
    }
}

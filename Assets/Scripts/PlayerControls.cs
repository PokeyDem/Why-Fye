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
    [SerializeField] private float PlacementTapIndicationDelay = 0.2f;
    [SerializeField] private float CancelPlacementOnMovementTreshold = 0.2f;
    
    [NonSerialized] public Vector2 LookDelta;
    [NonSerialized] public Vector2 PreviousDelta;
    [NonSerialized] public bool IsOrbiting;
    [NonSerialized] public Vector2 OnScreenPosition;
    
    [NonSerialized] private float _lastTapTime;
    [NonSerialized] private Vector2 _lastTapPosition;
    [NonSerialized] private bool IsHolding;
    
    private PlayerInputActions _playerInput;
    
    public event Action<int> OnSlotSelected;
    
    public event Action OnStartPlacement;
    public event Action OnStopPlacement;

    public event Action OnCameraPivotChanged;

    private int _currentSlot = 0;
    
    private void OnEnable()
    {
        PrepareInputSystem();
    }
    
    private void OnDisable()
    {
        IsOrbiting = false;
        IsHolding = false;
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
        if (context.performed)
        {
            IsHolding = true;
            Debug.Log("OnPlaced performed");
            OnStartPlacement?.Invoke();
        }

        if (context.canceled)
        {
            IsHolding = false;
            Debug.Log("OnPlaced canceled");
            OnStopPlacement?.Invoke();
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
        PreviousDelta = LookDelta;
        LookDelta = context.ReadValue<Vector2>();
        Debug.Log(PreviousDelta + " | " + LookDelta + " | " + Vector2.SqrMagnitude(LookDelta - PreviousDelta));
        
        if (context.started && Vector2.SqrMagnitude(LookDelta - PreviousDelta) > CancelPlacementOnMovementTreshold)
            OnStopPlacement?.Invoke();
    }

    public void OnCameraOrbitContact(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            IsOrbiting = true;
        }
        
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
            float maxDistancePixels = Screen.width * doubleTapDistanceTolerance;
            
            float timeSinceLastTap = Time.time - _lastTapTime;
            float distance = Vector2.Distance(OnScreenPosition, _lastTapPosition);

            if (timeSinceLastTap <= doubleTapTimeWindow && distance <= maxDistancePixels)
            {
                OnCameraPivotChanged?.Invoke();
                _lastTapTime = 0f;
            }
            else
            {
                _lastTapTime =  Time.time;
                _lastTapPosition = OnScreenPosition;
            }
        }
    }
}

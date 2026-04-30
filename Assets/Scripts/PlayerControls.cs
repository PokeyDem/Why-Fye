using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[CreateAssetMenu(fileName = "Player Control", menuName = "Player Control/Player Control")]
public class PlayerControls : ScriptableObject, PlayerInputActions.IPlayerControlActions
{
    [NonSerialized] public Vector2 LookDelta;
    [NonSerialized] public bool IsOrbiting;
    [NonSerialized] public Vector2 OnScreenPosition;
    
    private PlayerInputActions _playerInput;
    
    public event Action<int> OnSlotSelected;
    
    public event Action OnObjectPlaced;

    private int _currentSlot = 0;
    
    private void OnEnable()
    {
        PrepareInputSystem();
    }
    
    private void OnDisable()
    {
        IsOrbiting = false;
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
            OnObjectPlaced?.Invoke();
            Debug.Log("OnPlaceObject called");
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
        if (context.canceled)
            OnScreenPosition = context.ReadValue<Vector2>();
    }
}

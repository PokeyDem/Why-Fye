using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerControl : MonoBehaviour, PlayerInputActions.IPlayerControlActions
{
    private PlayerInputActions _playerInput;
    
    public static event Action<int> OnSlotSelected;
    public static event Action OnObjectPlaced;

    private int currentSlot = 0;

    private void Awake()
    {
        PrepareInputSystem();
    }
    
    private void OnEnable()
    {
        _playerInput.PlayerControl.Enable();
    }
    
    private void OnDisable()
    {
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
        currentSlot = slot;
    }

    public void OnSelectFirstSlot(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            OnSlotSelected?.Invoke(0);
            currentSlot = 0;
        }
    }

    public void OnSelectSecondSlot(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            OnSlotSelected?.Invoke(1);
            currentSlot = 1;
        }
    }

    public void OnSelectThirdSlot(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            OnSlotSelected?.Invoke(2);
            currentSlot = 2;
        }
    }

    public void OnIncreaseSlotIndex(InputAction.CallbackContext context)
    {
        if (context.performed && currentSlot < 2)
        {
            currentSlot++;
            OnSlotSelected?.Invoke(currentSlot);
        }
    }

    public void OnDecreseSlotIndex(InputAction.CallbackContext context)
    {
        if (context.performed && currentSlot > 0)
        {
            currentSlot--;
            OnSlotSelected?.Invoke(currentSlot);
        }
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HotbarSlotsManager : MonoBehaviour
{
    [SerializeField] private List<HotbarSlot> slots =  new List<HotbarSlot>();
    [SerializeField] private Color selectionColor;
    [SerializeField] private Color defaultColor;
    [SerializeField] private PlayerControls playerControls;

    [SerializeField] private DeviceAmountActualizer deviceAmountActualizer;

    [Tooltip("Scale range for the hotbar icons")] 
    [SerializeField] private Vector3 highlightedImageScale;
    [SerializeField] private Vector3 defaultImageScale;
    
    private int _selectedSlotIndex;
    
    private void OnEnable()
    {
        playerControls.OnSlotSelected += SelectSlot;
        ObjectPlacementSystem.OnObjectPlaced += HandleDecrease;
        ObjectPlacementSystem.OnDeviceAmountUpdate += SetAmountOfDevices;
        deviceAmountActualizer.OnDeviceAmountUpdate += SetAmountOfDevices;
    }

    private void OnDisable()
    {
        playerControls.OnSlotSelected -= SelectSlot;
        ObjectPlacementSystem.OnObjectPlaced -= HandleDecrease;
        ObjectPlacementSystem.OnDeviceAmountUpdate -= SetAmountOfDevices;
        deviceAmountActualizer.OnDeviceAmountUpdate -= SetAmountOfDevices;
    }

    private void HandleDecrease()
    {
        DecreaseAmountOfDevices(_selectedSlotIndex);
    }
    
    private void Start()
    {
        SelectSlot(0);
    }

    public void SelectSlot(int index)
    {
        DeselectSlot(_selectedSlotIndex);
        _selectedSlotIndex = index;
        slots[index].SetScale(highlightedImageScale);
        slots[index].EnableSelectionMask();
    }

    private void DeselectSlot(int index)
    {
        slots[index].SetScale(defaultImageScale);
        slots[index].DisableSelectionMask();
    }

    public void DeselectAllSlots()
    {
        for (int i = 0; i < slots.Count; i++)
        {
            slots[i].SetScale(defaultImageScale);
            slots[i].DisableSelectionMask();
        }
    }

    private void SetAmountOfDevices(List<int> amountOfDevices)
    {
        Debug.Log("HotbarSlotsManager heard the update call");
        for (int i = 0; i < amountOfDevices.Count; i++)
        {
            Debug.Log($"Setting amount of devices for {i} to " + amountOfDevices[i]);
            slots[i].SetCounter(amountOfDevices[i]);
        }
    }

    private void DecreaseAmountOfDevices(int slot)
    {
        slots[slot].DecreaseCounter();
    }

    public int GetRemoveModeSlotIndex()
    {
        return (slots.Count - 1);
    }
    
}

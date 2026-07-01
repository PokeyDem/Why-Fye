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

    private void SelectSlot(int index)
    {
        DeselectSlot(_selectedSlotIndex);
        _selectedSlotIndex = index;
        slots[index].ChangeFrameState(true);
    }

    private void DeselectSlot(int index)
    {
        slots[index].ChangeFrameState(false);
    }

    public void DeselectAllSlots()
    {
        for (int i = 0; i < slots.Count; i++)
        {
            slots[i].ChangeFrameState(false);
        }
    }

    private void SetAmountOfDevices(List<int> amountOfDevices)
    {
        for (int i = 0; i < amountOfDevices.Count; i++)
        {
            slots[i].SetCounter(amountOfDevices[i]);
        }
    }

    private void DecreaseAmountOfDevices(int slot)
    {
        slots[slot].DecreaseCounter();
    }
    
}

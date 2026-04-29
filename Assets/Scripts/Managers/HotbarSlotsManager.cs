using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HotbarSlotsManager : MonoBehaviour
{
    [SerializeField] private List<HotbarSlot> slots =  new List<HotbarSlot>();
    [SerializeField] private Color selectionColor;
    [SerializeField] private Color defaultColor;
    
    private int _selectedSlotIndex;
    
    private void OnEnable()
    {
        PlayerControl.OnSlotSelected += SelectSlot;
        ObjectPlacementSystem.OnObjectPlaced += HandleDecrease;
        ObjectPlacementSystem.OnInitialization += SetAmountOfDevices;
    }

    private void OnDisable()
    {
        PlayerControl.OnSlotSelected -= SelectSlot;
        ObjectPlacementSystem.OnObjectPlaced -= HandleDecrease;
        ObjectPlacementSystem.OnInitialization -= SetAmountOfDevices;
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
        DiselectSlot(_selectedSlotIndex);
        _selectedSlotIndex = index;
        slots[index].ChangeFrameState(true);
    }

    private void DiselectSlot(int index)
    {
        slots[index].ChangeFrameState(false);
    }

    private void SetAmountOfDevices(Dictionary<int, int> amountOfDevices)
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

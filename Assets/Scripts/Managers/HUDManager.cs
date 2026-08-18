using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUDManager : MonoBehaviour
{
   [SerializeField] private Button completeLevelButton;
   [SerializeField] private HotbarSlotsManager hotbarSlotsManager;
   [SerializeField] private GameObject removeModeIconFrame;

   private void Start()
   {
      completeLevelButton.gameObject.SetActive(false);
   }

   public void ShowCompleteButton()
   {
      completeLevelButton.gameObject.SetActive(true);
   }

   public void HideCompleteButton()
   {
      completeLevelButton.gameObject.SetActive(false);
   }

   public void EnableRemoveMode()
   {
      hotbarSlotsManager.SelectSlot(hotbarSlotsManager.GetRemoveModeSlotIndex());
   }

   public void HideRemoveModeIconFrame()
   {
      removeModeIconFrame.SetActive(false);
   }
}

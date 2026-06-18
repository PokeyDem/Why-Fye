using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingsMenuManager : MonoBehaviour
{
    [SerializeField] ActionConfirmationManager actionConfirmationManager;
    
    public void OnClearSaveDataButtonClick()
    {
        actionConfirmationManager.ProcessConfirmation(() =>
        {
            SaveManager.Instance.ClearSaveData();
            GameManager.Instance.ResetCompletedLevels();
        }
    );
    }
}

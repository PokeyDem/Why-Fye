using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionConfirmationManager : MonoBehaviour
{
    [SerializeField] private GameObject confirmationWindow;
    private Action _currentActionToConfirm;
    private void ShowConfirmationWindow()
    {
        confirmationWindow.SetActive(true);
    }

    private void HideConfirmationWindow()
    {
        confirmationWindow.SetActive(false);
    }

    public void ProcessConfirmation(Action onUserConfirmation)
    {
        _currentActionToConfirm = onUserConfirmation;
        Debug.Log("Current action to confirm set");
        ShowConfirmationWindow();
    }

    public void OnConfirmButtonClick()
    {
        _currentActionToConfirm?.Invoke();
        Debug.Log("Current action to confirm invoked");
        _currentActionToConfirm = null;
        HideConfirmationWindow();
    }

    public void OnDeclineButtonClick()
    {
        _currentActionToConfirm = null;
        HideConfirmationWindow();
    }
}

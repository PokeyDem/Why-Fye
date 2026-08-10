using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionConfirmationManager : MonoBehaviour
{
    [SerializeField] private CanvasGroup confirmationWindow;
    private Action _currentActionToConfirm;
    private void ShowConfirmationWindow()
    {
        confirmationWindow.alpha = 1;
        confirmationWindow.interactable = true;
        confirmationWindow.blocksRaycasts = true;
    }

    private void HideConfirmationWindow()
    {
        confirmationWindow.alpha = 0;
        confirmationWindow.interactable = false;
        confirmationWindow.blocksRaycasts = false;
    }

    public void ProcessConfirmation(Action onUserConfirmation)
    {
        _currentActionToConfirm = onUserConfirmation;
        ShowConfirmationWindow();
    }

    public void OnConfirmButtonClick()
    {
        _currentActionToConfirm?.Invoke();
        _currentActionToConfirm = null;
        HideConfirmationWindow();
    }

    public void OnDeclineButtonClick()
    {
        _currentActionToConfirm = null;
        HideConfirmationWindow();
    }
}

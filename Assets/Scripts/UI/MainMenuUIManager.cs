using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuUIManager : MonoBehaviour
{
    [SerializeField] private GameObject levelMenuElements;
    [SerializeField] private GameObject mainManuElements;
    [SerializeField] private GameObject creditsMenuElements;
    [SerializeField] private GameObject controlsMenuElements;
    [SerializeField] private GameObject settingMenuElements;
    [SerializeField] private List<LevelButtonBehaviour> levelButtons = new List<LevelButtonBehaviour>();
    [SerializeField] private Color unlockedTextColor;
    [SerializeField] private Color lockedTextColor;
    [SerializeField] private int unlockedButtonAlpha;
    [SerializeField] private int lockedButtonAlpha;

    private void Start()
    {
        foreach (var levelButtonBehaviour in levelButtons)
        {
            levelButtonBehaviour.Initialize(unlockedTextColor, lockedTextColor, unlockedButtonAlpha, lockedButtonAlpha);
            
        }
    }

    public void ValidateLevelButtons(List<bool> completedLevels)
    {
        for (int i = 0; i < levelButtons.Count; i++)
        {
            levelButtons[i].gameObject.SetActive(true);
            bool isUnlocked = (i == 0) || completedLevels[i] || completedLevels[i - 1];
            
            if (!isUnlocked)
            {
                levelButtons[i].LockButton();
                continue;
            }

            if (completedLevels[i])
            {
                levelButtons[i].SetCompleted();
            }
            else
            {
                levelButtons[i].UnlockButton();
            }
            levelButtons[i].gameObject.SetActive(true);
        }
    }

    public void EnableLevelMenuElements()
    {
        levelMenuElements.SetActive(true);
    }

    public void DisableLevelMenuElements()
    {
        levelMenuElements.SetActive(false);
    }

    public void EnableMainMenuElements()
    {
        mainManuElements.SetActive(true);
    }

    public void DisableMainMenuElements()
    {
        mainManuElements.SetActive(false);
    }

    public void EnableCreditsMenuElements()
    {
        creditsMenuElements.SetActive(true);
    }

    public void EnableControlsMenuElements()
    {
        controlsMenuElements.SetActive(true);
    }

    public void EnableSettingsMenuElements()
    {
        settingMenuElements.SetActive(true);
    }

    public void DisableAllSubMenus()
    {
        levelMenuElements.SetActive(false);
        creditsMenuElements.SetActive(false);
        controlsMenuElements.SetActive(false);
        settingMenuElements.SetActive(false);
    }
}

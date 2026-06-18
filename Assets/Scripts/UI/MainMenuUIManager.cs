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
    [SerializeField] private List<Button> levelButtons = new List<Button>();
    [SerializeField] private Color activeButtonColor;
    [SerializeField] private Color inactiveButtonColor;
    [SerializeField] private Color completedLevelButtonColor;
    
    public void ValidateLevelButtons(List<bool> completedLevels)
    {

        for (int i = 0; i < levelButtons.Count; i++)
        {
            bool isUnlocked = (i == 0) || completedLevels[i] || completedLevels[i - 1];
          
            levelButtons[i].interactable = isUnlocked;

            var block = levelButtons[i].colors;
            
            block.normalColor = isUnlocked ? activeButtonColor : inactiveButtonColor;

            if (completedLevels[i])
            {
                block.normalColor = completedLevelButtonColor;
            }
    
            levelButtons[i].colors = block;
        }
    }
    public List<Button> GetLevelButtons()
    {
        return levelButtons;
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

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuUIManager : MonoBehaviour
{
    [Tooltip("Parent object of the each sub menu UI")]
    [SerializeField] private CanvasGroup levelMenuElements;
    [SerializeField] private CanvasGroup mainManuElements;
    [SerializeField] private CanvasGroup creditsMenuElements;
    [SerializeField] private CanvasGroup controlsMenuElements;
    [SerializeField] private CanvasGroup settingMenuElements;
    [SerializeField] private CanvasGroup stagesMenuElements;
    
    [SerializeField] private List<LevelButtonBehaviour> levelButtons = new List<LevelButtonBehaviour>();
    
    [Tooltip("Colors and alphas for the level buttons")]
    [SerializeField] private Color unlockedTextColor;
    [SerializeField] private Color lockedTextColor;
    [SerializeField] private int unlockedLevelButtonAlpha;
    [SerializeField] private int lockedLevelButtonAlpha;
    
    [SerializeField] private List<StageButtonBehaviour> stageButtons = new List<StageButtonBehaviour>();
   
    private List<StageLevelsData> _completedLevels;
    private bool _buttonsInitialized = false;

    public void ValidateStageButtons(List<StageLevelsData> completedLevels)
    {
        if (!_buttonsInitialized)
        {
            InitializeButtons();
            _buttonsInitialized = true;
        }
        
        _completedLevels = completedLevels;
        for (int i = 0; i < completedLevels.Count; i++)
        {
            if (completedLevels[i].isStageUnlocked)
            {
                stageButtons[i].UnlockButton();
            }
            else
            {
                stageButtons[i].LockButton();
            }
        }
    }

    private void InitializeButtons()
    {
        foreach (var levelButtonBehaviour in levelButtons)
        {
            levelButtonBehaviour.Initialize(unlockedTextColor, lockedTextColor, unlockedLevelButtonAlpha, lockedLevelButtonAlpha);
        }
    }

    public void ValidateLevelButtons(int stageNum)
    {
        Debug.Log("Validating level buttons");
        for (int i = 0; i < levelButtons.Count; i++)
        {
            levelButtons[i].gameObject.SetActive(true);
            bool isUnlocked = (i == 0) || _completedLevels[stageNum].levelsUnlocked[i] || _completedLevels[stageNum].levelsUnlocked[i - 1];
            
            if (!isUnlocked)
            {
                levelButtons[i].LockButton();
                continue;
            }

            if (_completedLevels[stageNum].levelsUnlocked[i])
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
    
    public void EnableMainMenuElements()
    {
        mainManuElements.alpha = 1;
        mainManuElements.interactable = true;
        mainManuElements.blocksRaycasts = true;
    }

    public void DisableMainMenuElements()
    {
        mainManuElements.alpha = 0;
        mainManuElements.interactable = false;
        mainManuElements.blocksRaycasts = false;
    }
    
    public void EnableLevelMenuElements()
    {
        levelMenuElements.alpha = 1;
        levelMenuElements.interactable = true;
        levelMenuElements.blocksRaycasts = true;
    }

    private void DisableLevelMenuElements()
    {
        levelMenuElements.alpha = 0;
        levelMenuElements.interactable = false;
        levelMenuElements.blocksRaycasts = false;
    }

    public void EnableCreditsMenuElements()
    {
        creditsMenuElements.alpha = 1;
        creditsMenuElements.interactable = true;
        creditsMenuElements.blocksRaycasts = true;
    }

    private void DisableCreditsMenuElements()
    {
        creditsMenuElements.alpha = 0;
        creditsMenuElements.interactable = false;
        creditsMenuElements.blocksRaycasts = false;
    }

    public void EnableControlsMenuElements()
    {
        controlsMenuElements.alpha = 1;
        controlsMenuElements.interactable = true;
        controlsMenuElements.blocksRaycasts = true;
    }

    private void DisableControlsMenuElements()
    {
        controlsMenuElements.alpha = 0;
        controlsMenuElements.interactable = false;
        controlsMenuElements.blocksRaycasts = false;
    }

    public void EnableSettingsMenuElements()
    {
        settingMenuElements.alpha = 1;
        settingMenuElements.interactable = true;
        settingMenuElements.blocksRaycasts = true;
    }

    private void DisableSettingsMenuElements()
    {
        settingMenuElements.alpha = 0;
        settingMenuElements.interactable = false;
        settingMenuElements.blocksRaycasts = false;
    }

    public void EnableStagesMenuElements()
    {
        stagesMenuElements.alpha = 1;
        stagesMenuElements.interactable = true;
        stagesMenuElements.blocksRaycasts = true;
    }

    private void DisableStagesMenuElements()
    {
        stagesMenuElements.alpha = 0;
        stagesMenuElements.interactable = false;
        stagesMenuElements.blocksRaycasts = false;
    }

    public void DisableAllSubMenus()
    {
        DisableLevelMenuElements();
        DisableCreditsMenuElements();
        DisableControlsMenuElements();
        DisableSettingsMenuElements();
        DisableStagesMenuElements();
    }
}

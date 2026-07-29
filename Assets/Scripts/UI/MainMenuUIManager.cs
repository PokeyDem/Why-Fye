using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuUIManager : MonoBehaviour
{
    [Tooltip("Parent object of the each sub menu UI")]
    [SerializeField] private GameObject levelMenuElements;
    [SerializeField] private GameObject mainManuElements;
    [SerializeField] private GameObject creditsMenuElements;
    [SerializeField] private GameObject controlsMenuElements;
    [SerializeField] private GameObject settingMenuElements;
    [SerializeField] private GameObject stagesMenuElements;
    
    [SerializeField] private List<LevelButtonBehaviour> levelButtons = new List<LevelButtonBehaviour>();
    
    [Tooltip("Colors and alphas for the level buttons")]
    [SerializeField] private Color unlockedTextColor;
    [SerializeField] private Color lockedTextColor;
    [SerializeField] private int unlockedLevelButtonAlpha;
    [SerializeField] private int lockedLevelButtonAlpha;
    
    [SerializeField] private List<StageButtonBehaviour> stageButtons = new List<StageButtonBehaviour>();
   
    private List<StageLevelsData> _completedLevels;

    private void Start()
    {
        foreach (var levelButtonBehaviour in levelButtons)
        {
            levelButtonBehaviour.Initialize(unlockedTextColor, lockedTextColor, unlockedLevelButtonAlpha, lockedLevelButtonAlpha);
            
        }
    }

    public void ValidateStageButtons(List<StageLevelsData> completedLevels)
    {
        _completedLevels = completedLevels;
        for (int i = 0; i < completedLevels.Count; i++)
        {
            if (completedLevels[i].isStageUnlocked)
            {
                stageButtons[i].UnlockButton();
                Debug.Log("Stage " + i + " is unlocked");
            }
            else
            {
                stageButtons[i].LockButton();
                Debug.Log("Stage " + i + " is locked");
            }
        }
    }

    public void ValidateLevelButtons(int stageNum)
    {
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

    public void EnableLevelMenuElements()
    {
        levelMenuElements.SetActive(true);
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

    public void EnableStagesMenuElements()
    {
        stagesMenuElements.SetActive(true);
    }

    public void DisableAllSubMenus()
    {
        levelMenuElements.SetActive(false);
        creditsMenuElements.SetActive(false);
        controlsMenuElements.SetActive(false);
        settingMenuElements.SetActive(false);
        stagesMenuElements.SetActive(false);
    }
}

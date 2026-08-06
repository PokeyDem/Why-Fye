using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour
{
    [SerializeField] private string baseLevelSceneName;
    [SerializeField] private SceneTransitionManager sceneTransitionManager;
    [SerializeField] private MainMenuUIManager mainMenuUIManager;

    private bool _isInAction;

    private void OnEnable()
    {
        GameManager.OnLevelButtonsValidationRequest += ValidateStages;
    }
    
    private void OnDisable()
    {
        GameManager.OnLevelButtonsValidationRequest -= ValidateStages;
    }

    private void Start()
    {
        StartCoroutine(sceneTransitionManager.PlayFadeIn());
        if (GameManager.Instance.IsSaveLoaded() || GameManager.Instance.IsInitialized())
        {
            ValidateStages();
        }
        
        if (GameManager.Instance.GetLoadedFromLevel())
        {
            SwitchToLevelMenu();
        }
    }
    
    public void OnLevelButtonClick(int levelIndex)
    {
        if (_isInAction)
            return;
        _isInAction = true;
        GameManager.Instance.SetTargetLevel(levelIndex);
        StartCoroutine(sceneTransitionManager.PlayFadeOut());
        StartCoroutine(LoadLevel());
    }

    public void OnStageButtonClick(int stageIndex)
    {
        GameManager.Instance.SetTargetLevelStage(stageIndex);
        mainMenuUIManager.ValidateLevelButtons(stageIndex);
        mainMenuUIManager.DisableAllSubMenus();
        mainMenuUIManager.EnableLevelMenuElements();
    }

    private IEnumerator LoadLevel()
    {
        yield return StartCoroutine(sceneTransitionManager.PlayFadeOut());
        SceneManager.LoadSceneAsync(baseLevelSceneName);
        _isInAction = false;
    }
    
    private void SwitchToMainMenu()
    {
        mainMenuUIManager.DisableAllSubMenus();
        mainMenuUIManager.EnableMainMenuElements(); 
    }

    public void SwitchToLevelMenu()
    {
        mainMenuUIManager.DisableMainMenuElements();
        mainMenuUIManager.EnableLevelMenuElements();
    }

    public void SwitchToCreditsMenu()
    {
        mainMenuUIManager.DisableMainMenuElements();
        mainMenuUIManager.EnableCreditsMenuElements();
    }

    public void SwitchToControlsMenu()
    {
        mainMenuUIManager.DisableMainMenuElements();
        mainMenuUIManager.EnableControlsMenuElements();
    }

    public void SwitchToSettingsMenu()
    {
        mainMenuUIManager.DisableMainMenuElements();
        mainMenuUIManager.EnableSettingsMenuElements();
    }

    public void OnStartButtonClick()
    {
        mainMenuUIManager.DisableMainMenuElements();
        mainMenuUIManager.EnableStagesMenuElements();
    }

    public void OnBackButtonClick()
    {
        SwitchToMainMenu();
    }

    private void ValidateStages()
    {
        Debug.Log("Validating stages");
        mainMenuUIManager.ValidateStageButtons(GameManager.Instance.GetUnlockedLevelsData());
        mainMenuUIManager.ValidateLevelButtons(GameManager.Instance.GetTargetLevelStage());
        Debug.Log("Validating: " + GameManager.Instance.GetUnlockedLevelsData().Count + " stages");
    }
    
    public void OnExitButtonClick(){
        #if UNITY_EDITOR
            EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }
}

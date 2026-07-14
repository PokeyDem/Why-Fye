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
        GameManager.OnLevelButtonsValidationRequest += ValidateLevelButtons;
    }

    private void OnDisable()
    {
       GameManager.OnLevelButtonsValidationRequest -= ValidateLevelButtons;
    }

    private void Start()
    {
        StartCoroutine(sceneTransitionManager.PlayFadeIn());
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

    private void ValidateLevelButtons()
    {
        mainMenuUIManager.ValidateLevelButtons(GameManager.Instance.GetUnlockedLevelsData());
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
        // ValidateLevelButtons();
    }

    public void SwitchToLevelMenu()
    {
        mainMenuUIManager.DisableMainMenuElements();
        mainMenuUIManager.EnableLevelMenuElements();
        mainMenuUIManager.ValidateLevelButtons(GameManager.Instance.GetUnlockedLevelsData());
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
        SwitchToLevelMenu();
    }

    public void OnBackButtonClick()
    {
        SwitchToMainMenu();
    }
    
    public void OnExitButtonClick(){
        #if UNITY_EDITOR
            EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }
}

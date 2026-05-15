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

    private void Start()
    {
        StartCoroutine(sceneTransitionManager.PlayFadeIn());
        GameManager.Instance.InitButtons(mainMenuUIManager.GetLevelButtons());
        GameManager.Instance.ValidateLevelButtons();
        if (GameManager.Instance.GetLoadedFromLevel())
        {
            SwitchToLevelMenu();
        }
    }
    
    public void OnLevelButtonClick(int levelIndex)
    {
        GameManager.Instance.SetTargetLevel(levelIndex);
        StartCoroutine(sceneTransitionManager.PlayFadeOut());
        StartCoroutine(LoadLevel());
    }

    private IEnumerator LoadLevel()
    {
        yield return StartCoroutine(sceneTransitionManager.PlayFadeOut());
        SceneManager.LoadSceneAsync(baseLevelSceneName);
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

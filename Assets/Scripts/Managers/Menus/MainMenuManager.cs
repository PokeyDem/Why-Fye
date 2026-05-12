using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour
{
    [SerializeField] private GameObject levelMenuElements;
    [SerializeField] private GameObject mainManuElements;
    [SerializeField] private GameObject creditsMenuElements;
    [SerializeField] private GameObject controlsMenuElements;
    [SerializeField] private List<Button> levelButtons = new List<Button>();
    [SerializeField] private string baseLevelSceneName;
    [SerializeField] private SceneTransitionManager sceneTransitionManager;

    private void Start()
    {
        GameManager.Instance.InitButtons(levelButtons);
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
        //TODO Add TransitionManager and fade in / fade out
        //TODO Then assign to the buttons *Eventually create a script for that
        StartCoroutine(LoadLevel());
    }

    private IEnumerator LoadLevel()
    {
        yield return StartCoroutine(sceneTransitionManager.PlayFadeOut());
        SceneManager.LoadSceneAsync(baseLevelSceneName);
    }
    
    private void SwitchToMainMenu()
    {
        levelMenuElements.SetActive(false);
        creditsMenuElements.SetActive(false);
        controlsMenuElements.SetActive(false);
        mainManuElements.SetActive(true);
    }

    public void SwitchToLevelMenu()
    {
        mainManuElements.SetActive(false);
        levelMenuElements.SetActive(true);
    }

    public void SwitchToCreditsMenu()
    {
        mainManuElements.SetActive(false);
        creditsMenuElements.SetActive(true);
    }

    public void SwitchToControlsMenu()
    {
        mainManuElements.SetActive(false);
        controlsMenuElements.SetActive(true);
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

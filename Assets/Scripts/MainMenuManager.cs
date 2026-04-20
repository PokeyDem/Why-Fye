using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour
{
    [SerializeField] private GameObject levelMenuElements;
    [SerializeField] private GameObject mainManuElements;
    [SerializeField] private GameObject creditsMenuElements;
    [SerializeField] private GameObject controlsMenuElements;
    [SerializeField] private List<Button> levelButtons = new List<Button>();

    private void Start()
    {
        GameManager gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        gameManager.InitButtons(levelButtons);
        gameManager.ValidateLevelButtons();
        if (gameManager.GetLoadedFromLevel())
        {
            SwitchToLevelMenu();
        }
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

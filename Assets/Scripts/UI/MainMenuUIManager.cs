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
    [SerializeField] private List<Button> levelButtons = new List<Button>();

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

    public void DisableCreditsMenuElements()
    {
        creditsMenuElements.SetActive(false);
    }

    public void EnableControlsMenuElements()
    {
        controlsMenuElements.SetActive(true);
    }

    public void DisableControlsMenuElements()
    {
        controlsMenuElements.SetActive(false);
    }

    public void DisableAllSubMenus()
    {
        levelMenuElements.SetActive(false);
        creditsMenuElements.SetActive(false);
        controlsMenuElements.SetActive(false);
    }
    
    
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenuManager : MonoBehaviour{

    [SerializeField] private GameObject _pauseMenu;
    [SerializeField] private LevelManager _levelManager;
    private bool _isPaused = false;

    public static event Action OnPause;
    public static event Action OnResume;
    
    private void Update(){
        if (Input.GetKeyDown(KeyCode.Escape)){
            EscPressed();
        } 
    }

    public void EscPressed(){
        if (_isPaused)
            Resume();
        else{
            Pause();
        }
    }

    public void Pause(){
        OnPause?.Invoke();
        _pauseMenu.SetActive(true);
        Time.timeScale = 0f;
        _isPaused = true;
    }

    public void Resume(){
        OnResume?.Invoke();
        _pauseMenu.SetActive(false);
        Time.timeScale = 1f;
        _isPaused = false;
    }

    public void OnExitToMenuButtonClick(){
        Resume();
        _levelManager.GoToMainMenu();
    }
 
}

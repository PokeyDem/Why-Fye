using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    [SerializeField] SceneTransitionManager sceneTransitionManager;
    public void LoadLevel(int index)
    {
        sceneTransitionManager.MakeTransition(() => { SceneManager.LoadSceneAsync("Level_" + index);});
    }

    public void LoadMainMenu()
    {
        sceneTransitionManager.MakeTransition(() => { SceneManager.LoadSceneAsync("MainMenu");});
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    [SerializeField] private SceneTransitionManager sceneTransitionManager;
    [SerializeField] private String baseLevelName;
    [SerializeField] private String levelName;
    [SerializeField] private String mainMenuLevelName;

    private int _currentSceneIndex;
    public void SwitchLevelEnv(int index, Action onCleanUp, Action onInitialization, bool isInitialBoot)
    {
        StartCoroutine(LoadSequenceCoroutine(index, onCleanUp, onInitialization, isInitialBoot));
    }

    private IEnumerator LoadSequenceCoroutine(int index, Action onCleanUp, Action onInitialization, bool isInitialBoot)
    {
        if (!isInitialBoot)
        {
            yield return StartCoroutine(sceneTransitionManager.PlayFadeOut());
        }

        if (index-1 > 0)
        {
            onCleanUp?.Invoke();
            string sceneToUnload = levelName + (index-1) + "_Env";
            AsyncOperation unloadOp = SceneManager.UnloadSceneAsync(sceneToUnload);
            yield return unloadOp;
        }
        
        string sceneToLoad = levelName + index + "_Env";
        AsyncOperation loadOp = SceneManager.LoadSceneAsync(sceneToLoad, LoadSceneMode.Additive);

        while (!loadOp.isDone)
        {
            yield return null;
        }
        
        onInitialization?.Invoke();
        
        yield return StartCoroutine(sceneTransitionManager.PlayFadeIn());
    }
  
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField] private List<bool> completedLevels;
    [SerializeField] private MainMenuUIManager mainMenuUIManager;
    private bool _saveLoaded = false;

    private bool _isInitialized = false;
   
    public static GameManager Instance;

    private bool _loadedFromLevel = false;

    private bool _levelLoaded;

    private int _targetLevelToLoad;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        if (!_saveLoaded)
        {
            SaveManager.Instance.LoadGameFromFile();
            _saveLoaded = true;
        }
    }

    private void Initialize()
    {
        if (_saveLoaded)
            return;
        foreach (Button button in mainMenuUIManager.GetLevelButtons())
        {
            completedLevels.Add(false);
        }
        
        _isInitialized = true;
    }

    public void MarkAsCompleted(int level)
    {
        completedLevels[level] = true;
        SaveManager.Instance.SaveGameToFile();
    }

    public void ValidateLevelButtons()
    {
        if (!_isInitialized)
            Initialize();
        
        mainMenuUIManager.ValidateLevelButtons(completedLevels);
    }

    public void SetLoadedFromLevel(bool loaded)
    {
        _loadedFromLevel = loaded;
    }

    public bool GetLoadedFromLevel()
    {
        return _loadedFromLevel;
    }

    public void SetTargetLevel(int index)
    {
        _targetLevelToLoad = index;
    }

    public int GetTargetLevel()
    {
        return _targetLevelToLoad;
    }

    public List<bool> GetUnlockedLevelsData()
    {
        return completedLevels;
    }

    public void LoadUnlockedLevelsData(UnlockedLevelsData unlockedLevelsData)
    {
        completedLevels = unlockedLevelsData.unlockedLevels;
        ValidateLevelButtons();
    }
}

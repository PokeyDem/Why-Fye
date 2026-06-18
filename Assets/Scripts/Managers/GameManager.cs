using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField] private int amountOfLevels;
    [SerializeField] private List<bool> completedLevels;
    
    private bool _saveLoaded = false;

    private bool _isInitialized = false;
   
    public static GameManager Instance;

    private bool _loadedFromLevel = false;

    private bool _levelLoaded;

    private int _targetLevelToLoad;

    public static event Action OnLevelButtonsValidationRequest;

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
            List<bool> loadedSave = SaveManager.Instance.LoadGameFromFile();
            if (loadedSave != null)
            {
                _saveLoaded = true;
                LoadUnlockedLevelsData(loadedSave);
            }
            else
            {
                Initialize();
            }
        }
        OnLevelButtonsValidationRequest?.Invoke();
    }

    private void Initialize()
    {

        if (_isInitialized)
            return;
        
        completedLevels = new List<bool>();
        for (int i = 0; i < amountOfLevels; i++)
        {
            completedLevels.Add(false);
        }
        
        _isInitialized = true;
        OnLevelButtonsValidationRequest?.Invoke();
    }

    public void MarkAsCompleted(int level)
    {
        completedLevels[level] = true;
        SaveManager.Instance.SaveGameToFile();
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

    private void LoadUnlockedLevelsData(List<bool> unlockedLevels)
    {
        completedLevels = unlockedLevels;
        OnLevelButtonsValidationRequest?.Invoke();
    }

    public int GetAmountOfLevels()
    {
        return amountOfLevels;
    }
    
    public void ResetCompletedLevels()
    {
        completedLevels = new List<bool>();
        for (int i = 0; i < amountOfLevels; i++)
        {
            completedLevels.Add(false);
        }
        
        SaveManager.Instance.SaveGameToFile();
    }
}

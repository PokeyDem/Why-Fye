using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField] private int amountOfStages;
    [SerializeField] private int amountOfLevels;
    [SerializeField] private List<StageLevelsData> completedLevels;
    
    private bool _saveLoaded = false;

    private bool _isInitialized = false;
   
    public static GameManager Instance;

    private bool _loadedFromLevel = false;

    private bool _levelLoaded;

    private int _targetLevelToLoad;

    private int _targetLevelStage;

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
            List<StageLevelsData> loadedSave = SaveManager.Instance.LoadGameFromFile();
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

    private List<StageLevelsData> InitializeStagesAndLevels()
    {
        List<StageLevelsData> stagesAndLevels = new List<StageLevelsData>();
        for (int i = 0; i < amountOfStages; i++)
        {
            StageLevelsData currentStage = new StageLevelsData();
            List<bool> currentLevels = new List<bool>();
            for (int j = 0; j < amountOfLevels; j++)
            {
                currentLevels.Add(false);
            }
            currentStage.levelsUnlocked = currentLevels;
            stagesAndLevels.Add(currentStage);
            if (i == 0)
                currentStage.isStageUnlocked = true;
            else
                currentStage.isStageUnlocked = false;
        }
        return stagesAndLevels;
    }

    private void Initialize()
    {

        if (_isInitialized)
            return;
        
        completedLevels = InitializeStagesAndLevels();
        
        _isInitialized = true;
        OnLevelButtonsValidationRequest?.Invoke();
    }

    public void MarkAsCompleted(int stage, int level)
    {
        completedLevels[stage].levelsUnlocked[level] = true;
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

    public void SetTargetLevelStage(int stageIndex)
    {
        _targetLevelStage = stageIndex;
    }

    public int GetTargetLevelStage()
    {
        return _targetLevelStage;
    }

    public List<StageLevelsData> GetUnlockedLevelsData()
    {
        return completedLevels;
    }

    private void LoadUnlockedLevelsData(List<StageLevelsData> unlockedLevels)
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
        completedLevels = InitializeStagesAndLevels();
        
        SaveManager.Instance.SaveGameToFile();
    }
}

[Serializable]
public struct StageLevelsData
{
    public int stageNumber;
    public bool isStageUnlocked;
    public bool isStageCompleted;
    public List<bool> levelsUnlocked;
}

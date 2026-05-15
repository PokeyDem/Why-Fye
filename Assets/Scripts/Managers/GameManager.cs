using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField] private List<bool> completedLevels;
    [SerializeField] private List<Button> levelButtons = new List<Button>();
    [SerializeField] private Color activeColor;
    [SerializeField] private Color lockedColor;
    [SerializeField] private Color completedColor;

    private bool _isInitialized = false;
   
    public static GameManager Instance;

    private bool _loadedFromLevel = false;

    private bool _allLevelsCompleted = false;

    private int _levelIndex;

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
    
    private void Initialize()
    {
        foreach (Button button in levelButtons)
        {
            completedLevels.Add(false);
        }

        _isInitialized = true;
    }

    public void MarkAsCompleted(int level)
    {
        completedLevels[level] = true;
    }

    public void ValidateLevelButtons()
    {
        int completedLevelsCounter = 0;
        if (!_isInitialized)
            Initialize();

        for (int i = 0; i < levelButtons.Count; i++)
        {
            bool isUnlocked = (i == 0) || completedLevels[i] || completedLevels[i - 1];

          
            levelButtons[i].interactable = isUnlocked;

            var block = levelButtons[i].colors;
            
            block.normalColor = isUnlocked ? activeColor : lockedColor;

            if (completedLevels[i])
            {
                block.normalColor = completedColor;
            }
    
            levelButtons[i].colors = block;

            if (isUnlocked)
                completedLevelsCounter++;
        }

        if (completedLevelsCounter == levelButtons.Count)
            _allLevelsCompleted = true;
    }

    public void InitButtons(List<Button> buttons)
    {
        levelButtons =  buttons;
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
}

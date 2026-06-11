using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveManager : MonoBehaviour
{
    public static SaveManager Instance;

    [SerializeField] private string savePath;
    [SerializeField] private GameManager gameManager;
    private JsonDataService _jsonDataService = new JsonDataService();
    
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
    }

    public void SaveGameToFile()
    {
        UnlockedLevelsData unlockedLevelsData = new UnlockedLevelsData(gameManager.GetUnlockedLevelsData());
        SaveData saveData = new SaveData(unlockedLevelsData);
        _jsonDataService.SaveData(savePath, saveData, true);
    }

    public void LoadGameFromFile()
    {
        if (!_jsonDataService.DoesFileExist(savePath))
            return;
        
        SaveData saveData = _jsonDataService.LoadData<SaveData>(savePath, true);
        gameManager.LoadUnlockedLevelsData(saveData.unlockedLevels);
    }
}

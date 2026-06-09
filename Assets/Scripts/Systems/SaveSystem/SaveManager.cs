using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveManager : MonoBehaviour
{
    public static SaveManager instance;

    [SerializeField] private string savePath;
    [SerializeField] private GameManager gameManager;
    private JsonDataService _jsonDataService = new JsonDataService();
    
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (instance != this)
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

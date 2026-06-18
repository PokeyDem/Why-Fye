using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveManager : MonoBehaviour
{
    public static SaveManager Instance;

    [SerializeField] private string savePath;
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
        UnlockedLevelsData unlockedLevelsData = new UnlockedLevelsData(GameManager.Instance.GetUnlockedLevelsData());
        SaveData saveData = new SaveData(unlockedLevelsData);
        _jsonDataService.SaveData(savePath, saveData, true);
    }

    public List<bool> LoadGameFromFile()
    {
        if (!_jsonDataService.DoesFileExist(savePath))
            return null;
        
        SaveData saveData = _jsonDataService.LoadData<SaveData>(savePath, true);
        return saveData.unlockedLevelsData.unlockedLevels;
    }

    public void ClearSaveData()
    {
        _jsonDataService.ClearData(savePath);
    }
}

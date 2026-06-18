using System;
using System.IO;
using Newtonsoft.Json;
using UnityEngine;

public class JsonDataService : MonoBehaviour
{
    public bool SaveData<T>(string relativePath, T data, bool encrypted)
    {
        string path = Path.Combine(Application.persistentDataPath, relativePath);

        try
        {
            if (File.Exists(path))
            {
                File.Delete(path);
            }
            
            File.WriteAllText(path, JsonConvert.SerializeObject(data));
            return true;
        }
        catch (Exception e)
        {
            Debug.LogError($"Unable to save data: {e.Message} {e.StackTrace}");
            return false;
        }
    }
    
    public T LoadData<T>(string relativePath, bool encrypted){
        string path = Path.Combine(Application.persistentDataPath, relativePath);

        if (!File.Exists(path)){
            throw new FileNotFoundException($"{path} does not exist");
        }

        try{
            T data = JsonConvert.DeserializeObject<T>(File.ReadAllText(path));
            return data;
        }
        catch (Exception e){
            Debug.LogError($"Unable to load data due to: {e.Message} {e.StackTrace}");
            throw e;
        }
    }

    public void ClearData(string relativePath)
    {
        string path = Path.Combine(Application.persistentDataPath, relativePath);
        
        if (!File.Exists(path))
            return;
        
        File.Delete(path);
    }

    public bool DoesFileExist(String relativePath){
        return File.Exists(Path.Combine(Application.persistentDataPath, relativePath));
    }
}

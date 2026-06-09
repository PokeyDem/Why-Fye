using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnlockedLevelsData
{
    public List<bool> unlockedLevels;

    public UnlockedLevelsData(List<bool> unlockedLevels)
    {
        this.unlockedLevels = unlockedLevels;
    }
}

public class SaveData
{
    public UnlockedLevelsData unlockedLevels;

    public SaveData(UnlockedLevelsData unlockedLevelsData)
    {
        this.unlockedLevels =  unlockedLevelsData;
    }
}

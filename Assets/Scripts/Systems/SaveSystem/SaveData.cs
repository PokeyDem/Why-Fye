using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting;

[Preserve]
public class UnlockedLevelsData
{
    public List<bool> unlockedLevels;

    public UnlockedLevelsData(List<bool> unlockedLevels)
    {
        this.unlockedLevels = unlockedLevels;
    }
}

[Preserve]
public class SaveData
{
    public UnlockedLevelsData unlockedLevelsData;

    public SaveData(UnlockedLevelsData unlockedLevelsData)
    {
        this.unlockedLevelsData =  unlockedLevelsData;
    }
}

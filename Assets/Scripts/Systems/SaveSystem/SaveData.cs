using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting;

[UnityEngine.Scripting.Preserve]
public class UnlockedLevelsData
{
    public List<bool> unlockedLevels;

    public UnlockedLevelsData(List<bool> unlockedLevels)
    {
        this.unlockedLevels = unlockedLevels;
    }
}

[UnityEngine.Scripting.Preserve]
public class SaveData
{
    public UnlockedLevelsData unlockedLevels;

    public SaveData(UnlockedLevelsData unlockedLevelsData)
    {
        this.unlockedLevels =  unlockedLevelsData;
    }
}

using System.Collections.Generic;
using Managers;
using UnityEngine.Scripting;

namespace Systems.SaveSystem
{
    [Preserve]
    public class UnlockedLevelsData
    {
        public List<StageLevelsData> unlockedLevels;

        public UnlockedLevelsData(List<StageLevelsData> unlockedLevels)
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
}
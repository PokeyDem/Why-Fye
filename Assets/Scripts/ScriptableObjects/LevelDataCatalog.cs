using System.Collections.Generic;
using UnityEngine;

namespace ScriptableObjects
{
    [CreateAssetMenu(fileName = "LevelDataCatalog", menuName = "Level Data/Level Data Catalog")]
    public class LevelDataCatalog : ScriptableObject
    {
        public List<LevelData> levelsData =  new List<LevelData>(); 
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LevelDataCatalog", menuName = "Level Data/Level Data Catalog")]
public class LevelDataCatalog : ScriptableObject
{
    public List<LevelData> levelsData =  new List<LevelData>(); 
}

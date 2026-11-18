using UnityEngine;
using System;
using System.Collections.Generic;

// Serializable makes it possible for the class to be loaded into a JSON
[Serializable]
public class LevelData
{
    // stores level name as "1-1", "1-2" etc
    public string levelName;
    // true or false value if level if completed
    public bool completed;
}

[Serializable]
public class SaveData
{
    // list stores completed levels, cleared when data is reset
    public List<LevelData> levels = new();
}
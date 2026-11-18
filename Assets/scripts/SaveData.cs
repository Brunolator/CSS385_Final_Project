using UnityEngine;
using System;
using System.Collections.Generic;

[Serializable]
public class LevelData
{
    public string levelName;
    public bool completed;
}

[Serializable]
public class SaveData
{
    public List<LevelData> levels = new();
}
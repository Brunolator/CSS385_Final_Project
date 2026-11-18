using System.Collections.Generic;
using System.IO;
using UnityEngine;

public static class SaveManager
{
  static string SavePath => Path.Combine(Application.persistentDataPath, "save.json");
  static SaveData data;
  static Dictionary<string, LevelData> mapToLevel;

  public static void Load()
   {
        try
        {
            data = File.Exists(SavePath) ? JsonUtility.FromJson<SaveData>(File.ReadAllText(SavePath)) : new SaveData();
        } catch
        {
            data = new SaveData();
        }
        mapToLevel = new Dictionary<string, LevelData>();
        foreach (var l in data.levels)
        {
            mapToLevel[l.levelName] = l;
        }
    }
    static void EnsureLoaded(){
        if(data == null)
        {
            Load();
        }
    }
    
    public static void Save()
    {
        // store a temporary save file
        string tmp = SavePath + ".tmp";
        // write the temporary file
        File.WriteAllText(tmp, JsonUtility.ToJson(data, true));
        // if an existing file exists, overwrite the save
        if (File.Exists(SavePath))
        {
            File.Delete(SavePath);
        }
        File.Move(tmp, SavePath);
    }

    public static bool IsCompleted(string name)
    {
        EnsureLoaded();
        if(mapToLevel.TryGetValue(name, out var level))
        {
            return level.completed;
        }
        return false;
    }

    public static void CompleteLevel(string name)
    {
        EnsureLoaded();
        if(!mapToLevel.TryGetValue(name, out var level))
        {
            level = new LevelData{levelName = name};
            data.levels.Add(level);
            mapToLevel[name] = level;
        }
        level.completed = true;
        Save();
    }

    public static void ResetData()
    {
        data = new SaveData();
        mapToLevel = new Dictionary<string, LevelData>();
        Save();
    }


}
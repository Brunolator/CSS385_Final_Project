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
        // write save data to temporary file
        File.WriteAllText(tmp, JsonUtility.ToJson(data, true));
        // if an existing file exists, overwrite the save
        if (File.Exists(SavePath))
        {
            // delete existing save file
            File.Delete(SavePath);
        }
        // move temporary save file to regular save file path
        File.Move(tmp, SavePath);
    }

    // indicates if the level is completed
    public static bool IsCompleted(string name)
    {
        // ensure level is loaded to prevent corrupted states
        EnsureLoaded();
        // use TryGetValue to try and access key, if key exists in dictionary, export LevelData to level
        if(mapToLevel.TryGetValue(name, out var level))
        {
            // return the completion status of the level
            return level.completed;
        }
        // if TryGetValue() fails, return false
        return false;
    }

    public static void CompleteLevel(string name)
    {
         // ensure level is loaded to prevent corrupted states
        EnsureLoaded();
        // check if level does not already exist in the mapToLevel hashmap
        if(!mapToLevel.TryGetValue(name, out var level))
        {
            level = new LevelData{levelName = name};
            // add completed level to saved levels and dictionary for faster access
            data.levels.Add(level);
            mapToLevel[name] = level;
        }
        // toggle level as completed
        level.completed = true;
        // auto save after an action is completed
        Save();
    }

    public static void ResetData()
    {
        // clear save data
        data = new SaveData();
        // clear level data
        mapToLevel = new Dictionary<string, LevelData>();
        // auto save after an action is completed
        Save();
    }


}
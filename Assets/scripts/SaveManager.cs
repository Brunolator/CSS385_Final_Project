using System.Collections.Generic;
using System.IO;
using UnityEngine;

public static class SaveManager
{
    // get persistent data path and combine with save.json
  static string SavePath => Path.Combine(Application.persistentDataPath, "save.json");
  // keep track of save data object
  static SaveData data;
  // keep track of levels by level name
  static Dictionary<string, LevelData> mapToLevel;

  public static void Load()
   {
    // ensure there is always a savedata object that exists
        try
        {
            // see if save data file exists, if yes, load it in, else make a new save data object
            data = File.Exists(SavePath) ? JsonUtility.FromJson<SaveData>(File.ReadAllText(SavePath)) : new SaveData();
        } catch
        {
            // if attempting to load in fails, make a new save data object instead
            data = new SaveData();
        }
        // fill dictionary with level names and level data
        mapToLevel = new Dictionary<string, LevelData>();
        foreach (var l in data.levels)
        {
            mapToLevel[l.levelName] = l;
        }
    }
    // prevents an attempt to write to an undefined SaveData object
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
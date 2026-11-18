using UnityEngine;

// saves the overworld level status
public class LevelNode : MonoBehaviour
{
    // level name format is 1-1, 1-2 etc.
    // city levels are 1, mountain is 2, forest is 3, desert is 4, islands are 5, volcano is 6
    public string levelName;
    // notClearFlag is Red
    public GameObject notClearFlag;
    // clearFlag is blue
    public GameObject clearFlag;

    // get the current completion status from the level
    public bool isCompleted => SaveManager.IsCompleted(levelName);

    //TODO: add prerequisite management for levels
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Refresh();
    }

    // Unity system function, detects click input in collision area
    // Toggles level as complete and updates flag sprites
    void OnMouseDown()
    {
        // skip if level is already completed
         if (isCompleted)
        {
            return;
        }
        // toggle level as completed
        SaveManager.CompleteLevel(levelName);
        Refresh();
    }

    // updates flag colors
    public void Refresh()
    {
        notClearFlag.SetActive(!isCompleted);
        clearFlag.SetActive(isCompleted);
    }
}

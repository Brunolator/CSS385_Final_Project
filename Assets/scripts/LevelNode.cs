using UnityEngine;
using UnityEngine.SceneManagement;

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

    public GameObject lockedFlag;
    public LevelNode prereq;

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
            Refresh();
        }
        if(prereq != null)
        {
            Debug.Log(prereq.levelName);
        }
        if ((levelName == "1-1")|| ((prereq != null) && SaveManager.IsCompleted(prereq.levelName)))
        {
             // Load the scene if it exists
            SceneManager.LoadScene("Level" + levelName);
        }
       
    }

    // updates flag colors
    public void Refresh()
    {
        if ((prereq == null && levelName != "1-1")|| (prereq != null && !SaveManager.IsCompleted(prereq.levelName)))
        {
            lockedFlag.SetActive(true);
            notClearFlag.SetActive(false);
            clearFlag.SetActive(false);
        } else if (!isCompleted)
        {
            lockedFlag.SetActive(false);
            notClearFlag.SetActive(true);
            clearFlag.SetActive(false);
        } else
        {
            lockedFlag.SetActive(false);
            notClearFlag.SetActive(false);
            clearFlag.SetActive(true);
        }
        
    }
}

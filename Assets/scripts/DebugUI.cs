using UnityEngine;

// script for debugging save state
public class DebugUI : MonoBehaviour
{
    // reset button functionality
    public void resetSave()
    {
        // clear save data
        SaveManager.ResetData();
        // update all levels to make sure they reflect accurate state
        foreach(var level in FindObjectsByType<LevelNode>())
        {
            level.Refresh();
        }
    }
}

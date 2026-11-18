using UnityEngine;

// script for debugging save state
public class DebugUI : MonoBehaviour
{
    // reset button functionality
    public void resetSave()
    {
        SaveManager.ResetData();
        foreach(var level in FindObjectsByType<LevelNode>())
        {
            level.Refresh();
        }
    }
}

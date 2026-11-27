using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    Simulated[] simulatedObjects;
    // win text
    public GameObject winText;
    public GameObject loseText;
    public static bool isRunning { get; private set; }

    public string levelName;

    void Start()
    {
        // Get a list of all objects that are part of the simulation
        simulatedObjects = Object.FindObjectsByType<Simulated>();
        StopSimulation();
        isRunning = false;
    }

    // Start playing the level
    public void StartSimulation()
    {
        for (int i = 0; i < simulatedObjects.Length; i++)
        {
            simulatedObjects[i].StartSimulation();
        }
        isRunning = true;
        
    }

    // Stop playing the level
    public void StopSimulation()
    {
        for (int i = 0; i < simulatedObjects.Length; i++)
        {
            simulatedObjects[i].StopSimulation();
        }
        // hide win text
        if (winText != null)
        {
            winText.SetActive(false);
        }
        isRunning = false;
        if(loseText != null)
        {
            loseText.SetActive(false);
        }
    }

    // Switch to the main menu
    public void ExitToMenu()
    {
        SceneManager.LoadScene("LevelSelect");
    }

    // Save the current level state to hard disc
    /*public void saveLevel()
    {
        // TODO
    }*/

    // To be called on win condition
    public void OnGoalMet()
    {
        // Tell objects that the goal was met
        for (int i = 0; i < simulatedObjects.Length; i++)
        {
            simulatedObjects[i].OnLevelGoalMet();
        }

        // display win text
        if (winText != null)
        {
            winText.SetActive(true);
        }
        if (levelName != null)
        {
            SaveManager.CompleteLevel(levelName);
        }
    }

    public void OnGoalFailed()
    {

        if (!isRunning)
        {
            return;
        } 
        // display win text
        if (loseText != null)
        {
            loseText.SetActive(true);
        }
        Physics2D.simulationMode = SimulationMode2D.Script;
    }
}

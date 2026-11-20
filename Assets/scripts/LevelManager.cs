using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    Simulated[] simulatedObjects;
    // win text
    public GameObject winText;

    void Start()
    {
        // Get a list of all objects that are part of the simulation
        simulatedObjects = Object.FindObjectsByType<Simulated>();
        StopSimulation();
    }

    // Start playing the level
    public void StartSimulation()
    {
        for (int i = 0; i < simulatedObjects.Length; i++)
        {
            simulatedObjects[i].StartSimulation();
        }
        Physics2D.simulationMode = SimulationMode2D.FixedUpdate;
    }

    // Stop playing the level
    public void StopSimulation()
    {
        for (int i = 0; i < simulatedObjects.Length; i++)
        {
            simulatedObjects[i].StopSimulation();
        }
        Physics2D.simulationMode = SimulationMode2D.Script;
        // hide win text
        if (winText != null)
        {
            winText.SetActive(false);
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
        Debug.Log("you win");
        // display win text
        if (winText != null)
        {
            winText.SetActive(true);
        }
    }
}

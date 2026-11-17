using UnityEngine;

public class LevelManager : MonoBehaviour
{
    Simulated[] simulatedObjects;

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
    }

    // Save the current level state to hard disc
    /*public void saveLevel()
    {
        // TODO
    }*/

    // To be called on win condition
    public void onCharacterReachGoal()
    {
        Debug.Log("you win");
    }
}

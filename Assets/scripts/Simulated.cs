// For objects that have behavior when the level is simulated

using UnityEngine;

public class Simulated : MonoBehaviour
{
    protected virtual void Awake() { }

    // Save initial object state and start simulating
    public virtual void StartSimulation() { }

    // Returned to object state before the simulation began
    public virtual void StopSimulation() { }

    // Called whenever the level's goal is met
    public virtual void OnLevelGoalMet() { }
}

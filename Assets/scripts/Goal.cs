using UnityEngine;

public class Goal : Simulated
{
    public LevelManager lvlManager;
    bool goalReached;

    // if goal is reached or colliding object isn't a ball, don't check for collision
    void OnTriggerEnter2D(Collider2D other)
    {
        if(goalReached) return;
        if(!other.TryGetComponent(out SimulatedBall ball)) return;

        goalReached = true;
        lvlManager.onCharacterReachGoal();
    }

    // reset all instances of the goal being reached
    public override void StartSimulation()
    {
        goalReached = false;
    }

    public override void StopSimulation()
    {
        goalReached = false;
    }
}

using UnityEngine;

public class Goal : Simulated
{
    public LevelManager lvlManager;
    bool goalReached;

    void OnTriggerEnter2D(Collider2D other)
    {
        // if goal is reached or colliding object isn't a goal collider, don't check for collision
        if (goalReached || !other.CompareTag("GoalCollider")) return;

        // Goal is met
        goalReached = true;
        lvlManager.OnGoalMet();
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

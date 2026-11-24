// For objects that move in the level simulation

using UnityEngine;

public class Simulated : MonoBehaviour
{
    protected Rigidbody2D body;
    protected Vector2 startPosition;

    void Awake()
    {
        // get object components and starting position
        body = GetComponent<Rigidbody2D>();
        startPosition = transform.position;
    }

    // Save initial object state and start simulating physics
    public virtual void StartSimulation()
    {
        // set object physics to moveable
        startPosition = transform.position;
        body.bodyType = RigidbodyType2D.Dynamic;
    }

    // Stop simulating physics and return to saved object state
    public virtual void StopSimulation()
    {
        // reset ball physics and position
        body.bodyType = RigidbodyType2D.Kinematic;
        body.linearVelocity = Vector2.zero;
        body.angularVelocity = 0f;
        body.position = startPosition;
        transform.position = startPosition;
    }

    // Called whenever the level's goal is met
    public virtual void OnLevelGoalMet()
    {

    }
}

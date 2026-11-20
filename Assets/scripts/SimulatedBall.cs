using UnityEngine;

// inherits from simulated
public class SimulatedBall : Simulated
{
    Rigidbody2D body;

    Vector2 startPosition;

    void Awake()
    {
        // get ball components and starting position
        body = GetComponent<Rigidbody2D>();
        startPosition = transform.position;
    }

    public override void StartSimulation()
    {
       // set ball physics to moveable
        startPosition = transform.position;
        body.bodyType = RigidbodyType2D.Dynamic;
         
    }

    public override void StopSimulation()
    {
       // reset ball physics and position
        body.bodyType = RigidbodyType2D.Kinematic;
        body.linearVelocity = Vector2.zero;
        body.angularVelocity = 0f;
        body.position = startPosition;
        transform.position = startPosition;
        
    }
}


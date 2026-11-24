// For objects that walk when touching the ground, like characters

using UnityEngine;

public class Walking : Simulated
{
    public float speed;
    public float collisionBoxY;
    public Vector2 collisionSize;
    public LayerMask groundLayer;
    public string stopWalkingIfCollidesWithTag;
    private Rigidbody2D rb;
    private LevelManager levelManager;
    private float maxSpeedToWalk; // Max speed for which walking force should be applied
    private float speedModifier = 8.5f; // Controls both horsepower and direction
    private bool isCurrentlyWalking = true;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        maxSpeedToWalk = speed;
        rb = GetComponent<Rigidbody2D>();
        levelManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<LevelManager>();
        rb.freezeRotation = true;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (Physics2D.simulationMode == SimulationMode2D.Script)
        {
            // Do nothing if not in simulation mode
            return;
        }

        // Check if the velocity isn't too high and it is on the ground
        if (isCurrentlyWalking && rb.linearVelocity.magnitude <= maxSpeedToWalk && !isFalling())
        {
            rb.AddForce(new Vector2(speed * speedModifier, 0));
        }
    }

    public override void StartSimulation()
    {
        base.StartSimulation();
    }

    public override void StopSimulation()
    {
        base.StopSimulation();
    }

    public override void OnLevelGoalMet()
    {
        base.OnLevelGoalMet();
        isCurrentlyWalking = false;
    }

    public bool isFalling()
    {
        return !Physics2D.BoxCast(transform.position, collisionSize, 0, -transform.up, collisionBoxY, groundLayer);
    }

    // Draw the ground collider
    // https://www.youtube.com/watch?v=P_6W-36QfLA&t=26s
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position - transform.up * collisionBoxY, collisionSize);
    }
}

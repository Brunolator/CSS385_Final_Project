using UnityEngine;

public class WalkingCharacter : MonoBehaviour
{
    public float speed;
    public float collisionBoxY;
    public Vector2 collisionSize;
    public LayerMask groundLayer;
    private float maxSpeedToWalk; // Max speed for which walking force should be applied
    private float speedModifier = 5; // Controls both horsepower and direction

    private Rigidbody2D rb;
    private Collider2D collider;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        collider = GetComponent<Collider2D>();
        rb.freezeRotation = true;
        maxSpeedToWalk = speed;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // Check if the velocity isn't too high and it is on the ground
        if (rb.linearVelocity.magnitude <= maxSpeedToWalk && !isFalling())
        {
            rb.AddForce(new Vector2(speed * speedModifier, 0));
        }
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

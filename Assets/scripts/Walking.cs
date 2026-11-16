using UnityEngine;

public class WalkingCharacter : MonoBehaviour
{
    public float speed;
    private float maxSpeedToWalk; // Max speed for which walking force should be applied
    private float speedModifier = 1;
    private float groundCheckDistance = 0.1f;
    private bool falling = true;

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
        if (rb.linearVelocity.magnitude <= maxSpeedToWalk && !falling)
        {
            rb.AddForce(new Vector2(speed * speedModifier, 0));
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.GetContact(0).normal == Vector2.up)
        {
            falling = false;
        }
    }

    private void OnCollisionExit2D(Collision2D other)
    {
        falling = true;
    }
}

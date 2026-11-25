using UnityEngine;

public class Spring : MonoBehaviour
{
    public float launchSpd = 12f;

    public float squash = 0.4f;
    public float stretch = 0.25f;
    public float squashTime = 0.05f;
    public float stretchTime = 0.08f;
    public float settleTime = 0.1f;

    Vector3 baseScale;
    float timer = -1f;

    void Awake()
    {
        baseScale = transform.localScale;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Rigidbody2D otherBody = collision.rigidbody;
        if (otherBody == null)
        {
            return;
        }
        if (otherBody.bodyType != RigidbodyType2D.Dynamic)
        {
            return;
        }
        Vector2 up = transform.up;
        Vector2 velocity = otherBody.linearVelocity;

        Vector2 sideMovement = velocity - Vector2.Dot(velocity, up) * up;
        otherBody.linearVelocity = sideMovement + up * launchSpd;

        timer = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        if (timer < 0f)
        {
            return;
        }

        timer += Time.deltaTime;

        float offset;

        if (timer < squashTime)
        {
            offset = Mathf.Lerp(0f, -squash, timer / squashTime);
        }
        else if (timer < squashTime + stretchTime)
        {
            offset = Mathf.Lerp(-squash, stretch, (timer - squashTime) / stretchTime);
        }
        else if (timer < squashTime + stretchTime + settleTime)
        {
            offset = Mathf.Lerp(stretch, 0f, (timer - squashTime - stretchTime) / stretchTime);
        }
        else
        {
            offset = 0f;
            timer = -1f;
        }

        transform.localScale = new Vector3(baseScale.x * (1f - offset * 0.5f), baseScale.y * (1f + offset), baseScale.z);
    }
}

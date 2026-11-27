using UnityEngine;

public class Dynamite : MonoBehaviour
{

    public float blastSpeed = 14f;
    public float upward = 0.5f;
    public float effectLifetime = 2f;
    public GameObject explosionEffect;
    SpriteRenderer sprite;
    Collider2D trigger;
    bool exploded;

    void Awake()
    {
        sprite = GetComponent<SpriteRenderer>();
        trigger = GetComponent<Collider2D>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        Rigidbody2D body = other.attachedRigidbody;
        if(body == null)
        {
            return;
        }
        if(body.bodyType != RigidbodyType2D.Dynamic) return;

        Vector2 away = (body.position - (Vector2)transform.position).normalized;
        Vector2 direction = (away + Vector2.up * upward).normalized;
        body.linearVelocity = direction * blastSpeed;

        if(explosionEffect != null)
        {
            GameObject effect = Instantiate(explosionEffect, transform.position, Quaternion.identity);
            Destroy(effect, effectLifetime);
        }
    }

    
}

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

    // Update is called once per frame
    void Update()
    {
        
    }
}

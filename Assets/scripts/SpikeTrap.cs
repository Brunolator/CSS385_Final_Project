using UnityEngine;

public class SpikeTrap : MonoBehaviour
{
    public LevelManager lvlManager;
    bool triggered;
    
    void Awake()
    {
        
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (triggered)
        {
            return;
        }
        if(other.GetComponentInParent<Walking>() == null)
        {
            return;
        }
        triggered = true;
        lvlManager.OnGoalFailed();
    }

    // Update is called once per frame
    void Update()
    {
        if(triggered && !LevelManager.isRunning)
        {
            triggered = false;
        }
    }
}

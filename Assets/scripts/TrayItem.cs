using UnityEngine;
using UnityEngine.UI;

public class TrayItem : MonoBehaviour
{

    public GameObject boardSpawn;
    public Transform spawnPoint;
    public int max = 3; 
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    int remaining;
    Button button;
    
    void Awake()
    {
        // initialize button
        button = GetComponent<Button>();
        // initialize the amount you can have from the max
        remaining = max;
        // make sure button spawns in board when clicked
        button.onClick.AddListener(Spawn);
        // make sure button is interactable
        button.interactable = remaining > 0;
    }

    // Update is called once per frame
    void Spawn()
    {
        // skip if spawn limit has been reached
        if (remaining <= 0)
        {
            return;
        }
        // create new board of boardSpawn in the center 
        GameObject board = Instantiate(boardSpawn, spawnPoint.position, spawnPoint.rotation);
        // reduce the amount of boards you can work with
        remaining--;
        // update display
        button.interactable = remaining > 0;
    }
}

using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class StartButton : MonoBehaviour
{
    public LevelManager lvlManager;
    public TMP_Text buttonLabel;
    Button button;
    bool running;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        // set up button to use for state transitions
        button = GetComponent<Button>();
        button.onClick.AddListener(StateTransition);
    }

    void StateTransition()
    {
        // switch from running to stopped
        if (running)
        {
            lvlManager.StopSimulation();
            running = false;
            if (buttonLabel != null)
            {
                buttonLabel.text = "Start";
            }
        }
        else
        {
            // switch from stopped to running
            lvlManager.StartSimulation();
            running = true;
            if (buttonLabel != null)
            {
                buttonLabel.text = "Reset";
            }
        }

    }
}

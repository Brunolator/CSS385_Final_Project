// Lets player rotate objects in the level using arrow keys
using UnityEngine;
using UnityEngine.InputSystem;

public class Rotatable : Draggable
{
    Rigidbody2D rigidbody;
    public float tiltSpd = 90; // speed of rotation
    public float maxTilt = 45; // steepest allowed angle
    float angle; // current angle

    protected override void Awake()
    {
        base.Awake();

        rigidbody = GetComponent<Rigidbody2D>();
        angle = rigidbody.rotation;
    }

    protected override void Update()
    {
        base.Update();

        // do nothing if simulation is running
        if (LevelManager.isRunning)
        {
            if (this == currentlySelected) ClearSelected();
            return;
        }

        if (this == currentlySelected)
        {
            Rotate();
        }
    }

    // rotate based on key input
    private void Rotate()
    {
        // Cannot interfere with physics rotations if the level is running
        if (LevelManager.isRunning)
        {
            return;
        }

        // rotate object
        Keyboard keys = Keyboard.current;
        // check for left or right tilt
        float input = (keys.leftArrowKey.isPressed ? 1f : 0f) - (keys.rightArrowKey.isPressed ? 1f : 0f);
        // set the angle, restricted based on maxTilt
        angle = Mathf.Clamp(angle + input * tiltSpd * Time.deltaTime, -maxTilt, maxTilt);
        transform.rotation = Quaternion.Euler(0, 0, angle);
    }
}

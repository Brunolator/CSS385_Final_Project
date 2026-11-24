using UnityEngine;
using UnityEngine.InputSystem;
public class Board : Draggable
{
    public float tiltSpd = 90;
    public float maxTilt = 45;

    // use to make boards movable
    Rigidbody2D rigidbody;
    // board image
    SpriteRenderer board;
    // board color (should be white)
    Color defaultColor = Color.white;
    public Color selectColor = Color.yellow;
    // current angle of the board
    float angle;
    protected override void Awake()
    {
        base.Awake();

        rigidbody = GetComponent<Rigidbody2D>();
        board = GetComponent<SpriteRenderer>();
        if (board != null)
        {
            defaultColor = board.color;
        }
        angle = rigidbody.rotation;

    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();

        // rotate if I am selected
        if (selected)
        {
            Rotate();
        }
    }

    // rotate based on key input
    private void Rotate()
    {
        // rotate object
        Keyboard keys = Keyboard.current;
        // check for left or right tilt
        float input = (keys.leftArrowKey.isPressed ? 1f : 0f) - (keys.rightArrowKey.isPressed ? 1f : 0f);
        // set the angle, restricted based on maxTilt
        angle = Mathf.Clamp(angle + input * tiltSpd * Time.deltaTime, -maxTilt, maxTilt);
        transform.rotation = Quaternion.Euler(0, 0, angle);
    }

    // select or deselect board
    protected override void SetSelected(bool value)
    {
        base.SetSelected(value);

        // set board to yellow to indicate it's being used
        if (board != null)
        {
            board.color = selected ? selectColor : defaultColor;
        }
    }
}

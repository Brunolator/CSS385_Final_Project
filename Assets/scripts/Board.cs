using UnityEngine;
using UnityEngine.InputSystem;
public class Board : MonoBehaviour
{
    public float tiltSpd = 90;
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    public Color selectColor = Color.yellow;

    // keep track of the current board
    static Board current;
    // use to make boards movable
    Rigidbody2D rigidbody;
    // collider object, use to detect clicks
    Collider2D collider;
    // board image
    SpriteRenderer board;
    // board color (should be white)
    Color boardColor = Color.white;
    // Use to keep track of where the mouse is
    Camera cam;
    // current angle of the board (-45 to 45)
    float angle;
    // is the current board selected
    bool selected;
    // is the current board being dragged
    bool dragging;
    // vector coordinates of drag
    Vector3 drag;
    // initialize all components, use Awake because Start breaks spawning system
    void Awake()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        collider = GetComponent<Collider2D>();
        board = GetComponent<SpriteRenderer>();
        cam = Camera.main;
        // initialize board color
        if(board != null)
        {
            boardColor = board.color; 
        }
        angle = rigidbody.rotation;
        
    }

    // Update is called once per frame
    void Update()
    {
        // lock down board if simulation is running
        if (LevelManager.isRunning)
        {
            if(selected) SetSelected(false);
            dragging = false;
            return;
        }

        // get mouse coordinates
        Mouse mouse = Mouse.current;
        Vector3 coord = getMouseCoordinates();
        // if mouse was just clicked
        if (Mouse.current.leftButton.wasPressedThisFrame){
            // select object that mouse is on
            Physics2D.SyncTransforms();
            Collider2D hit = Physics2D.OverlapPoint(coord);
            // set object as true and dragging
            if (hit == collider){
                SetSelected(true);
                dragging = true;
                drag = transform.position - coord;
             } 
             // otherwise set it as false
            else
            {
                SetSelected(false);
            }
        }
        
       
        // if dragging is active and left click is being held, change board positions
        if(dragging && mouse.leftButton.isPressed)
        {
            transform.position = coord + drag;
        }

        // if mouse button was released
        if (mouse.leftButton.wasReleasedThisFrame)
        {
            dragging = false;
        }

        // skip over rotation
        if (!selected)
        {
            return;
        }
        // rotate object
        Keyboard keys = Keyboard.current;
        // check for left or right tilt
        float input  = (keys.leftArrowKey.isPressed ? 1f : 0f) - (keys.rightArrowKey.isPressed ? 1f : 0f);
        // restrict to 45 degree angles
        angle = Mathf.Clamp(angle + input * tiltSpd * Time.deltaTime, -45f, 45f);
        transform.rotation = Quaternion.Euler(0, 0, angle); 
    }

    // select or deselect board
    public void SetSelected(bool value)
    {
        // if selected
        if (value)
        {
            // if this is not the current object, deselect
            if (current != null && current != this)
            {
                current.SetSelected(false);
            }
            current = this;
        // if this is the current object, disable it as the current object
        } else if (current == this)
        {
            current = null;
        }
        // set current as selected 
        selected = value;
        // set board to yellow to indicate it's being used
        if(board != null)
        {
            board.color = selected? selectColor : boardColor;
        }
    }
    
    Vector3 getMouseCoordinates()
    {
        Vector3 coord = cam.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        // prevents vector from going off coordinates
        coord.z = 0;
        return coord;
    }
}

using UnityEngine;
using UnityEngine.InputSystem;
public class Board : MonoBehaviour
{
    public float tiltSpd = 90;
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    public Color selectColor = Color.yellow;

    // keep track of the current board
    static Board current;

    Rigidbody2D rigidbody;
    Collider2D collider;
    SpriteRenderer board;
    Color boardColor = Color.white;
    Camera cam;
    float angle;
    bool selected;
    void Awake()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        collider = GetComponent<Collider2D>();
        board = GetComponent<SpriteRenderer>();
        cam = Camera.main;
        if(board != null)
        {
            boardColor = board.color; 
        }
        angle = rigidbody.rotation;
        
    }

    void OnMouseDown()
    {
        SetSelected(true);
    }

    // Update is called once per frame
    void Update()
    {
        if(selected && Mouse.current.leftButton.wasPressedThisFrame)
        {
            Vector2 world = cam.ScreenToWorldPoint(Mouse.current.position.ReadValue());
            // unselect if click when not over board
            if (!collider.OverlapPoint(world))
            {
                SetSelected(false);
            }
        }
        // skip over rotation
        if (!selected)
        {
            return;
        }
        Keyboard keys = Keyboard.current;
        float input  = (keys.leftArrowKey.isPressed ? 1f : 0f) - (keys.rightArrowKey.isPressed ? 1f : 0f);
        angle = Mathf.Clamp(angle + input * tiltSpd * Time.deltaTime, -45f, 45f);
    }

    void FixedUpdate()
    {
        transform.rotation = Quaternion.Euler(0, 0, angle); 
    }

    public void SetSelected(bool value)
    {
        if (value)
        {
            if (current != null && current != this)
            {
                current.SetSelected(false);
            }
            current = this;
        } else if (current == this)
        {
            current = null;
        }
        selected = value;
        if(board != null)
        {
            board.color = selected? selectColor : boardColor;
        }
    }
    
}

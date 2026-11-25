// Lets players drag around objects in the level
using UnityEngine;
using UnityEngine.InputSystem;

public class Draggable : MonoBehaviour
{
    SpriteRenderer spriteRenderer;
    Color defaultColor = Color.white;
    public Color selectColor = Color.yellow;

    // Universal selected object
    static Draggable currentlySelected;

    // Specific object variables
    Collider2D collider;
    Camera cam;

    // current state
    protected bool selected;
    bool dragging;
    Vector3 drag;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected virtual void Awake()
    {
        collider = GetComponent<Collider2D>();
        cam = Camera.main;

        // Set sprite renderer and color
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer != null)
        {
            defaultColor = spriteRenderer.color;
        }
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        // do not drag if simulation is running
        if (LevelManager.isRunning)
        {
            if (selected) SetSelected(false);
            dragging = false;
            return;
        }

        // get mouse coordinates
        Mouse mouse = Mouse.current;
        Vector3 coord = GetMouseCoordinates();
        // if mouse was just clicked
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            // select object that mouse is on
            Physics2D.SyncTransforms();
            Collider2D hit = Physics2D.OverlapPoint(coord);
            // Get whether I am selected
            SetSelected(hit == collider);
            // If I am selected, begin dragging
            if (selected)
            {
                dragging = true;
                drag = transform.position - coord;
            }
        }

        // if dragging is active and left click is being held, change board positions
        if (dragging && mouse.leftButton.isPressed)
        {
            transform.position = coord + drag;
        }

        // if mouse button was released
        if (mouse.leftButton.wasReleasedThisFrame)
        {
            dragging = false;
        }
    }

    // select or deselect board
    protected virtual void SetSelected(bool value)
    {
        // set current as selected 
        selected = value;

        // color board to indicate it's being used
        if (spriteRenderer != null)
        {
            spriteRenderer.color = selected ? selectColor : defaultColor;
        }

        // if setting selected
        if (value)
        {
            // if this is not the current object, deselect
            if (currentlySelected != null && currentlySelected != this)
            {
                currentlySelected.SetSelected(false);
            }
            currentlySelected = this;
        }

        // if setting unselected, unselect me
        else if (currentlySelected == this)
        {
            currentlySelected = null;
        }
    }

    protected Vector3 GetMouseCoordinates()
    {
        Vector3 coord = cam.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        // prevents vector from going off coordinates
        coord.z = 0;
        return coord;
    }
}

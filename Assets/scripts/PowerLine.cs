using NUnit.Framework;
using UnityEngine;
using UnityEngine.InputSystem;

public class PowerLine : MonoBehaviour
{
    GameObject anchor1;
    GameObject anchor2;

    private const float defaultRopeLength = 4;
    private const float maxRopeLength = 6;
    private float ropeLength;
    bool needsToUpdatePowerLinePosition;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        Transform anchor1Transform = transform.Find("Anchor1");
        Transform anchor2Transform = transform.Find("Anchor2");
        Assert.IsNotNull(anchor1Transform, "Power line is missing anchor 1");
        Assert.IsNotNull(anchor2Transform, "Power line is missing anchor 2");
        anchor1 = anchor1Transform.gameObject;
        anchor2 = anchor2Transform.gameObject;
        ropeLength = defaultRopeLength;
        needsToUpdatePowerLinePosition = true;

        // TODO: create a child of Draggable where the 2 anchors can only drag up to maxRopeLength distance from each other
    }

    void Update()
    {
        // Cannot interfere if level is running
        if (LevelManager.isRunning)
        {
            return;
        }

        // If selected, use keyboard input to change the slack of the rope
        if (Selected())
        {
            ChangeRopeLengthOnInput();
        }

        if (needsToUpdatePowerLinePosition && !Dragging())
        {
            UpdatePowerLinePosition();
        }

        if (Dragging())
        {
            needsToUpdatePowerLinePosition = true;
        }
    }

    // Update the links of the power line based on 
    private void UpdatePowerLinePosition()
    {
        needsToUpdatePowerLinePosition = false;
        ropeLength = Mathf.Max(ropeLength, DistanceBetweenAnchors());

        // TODO: create the rope links
    }

    // Use keyboard input to change the slack in the rope
    private void ChangeRopeLengthOnInput()
    {
        Keyboard keys = Keyboard.current;
        float input = (keys.upArrowKey.isPressed ? 0.005f : 0f) - (keys.downArrowKey.isPressed ? 0.005f : 0f);

        // Update rope length if longer than distance between anchors, but shorter than max rope length
        ropeLength = Mathf.Max(Mathf.Min(ropeLength + input, maxRopeLength), DistanceBetweenAnchors());
    }

    private float DistanceBetweenAnchors()
    {
        float dx = anchor1.transform.position.x - anchor2.transform.position.x;
        float dy = anchor1.transform.position.y - anchor2.transform.position.y;
        return Mathf.Sqrt(dx * dx + dy * dy);
    }

    private bool Selected()
    {
        return anchor1.GetComponent<Draggable>().IsSelected() || anchor2.GetComponent<Draggable>().IsSelected();
    }

    private bool Dragging()
    {
        return anchor1.GetComponent<Draggable>().IsBeingDragged() || anchor2.GetComponent<Draggable>().IsBeingDragged(); ;
    }
}

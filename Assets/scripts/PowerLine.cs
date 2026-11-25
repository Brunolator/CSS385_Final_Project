using NUnit.Framework;
using UnityEngine;
using UnityEngine.InputSystem;

public class PowerLine : MonoBehaviour
{
    GameObject anchor1;
    PairDraggable anchor1Draggable;
    GameObject anchor2;
    PairDraggable anchor2Draggable;

    private const float defaultRopeLength = 4;
    private const float maxRopeLength = 6;
    private float ropeLength;
    bool needsToUpdatePowerLinePosition;

    void Awake()
    {
        // Get the 2 anchors
        Transform anchor1Transform = transform.Find("Anchor1");
        Transform anchor2Transform = transform.Find("Anchor2");
        Assert.IsNotNull(anchor1Transform, "Power line is missing anchor 1");
        Assert.IsNotNull(anchor2Transform, "Power line is missing anchor 2");
        anchor1 = anchor1Transform.gameObject;
        anchor2 = anchor2Transform.gameObject;

        // Get the PairDraggable script component from the 2 anchors
        anchor1Draggable = anchor1.GetComponent<PairDraggable>();
        anchor2Draggable = anchor2.GetComponent<PairDraggable>();
        Assert.IsNotNull(anchor1Draggable, "Power line anchor 1 is missing the PairDraggable script");
        Assert.IsNotNull(anchor2Draggable, "Power line anchor 2 is missing the PairDraggable script");
        anchor1Draggable.SetMaxDistance(maxRopeLength);
        anchor2Draggable.SetMaxDistance(maxRopeLength);

        ropeLength = DistanceBetweenAnchors();
        needsToUpdatePowerLinePosition = true;
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

        // Update position once at the end of each drag
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
        return anchor1Draggable.DistanceFromOther();
    }

    private bool Selected()
    {
        return anchor1Draggable.IsSelected() || anchor2Draggable.IsSelected();
    }

    private bool Dragging()
    {
        return anchor1Draggable.IsBeingDragged() || anchor2Draggable.IsBeingDragged(); ;
    }
}

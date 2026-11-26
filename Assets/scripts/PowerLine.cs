using NUnit.Framework;
using UnityEngine;

public class PowerLine : Simulated
{
    GameObject anchor1;
    PairDraggable anchor1Draggable;
    GameObject anchor2;
    PairDraggable anchor2Draggable;

    private const float maxRopeLength = 10;
    bool needsToUpdatePowerLinePosition;
    Rope rope;

    protected override void Awake()
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

        needsToUpdatePowerLinePosition = true;

        // Create the rope
        Rigidbody2D anchor1RB = anchor1.GetComponent<Rigidbody2D>();
        Rigidbody2D anchor2RB = anchor2.GetComponent<Rigidbody2D>();
        Assert.IsNotNull(anchor1RB, "Power line anchor 1 is missing a Rigidbody2D");
        Assert.IsNotNull(anchor2RB, "Power line anchor 2 is missing a Rigidbody2D");
        rope = gameObject.AddComponent<Rope>();
        rope.SetMaxRopeLength(maxRopeLength);
        rope.SetAnchors(anchor1RB, anchor2RB);
        rope.RecalculateRopePoints();
    }

    void FixedUpdate()
    {
        // Cannot interfere if level is running
        if (LevelManager.isRunning)
        {
            return;
        }

        // If selected, use keyboard input to change the slack of the rope
        if (Selected())
        {
            rope.UpdateRopeOnInput();
            needsToUpdatePowerLinePosition = true;
        }

        // Update position once at the end of each drag
        if (needsToUpdatePowerLinePosition && !Dragging())
        {
            needsToUpdatePowerLinePosition = false;
            rope.RecalculateRopePoints();
        }
    }

    public override void StartSimulation()
    {
        rope.GenerateRope();
    }

    public override void StopSimulation()
    {
        rope.RecalculateRopePoints(); // Reset rope position
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

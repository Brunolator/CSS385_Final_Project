using NUnit.Framework;
using UnityEngine;

public class AnchorPoint : PhysicsSimulated
{
    Collider2D myCollider;
    Collider2D attachedTo;
    HingeJoint2D attachmentJoint;
    Draggable myDraggable;

    protected override void Awake()
    {
        base.Awake();

        myCollider = GetComponent<Collider2D>();

        // Get a draggable component if I have one
        if (TryGetComponent<Draggable>(out Draggable draggable))
        {
            myDraggable = draggable;
        }
    }

    public override void StartSimulation()
    {
        base.StartSimulation();
        CheckAttachment();
    }

    // Attach myself to the given collider
    private void Attach(Collider2D toAttachTo)
    {
        Assert.NotNull(toAttachTo);
        attachedTo = toAttachTo;
        Detatch();

        // Make a HingeJoint2D for the attachment
        attachmentJoint = gameObject.AddComponent<HingeJoint2D>();
        attachmentJoint.autoConfigureConnectedAnchor = false;
        attachmentJoint.connectedBody = toAttachTo.attachedRigidbody;
        attachmentJoint.anchor = Vector2.zero;
        attachmentJoint.connectedAnchor = toAttachTo.transform.InverseTransformPoint(body.position);
    }

    // If I am attached to something, detatch
    private void Detatch()
    {
        Destroy(attachmentJoint);
        attachedTo = null;
    }

    // Checks if the anchor is still overlapping with its attachment
    // If not, destroy connection and try to find a new attachment
    private void CheckAttachment()
    {
        bool stillAttached = attachedTo != null && attachedTo.OverlapPoint(body.position);
        if (!stillAttached)
        {
            Detatch();
            TryAttachAnchor();
        }
    }

    // Attempts to find an overlapping object to attach the anchor to, prioritizing physics simulated obejcts
    private void TryAttachAnchor()
    {
        Collider2D[] hits = Physics2D.OverlapPointAll(body.position);

        // If there is only 1 hit then its just the anchor itself
        if (hits.Length <= 1)
        {
            return;
        }

        // Look for objects to attach to, prioritizing PhysicsSimulateds
        for (int i = 0; i < hits.Length; i++)
        {
            // If the collider is a PhysicsSimulated, choose that one as selected
            if (hits[i].TryGetComponent<PhysicsSimulated>(out PhysicsSimulated otherPhysicsObject))
            {
                if (otherPhysicsObject == this)
                {
                    continue; // Cannot attach to self
                }
                Attach(hits[i]);
                return;
            }
        }

        // If no attachments are physics but there are other ones, choose one arbitrarily
        for (int i = 0; i < hits.Length; i++)
        {
            if (hits[i] != myCollider)
            {
                Attach(hits[i]);
                break;
            }
        }
    }
}

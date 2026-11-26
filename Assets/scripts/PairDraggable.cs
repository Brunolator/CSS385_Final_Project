// Two draggables that cannot be dragged too far apart from each other
// They must be under the same parent object in Unity
using NUnit.Framework;
using UnityEngine;

public class PairDraggable : Draggable
{
    GameObject other;
    float maxDistance;

    protected override void Awake()
    {
        base.Awake();

        // Get the other PairDraggable game object
        Assert.IsNotNull(transform.parent, "PairDraggable requires a parent object with a second PairDraggable");
        PairDraggable[] allPairDraggables = transform.parent.GetComponentsInChildren<PairDraggable>();
        Assert.IsTrue(allPairDraggables.Length == 2 && (allPairDraggables[0] == this || allPairDraggables[1] == this),
            "There must be exactly 2 PairDraggable anchors per power line");
        if (allPairDraggables[0] == this)
        {
            other = allPairDraggables[1].gameObject;
        }
        else
        {
            other = allPairDraggables[0].gameObject;
        }
    }

    // Moves to the given drag location, bounded by power line max length
    protected override void Move(Vector3 dragLocation)
    {
        Vector3 diff = (dragLocation - other.transform.position);
        float distance = Mathf.Min(diff.magnitude, maxDistance);
        Vector3 direction = diff.normalized;
        Vector3 destination = other.transform.position + distance * direction;
        base.Move(destination);
    }

    // Returns the distance from me to the other PairDraggable
    public float DistanceFromOther()
    {
        return (transform.position - other.transform.position).magnitude;
    }

    // Sets the max distance from the other PairDraggable
    public void SetMaxDistance(float newMaxDistance)
    {
        maxDistance = newMaxDistance;
    }
}

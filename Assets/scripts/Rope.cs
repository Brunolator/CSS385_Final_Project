using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class Rope : MonoBehaviour
{
    // Rope ends
    public Rigidbody2D startAnchor;
    public Rigidbody2D endAnchor;

    // Rope properties
    public int segmentCount = 15;
    public int segmentsPerUnit = 4;
    public float sagAmount = 1.5f;
    public float segmentRadius = 0.08f;
    public float segmentMass = 0.2f;
    public float linearDamping = 0.5f;

    // Makes the rope visible
    private LineRenderer lineRenderer;
    // List of rope segments
    private List<Rigidbody2D> segments = new List<Rigidbody2D>();
    private List<HingeJoint2D> joints = new List<HingeJoint2D>();
    private List<Vector2> segmentPositions = new List<Vector2>();
    private bool simulating = false;

    private void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
    }

    private void LateUpdate()
    {
        if (simulating)
        {
            // If simulating, update line renderer based on segments
            lineRenderer.positionCount = segments.Count + 2;
            lineRenderer.SetPosition(0, startAnchor.position);
            for (int i = 0; i < segments.Count; i++)
            {
                lineRenderer.SetPosition(i + 1, segments[i].position);
            }
            lineRenderer.SetPosition(segments.Count + 1, endAnchor.position);

        }
        else
        {
            // If not simulating, update line renderer based on segment locations
            lineRenderer.positionCount = segmentPositions.Count + 2;
            lineRenderer.SetPosition(0, startAnchor.position);
            for (int i = 0; i < segmentPositions.Count; i++)
            {
                lineRenderer.SetPosition(i + 1, segmentPositions[i]);
            }
            lineRenderer.SetPosition(segmentPositions.Count + 1, endAnchor.position);
        }
    }

    // Sets the ends of the rope
    public void SetAnchors(Rigidbody2D startAnchor_, Rigidbody2D endAnchor_)
    {
        startAnchor = startAnchor_;
        endAnchor = endAnchor_;
    }

    // Create the rope
    public void GenerateRope()
    {
        Assert.IsTrue(startAnchor != null && endAnchor != null);
        RecalculateRopePoints();
        lineRenderer.positionCount = segmentCount + 2;

        // Loop of generating rope points and connecting each to the previous
        Rigidbody2D previousBody = startAnchor; // Store the rope's previous connection
        for (int i = 0; i < segmentCount; i++)
        {
            Vector2 segmentPos = segmentPositions[i];

            // Create the segment
            GameObject segment = CreateSegment(segmentPos, previousBody, "RopeSegment_" + i, i == 0);
            Rigidbody2D segmentRB = segment.GetComponent<Rigidbody2D>();
            segments.Add(segmentRB);
            previousBody = segmentRB;
        }

        // Add the hinge from the last object to the end
        AddHingeJoint(endAnchor, previousBody, false);
        simulating = true;
    }

    // Get the points that the rope segments will take
    public void RecalculateRopePoints()
    {
        Assert.IsTrue(startAnchor != null && endAnchor != null);
        ClearRope();
        // Update segment count
        segmentCount = Mathf.Max(1, Mathf.RoundToInt(GetParabolaLength() * segmentsPerUnit));
        for (int i = 0; i < segmentCount; i++)
        {
            // Calculate the segment position
            float t = (float)(i + 1) / (segmentCount + 1); // Number between 0 and 1 for segment index
            Vector2 linearPos = Vector2.Lerp(startAnchor.position, endAnchor.position, t); // Position on the line between points
            float sagOffset = 4f * sagAmount * t * (1f - t); // Y offset in segment position caused by sag
            Vector2 segmentPos = linearPos + Vector2.down * sagOffset; // Segment position
            segmentPositions.Add(segmentPos);
        }
    }

    private GameObject CreateSegment(Vector2 SegmentPos, Rigidbody2D connectTo, string name, bool isFirstSegment)
    {
        // Create the segment
        GameObject segmentObject = new GameObject(name);
        segmentObject.transform.SetParent(transform);
        segmentObject.transform.position = SegmentPos;
        segmentObject.layer = LayerMask.NameToLayer("Standable"); // Allow the player to stand on it

        // Create the segment's rigid body
        Rigidbody2D rb = segmentObject.AddComponent<Rigidbody2D>();
        rb.mass = segmentMass;
        rb.linearDamping = linearDamping;
        rb.angularDamping = 0.5f;
        rb.interpolation = RigidbodyInterpolation2D.Interpolate;

        // Add a collider
        CircleCollider2D collider = segmentObject.AddComponent<CircleCollider2D>();
        collider.radius = segmentRadius;

        // Attach the segment to the rope using a joint
        AddHingeJoint(rb, connectTo, isFirstSegment);
        return segmentObject;
    }

    // Make a joint from the given game object to the given rigid body
    private void AddHingeJoint(Rigidbody2D connectFrom, Rigidbody2D connectTo, bool isFirstSegment)
    {
        HingeJoint2D joint = connectFrom.gameObject.AddComponent<HingeJoint2D>();
        joint.autoConfigureConnectedAnchor = false;
        joint.connectedBody = connectTo;

        // Set connectedAnchor to the exact spawn offset so the rope doesn't snap/jitter on frame 1
        if (isFirstSegment)
        {
            joint.anchor = connectTo.transform.InverseTransformPoint(connectTo.position);
            joint.connectedAnchor = Vector2.zero;
        }
        else
        {
            joint.anchor = Vector2.zero;
            joint.connectedAnchor = connectTo.transform.InverseTransformPoint(connectFrom.position);
        }
        joints.Add(joint);
    }

    // Set the rope's sag
    public void SetSag(float amount)
    {
        sagAmount = amount;
        GenerateRope();
    }

    // Delete the rope by deleting all rope segments
    public void ClearRope()
    {
        simulating = false;

        // Clear all rope segments
        foreach (var segment in segments)
        {
            if (segment != null)
            {
                Destroy(segment.gameObject);
            }
        }
        segments.Clear();

        // Clear all rope joints
        foreach (var joint in joints)
        {
            if (joint != null)
            {
                Destroy(joint);
            }
        }
        joints.Clear();

        // Clear all segment positions
        segmentPositions.Clear();
    }

    // Uses complicated math stuff to get the parabola length
    public float GetParabolaLength()
    {
        Vector2 start = startAnchor.position;
        Vector2 end = endAnchor.position;

        float w = Mathf.Abs(end.x - start.x);
        float dy = end.y - start.y;

        // Straight line fallback if anchors are vertically aligned or sag is ~0
        if (w < 0.0001f || Mathf.Abs(sagAmount) < 0.0001f)
            return Vector2.Distance(start, end);

        // Slopes at start (x=0), midpoint (x=w/2), and end (x=w)
        float mStart = (dy - 4f * sagAmount) / w;
        float mMid = dy / w;
        float mEnd = (dy + 4f * sagAmount) / w;

        // Simpson's 1/3 Rule approximation (within ~99.9% of exact integral)
        float lStart = Mathf.Sqrt(1f + mStart * mStart);
        float lMid = Mathf.Sqrt(1f + mMid * mMid);
        float lEnd = Mathf.Sqrt(1f + mEnd * mEnd);

        return (w / 6f) * (lStart + 4f * lMid + lEnd);
    }
}

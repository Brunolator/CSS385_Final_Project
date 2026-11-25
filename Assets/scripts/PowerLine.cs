using UnityEngine;

public class PowerLine : Draggable
{
    GameObject anchor1;
    GameObject anchor2;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected override void Awake()
    {
        base.Awake();
        Transform anchor1Transform = transform.Find("Anchor1");
        Transform anchor2Transform = transform.Find("Anchor2");

        if (anchor1Transform != null)
        {
            anchor1 = anchor1Transform.gameObject;
        }
        if (anchor2Transform != null)
        {
            anchor2 = anchor2Transform.gameObject;
        }
    }

    protected override void Update()
    {
        bool dragging = anchor1.GetComponent<Draggable>().IsBeingDragged() || anchor2.GetComponent<Draggable>().IsBeingDragged();
        Debug.Log("dragging: " + dragging);
    }
}

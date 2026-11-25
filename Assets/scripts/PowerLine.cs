using UnityEngine;

public class PowerLine : MonoBehaviour
{
    GameObject anchor1;
    GameObject anchor2;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
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

    void Update()
    {
        bool dragging = anchor1.GetComponent<Draggable>().IsBeingDragged() || anchor2.GetComponent<Draggable>().IsBeingDragged();
    }
}

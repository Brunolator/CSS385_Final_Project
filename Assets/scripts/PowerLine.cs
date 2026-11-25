using UnityEngine;

public class PowerLine : Simulated
{
    Transform anchor1;
    Transform anchor2;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected override void Awake()
    {
        base.Awake();
        anchor1 = transform.Find("Anchor1");
        anchor2 = transform.Find("Anchor2");
    }

    void FixedUpdate()
    {
        //Debug.Log("anchor1: " + anchor1.position.x);
    }

    public override void StartSimulation()
    {

    }

    public override void StopSimulation()
    {

    }
}

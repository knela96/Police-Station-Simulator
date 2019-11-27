using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Point : MonoBehaviour
{
    bool available;
    CitizenBehaviour assigned;
    Transform point;
    AssignPoints assignpoints;

    // Use this for initialization
    void Awake()
    {
        assignpoints = GameObject.Find("Sofas").GetComponent<AssignPoints>();
        point = transform;
        available = true;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public Point setAgent(GameObject agent)
    {
        assignpoints.numAssigned++;
        assigned = agent.GetComponent<CitizenBehaviour>();
        available = false;
        return this;
    }

    public void Release()
    {
        assignpoints.numAssigned--;
        assigned = null;
        available = true;
    }

    public bool isAvailable()
    {
        return available;
    }

    public Transform getPoint()
    {
        return point;
    }
}

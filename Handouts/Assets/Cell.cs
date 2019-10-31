using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell : MonoBehaviour
{

    bool available;
    PoliceBehaviour assigned;
    Transform point;

    // Use this for initialization
    void Start()
    {
        point = transform;
        available = true;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public Cell setAgent(GameObject agent)
    {
        assigned = agent.GetComponent<PoliceBehaviour>();
        available = false;
        return this;
    }

    public void Release()
    {
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

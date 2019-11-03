using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Desk : MonoBehaviour {

    bool available;
    public PoliceBehaviour assigned;
    Transform point;

	// Use this for initialization
	void Awake () {
        point = transform;
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public Desk setAgent(GameObject agent)
    {
        assigned = agent.GetComponent<PoliceBehaviour>();
        return this;
    }

    public void Release()
    {
        assigned = null;
    }

   public bool isAvailable()
    {
        return assigned == null;
    }

    public Transform getPoint()
    {
        return point;
    }
}

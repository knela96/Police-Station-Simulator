using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AssignDesk : MonoBehaviour {

    public List<GameObject> desks;
    public int desksav = 0; 
	// Use this for initialization
    
	void Awake () {
        foreach (Transform child in gameObject.transform)
        {
            foreach (Transform child2 in child.transform)
            {
                if (child2.name == "AI_Prop_ComputerChair")
                {
                    Transform c = child2.transform.Find("Point");
                    desks.Add(c.gameObject); //Stores the current desk points
                    //if (c.gameObject.active == true)
                        desksav++;
                }
            }
        }
    }

    public bool FreeDesks()
    {
        for (int i = 0; i < desks.Count; ++i)
        {
            if (desks[i].GetComponent<Desk>().isAvailable() && desks[i].active)
                return true;
        }
        return false;
    }

    // Update is called once per frame
    void Update () {
		
	}
}

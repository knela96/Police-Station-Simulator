using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AssignDesk : MonoBehaviour {

    public List<GameObject> desks;
	// Use this for initialization
    
	void Awake () {
        foreach (Transform child in gameObject.transform)
        {
            Transform c = child.Find("AI_Prop_ComputerChair").transform.Find("Point");
            desks.Add(c.gameObject); //Stores the current desk points
        }
        
	}

    //public bool numAgents(int count)
    //{
    //    int num = 0;
    //    foreach (Transform child in gameObject.transform)
    //    {
    //        if (!child.GetComponent<Desk>().isAvailable())
    //            num++;
    //    }
    //    if (count < num)
    //        return true;
    //    else
    //        return false;
    //}

    // Update is called once per frame
    void Update () {
		
	}
}

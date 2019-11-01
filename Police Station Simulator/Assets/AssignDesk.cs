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
            desks.Add(c.gameObject);
        }
        
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}

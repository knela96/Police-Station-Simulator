using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AssignDesk : MonoBehaviour {

    public List<GameObject> desks;
	// Use this for initialization
	void Start () {
        foreach (Transform child in gameObject.transform)
        {
            desks.Add(child.gameObject);
        }
        
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}

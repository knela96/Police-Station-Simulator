using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI2Camera : MonoBehaviour {

    public Camera camera = null;
    // Use this for initialization
    void Start () {
        camera = GameObject.Find("Main Camera").GetComponent<Camera>();
    }
	
	// Update is called once per frame
	void Update () {
        transform.LookAt(transform.position + camera.transform.rotation * Vector3.forward,
                                           camera.transform.rotation * Vector3.up);
    }
}

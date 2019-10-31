using UnityEngine;
using System.Collections;

public class KinematicFaceMovement : MonoBehaviour {

	public float min_angle = 1.0f;

	Move move;

	// Use this for initialization
	void Start () {
		move = GetComponent<Move>();
	}
	
	// Update is called once per frame
	void Update () 
	{
        float delta = Vector3.SignedAngle(transform.forward, move.current_velocity, new Vector3(0.0f, 1.0f, 0.0f));

		if(Mathf.Abs(delta) < min_angle)
			move.SetRotationVelocity(0.0f);
		else
			move.SetRotationVelocity(delta);
	}
}

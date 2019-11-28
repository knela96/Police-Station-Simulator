using UnityEngine;
using System.Collections;

public class KinematicFlee : MonoBehaviour {

	Move move;

	// Use this for initialization
	void Start () {
		move = GetComponent<Move>();
	}
	
	// Update is called once per frame
	void Update () 
	{
		Vector3 diff = transform.position - move.target.transform.position;
		diff = diff.normalized * move.max_mov_speed;

		move.SetMovementVelocity(diff);
	}
}

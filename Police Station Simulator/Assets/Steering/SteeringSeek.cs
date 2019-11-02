using UnityEngine;
using System.Collections;

public class SteeringSeek : SteeringAbstract {

	Move move;

	// Use this for initialization
	void Awake () {
		move = GetComponent<Move>(); 
	}
	
	// Update is called once per frame
	void Update () 
	{
		//Steer(move.target.transform.position);
	}

	public void Steer(Vector3 target,int priority)
	{
		Vector3 distance = (target - transform.position).normalized * move.max_mov_acceleration * move.cur_run_multiplier;

		move.AccelerateMovement(distance,priority);
	}
}

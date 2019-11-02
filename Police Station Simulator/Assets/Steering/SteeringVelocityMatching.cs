using UnityEngine;
using System.Collections;

public class SteeringVelocityMatching : SteeringAbstract {

	public float time_to_target = 0.25f;

	Move move;
	Move target_move;

	// Use this for initialization
	void Awake () {
		move = GetComponent<Move>();
        if(move.target != null)
		    target_move = move.target.GetComponent<Move>();
	}
	
	// Update is called once per frame
	void Update () 
	{
		if(target_move)
		{
			Vector3 acceleration = (target_move.current_velocity - move.current_velocity) / time_to_target;
            
            if (acceleration.magnitude > move.max_mov_acceleration * move.cur_run_multiplier)
			{
                acceleration = acceleration.normalized * move.max_mov_acceleration * move.cur_run_multiplier;
			}

			move.AccelerateMovement(acceleration,priority);
		}
	}
}

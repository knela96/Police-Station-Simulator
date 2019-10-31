using UnityEngine;
using System.Collections;

public class SteeringVelocityMatching : MonoBehaviour {

	public float time_to_target = 0.25f;

	Move move;
	Move target_move;

	// Use this for initialization
	void Start () {
		move = GetComponent<Move>();
		target_move = move.target.GetComponent<Move>();
	}
	
	// Update is called once per frame
	void Update () 
	{
		if(target_move)
		{
			// Create a vector that describes the ideal velocity
			Vector3 ideal_movement = target_move.current_velocity;

			// Calculate acceleration needed to match that velocity
			Vector3 acceleration = ideal_movement - move.current_velocity;
			acceleration /= time_to_target;

			// Cap acceleration
			if(acceleration.magnitude > move.max_mov_acceleration)
			{
				acceleration.Normalize();
				acceleration *= move.max_mov_acceleration;
			}

			move.AccelerateMovement(acceleration);
		}
	}
}

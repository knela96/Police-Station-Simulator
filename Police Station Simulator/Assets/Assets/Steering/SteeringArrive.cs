using UnityEngine;
using System.Collections;

public class SteeringArrive : SteeringAbstract {

	public float min_distance = 1.0f;
	public float slow_distance = 5.0f;
	public float time_to_target = 0.1f;

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

	public void Steer(Vector3 target)
	{
        Vector3 direction = target - transform.position;
        float distance = direction.magnitude;
        if (distance < min_distance)
        {
            move.SetMovementVelocity(Vector3.zero);
            return;
        }

        //Calculates desired speed
        float target_speed;
        if (distance < slow_distance)
            target_speed = move.max_mov_speed * (distance / slow_distance);
        else
            target_speed = move.max_mov_speed;

        Vector3 target_vel = direction.normalized * target_speed;
        Vector3 accel = (target_vel - move.current_velocity) / time_to_target;

        //Clamps Acceleration
        if (accel.magnitude > move.max_mov_acceleration * move.cur_run_multiplier)
            accel = accel.normalized * move.max_mov_acceleration * move.cur_run_multiplier;

        move.AccelerateMovement(accel,priority);
    }

	void OnDrawGizmosSelected() 
	{
		// Display the explosion radius when selected
		Gizmos.color = Color.white;
		Gizmos.DrawWireSphere(transform.position, min_distance);

		Gizmos.color = Color.green;
		Gizmos.DrawWireSphere(transform.position, slow_distance);
	}
}

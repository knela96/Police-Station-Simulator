using UnityEngine;
using System.Collections;

public class SteeringCollisionAvoidance : SteeringAbstract
{

	public LayerMask mask;
	public float search_radius = 5.0f;

	Move move;

	// Use this for initialization
	void Start () {
		move = GetComponent<Move>();
	}

	// Update is called once per frame
    void Update () 
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, search_radius, mask);

        // collision data
        GameObject target = null;
        float target_shortest_time = float.PositiveInfinity;
        float target_min_separation = 0.0f;
        float target_distance = 0.0f;
        Vector3 target_relative_pos = Vector3.zero;
        Vector3 target_relative_vel = Vector3.zero;

        foreach(Collider col in colliders)
        {
            GameObject go = col.gameObject;

            if(go == gameObject) 
                continue;

            Move target_move = go.GetComponent<Move>();

            if(target_move == null)
            	continue;

            // calculate time to collision
            Vector3 relative_pos = go.transform.position - transform.position;
            Vector3 relative_vel = target_move.current_velocity - move.current_velocity;
            float relative_speed = relative_vel.magnitude;
            float time_to_collision = Vector3.Dot(relative_pos, relative_vel) / relative_speed * relative_speed;

            // make sure there is a collision at all
            float distance = relative_pos.magnitude;
            float min_separation = distance - relative_speed * time_to_collision;
            if(min_separation > 2.0f * search_radius)
            	continue;

            if(time_to_collision > target_shortest_time)
            	continue;

            Debug.Log("Avoiding " + go.name);
            target = go;
            target_shortest_time = time_to_collision;
            target_min_separation = min_separation;
            target_distance = distance;
            target_relative_pos = relative_pos;
            target_relative_vel = relative_vel;
         }

         //if we have a target, avoid collision
         if(target != null)
         {
         	Vector3 escape_pos;
         	if(target_min_separation <= 0.0f || target_distance < search_radius * 2.0f)
         		escape_pos = target.transform.position - transform.position;
         	else
         		escape_pos = target_relative_pos + target_relative_vel * target_shortest_time;

         	move.AccelerateMovement(- (escape_pos.normalized * move.max_mov_acceleration), priority);
         }
    }

	void OnDrawGizmosSelected() 
	{
		// Display the explosion radius when selected
		Gizmos.color = Color.yellow;
		Gizmos.DrawWireSphere(transform.position, search_radius);
	}
}

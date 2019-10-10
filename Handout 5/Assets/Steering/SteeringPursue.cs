using UnityEngine;
using System.Collections;

public class SteeringPursue : MonoBehaviour {

	public float max_seconds_prediction = 5.0f;

	Move move;
	SteeringArrive arrive;

	// Use this for initialization
	void Start () {
		move = GetComponent<Move>();
		arrive = GetComponent<SteeringArrive>();
	}
	
	// Update is called once per frame
	void Update () 
	{
		Steer(move.target.transform.position, move.target.GetComponent<Move>().current_velocity);
	}

	public void Steer(Vector3 target, Vector3 velocity)
	{
		Vector3 diff = target - transform.position;

		float distance = diff.magnitude;
		float current_speed = move.current_velocity.magnitude;
		float prediction;

		// is the speed too small ?
		if(current_speed < distance / max_seconds_prediction)
			prediction = max_seconds_prediction;
		else
			prediction = distance / current_speed;

		arrive.Steer(target + (velocity * prediction));
	}
}

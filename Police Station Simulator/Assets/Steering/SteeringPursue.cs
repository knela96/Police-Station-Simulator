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
		//Steer(move.target.transform.position, move.target.GetComponent<Move>().current_velocity);
	}

	public void Steer(Vector3 target, Vector3 velocity)
	{
		Vector3 direction = target - transform.position;

		float distance = direction.magnitude;
		float c_speed = move.current_velocity.magnitude;

		float predictionTime;

		if(c_speed <= distance / max_seconds_prediction)
            predictionTime = max_seconds_prediction;
		else
            predictionTime = distance / c_speed;

		arrive.Steer(target + (velocity * predictionTime));
	}
}

using UnityEngine;
using System.Collections;

public class KinematicWander : MonoBehaviour {

	public float max_angle = 0.5f;

	Move move;

	// Use this for initialization
	void Start () {
		move = GetComponent<Move>();
	}

	// number [-1,1] values around 0 more likely
	float RandomBinominal()
	{
		return Random.value * Random.value;
	}
	
	// Update is called once per frame
	void Update () 
	{
		// random rotation
		float angle = RandomBinominal() * max_angle;
		Vector3 velocity = Quaternion.AngleAxis(Mathf.Rad2Deg * angle, Vector3.up) * Vector3.forward;

		velocity *= move.max_mov_speed;

		move.SetMovementVelocity (velocity);
	}
}

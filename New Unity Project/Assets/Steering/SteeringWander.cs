using UnityEngine;
using System.Collections;

public class SteeringWander : MonoBehaviour {

	public float min_distance = 0.1f;
	public float time_to_target = 0.25f;

	Move move;

	// Use this for initialization
	void Start () {
		move = GetComponent<Move>();
	}

	// Update is called once per frame
	void Update () 
	{
        // TODO Homework: Update the target location to a random point in a circle
        // You could just call seek.Steer() / arrive.Steer() or simply do the calculations by yourself
        // like the code below.

		Vector3 diff = move.target.transform.position - transform.position;

		if(diff.magnitude < min_distance)
			return;

		diff /= time_to_target;

		move.AccelerateMovement(diff);
	}

	void OnDrawGizmosSelected() 
	{
		// Display the explosion radius when selected
		Gizmos.color = Color.white;
		Gizmos.DrawWireSphere(transform.position, min_distance);
	}
}

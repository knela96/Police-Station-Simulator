using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Move : MonoBehaviour {

	public GameObject target;
	public GameObject aim;
	public Slider arrow;

	public float max_mov_speed = 5.0f;
	public float max_mov_acceleration = 0.1f;
	public float max_rot_speed = 10.0f; // in degrees / second
	public float max_rot_acceleration = 0.1f; // in degrees

	[Header("-------- Read Only --------")]
	public Vector3 movement = Vector3.zero;
	public float rotation = 0.0f; // degrees

	// Methods for behaviours to set / add velocities
	public void SetMovementVelocity (Vector3 velocity) 
	{
		movement = velocity;
	}

	public void AccelerateMovement (Vector3 velocity) 
	{
		movement += velocity;
	}

	public void SetRotationVelocity (float rotation_velocity) 
	{
		rotation = rotation_velocity;
	}

	public void AccelerateRotation (float rotation_acceleration) 
	{
		rotation += rotation_acceleration;
	}
	
	// Update is called once per frame
	void Update () 
	{
		// cap velocity
		if(movement.magnitude > max_mov_speed)
		{
			movement.Normalize();
			movement *= max_mov_speed;
		}

		// cap rotation
		Mathf.Clamp(rotation, -max_rot_speed, max_rot_speed);

		// rotate the arrow
		float angle = Mathf.Atan2(movement.x, movement.z);
		aim.transform.rotation = Quaternion.AngleAxis(Mathf.Rad2Deg * angle, Vector3.up);

		// strech it
		arrow.value = movement.magnitude * 4;

		// final rotate
		transform.rotation *= Quaternion.AngleAxis(rotation * Time.deltaTime, Vector3.up);

        // finally move
        movement.y = 0.0f;
		transform.position += movement * Time.deltaTime;
	}
}

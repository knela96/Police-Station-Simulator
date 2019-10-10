using UnityEngine;
using System.Collections;

public class SteeringWander : MonoBehaviour {

	public Vector3 offset = Vector3.zero;
	public float radius = 1.0f;
	public float min_update = 0.5f;
	public float max_update = 3.0f;

	SteeringSeek seek;
	Vector3 random_point;

	// Use this for initialization
	void Start () {
		seek = GetComponent<SteeringSeek>();
		ChangeTarget();
	}

    private void Update()
    {
        seek.Steer(random_point);
    }

    // Update is called once per frame
    void ChangeTarget () 
	{
		random_point = Random.insideUnitSphere;
		random_point *= radius;
		random_point += transform.position + offset;
		random_point.y = transform.position.y;

		Invoke("ChangeTarget", Random.Range(min_update, max_update));
	}

	void OnDrawGizmosSelected() 
	{
		if(this.isActiveAndEnabled)
		{
			// Display the explosion radius when selected
			Gizmos.color = Color.yellow;
			Gizmos.DrawWireSphere(transform.TransformPoint(offset), radius);
		
			Gizmos.color = Color.red;
			Gizmos.DrawWireSphere(random_point, 0.2f);
		}
	}
}

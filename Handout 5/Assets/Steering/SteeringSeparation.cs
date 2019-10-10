using UnityEngine;
using System.Collections;

public class SteeringSeparation : MonoBehaviour {

	public LayerMask mask;
	public float search_radius = 5.0f;
	public AnimationCurve falloff;

	Move move;

	// Use this for initialization
	void Start () {
		move = GetComponent<Move>();
	}

	// Update is called once per frame
    void Update () 
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, search_radius, mask);
        Vector3 final = Vector3.zero;

        foreach(Collider col in colliders)
        {
            GameObject go = col.gameObject;

            if(go == gameObject) 
                continue;

            Vector3 diff = transform.position - go.transform.position;
            float distance = diff.magnitude;
            float acceleration = (1.0f - falloff.Evaluate(distance / search_radius)) * move.max_mov_acceleration;

            final += diff.normalized * acceleration;
        }

        float final_strength = final.magnitude;
        if(final_strength > 0.0f)
        {
            if(final_strength > move.max_mov_acceleration)
                final = final.normalized * move.max_mov_acceleration;
            move.AccelerateMovement(final);
        }
    }

	void OnDrawGizmosSelected() 
	{
		// Display the explosion radius when selected
		Gizmos.color = Color.yellow;
		Gizmos.DrawWireSphere(transform.position, search_radius);
	}
}

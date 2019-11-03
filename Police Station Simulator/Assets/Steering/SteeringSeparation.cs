using UnityEngine;
using System.Collections;

public class SteeringSeparation : SteeringAbstract
{

    public LayerMask mask;
    public float search_radius = 5.0f;
    public AnimationCurve falloff;

    Move move;

    // Use this for initialization
    void Awake()
    {
        move = GetComponent<Move>();
    }

    // Update is called once per frame
    void Update()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, search_radius, mask);
        Vector3 accel = Vector3.zero;
        float acceleration = 0;

        for(int i = 0; i < colliders.Length; ++i)
        {
            GameObject entity = colliders[i].gameObject;

            //Accelerate the target with the curve form
            if (entity != gameObject)
            {
                Vector3 direction = transform.position - entity.transform.position;
                float distance = direction.magnitude;

                acceleration = (1.0f - falloff.Evaluate(distance / search_radius)) * move.max_mov_acceleration;
                accel += direction.normalized * acceleration;
            }
        }

        if (accel.magnitude > 0.0f)
        {
            if (accel.magnitude > move.max_mov_acceleration)
                accel = accel.normalized * move.max_mov_acceleration;
            move.AccelerateMovement(accel,priority);
        }
    }

    void OnDrawGizmosSelected()
    {
        // Display the explosion radius when selected
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, search_radius);
    }
}

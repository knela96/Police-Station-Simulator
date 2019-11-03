using UnityEngine;
using System.Collections;

public class SteeringCollisionAvoidance : SteeringAbstract
{

    public LayerMask mask;
    public float radius = 2.0f;

    Move move;

    // Use this for initialization
    void Awake()
    {
        move = GetComponent<Move>();
    }

    // Update is called once per frame
    void Update()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, radius, mask);

        // collision data
        GameObject aux_target = null;
        float aux_separation = 0.0f;
        float aux_distance = 0.0f;
        Vector3 aux_position = Vector3.zero;
        Vector3 aux_velocity = Vector3.zero;

        float shortestTime = float.PositiveInfinity;

        for (int i = 0; i < colliders.Length; ++i)
        {
            GameObject target = colliders[i].gameObject;

            if (target == gameObject)
                continue;

            Move move_target = target.GetComponent<Move>();

            if(move_target != null) {
                Vector3 position = target.transform.position - transform.position;
                Vector3 velocity = move_target.current_velocity - move.current_velocity;
                float speed = velocity.magnitude;
                float time = Vector3.Dot(position, velocity) / (speed * speed);

                float distance = position.magnitude;
                float separation = distance - speed * time;
                if(separation <= 2 * radius)
                {
                    if((time > 0) && (time < shortestTime))
                    {
                        shortestTime = time;
                        aux_target = target;
                        aux_separation = separation;
                        aux_distance = distance;
                        aux_position = position;
                        aux_velocity = velocity;
                    }
                }
            }
        }

        //if we have a target, avoid collision
        if (aux_target != null)
        {
            Vector3 pos;
            if (aux_separation <= 0.0f || aux_distance <= radius * 2.0f)
                pos = aux_target.transform.position - transform.position;
            else
                pos = aux_position + (aux_velocity * shortestTime);

            pos.y = 0;
            pos.Normalize();
            move.AccelerateMovement((pos * -move.max_mov_acceleration), priority);
            move.resetTransform();
        }
    }

    void OnDrawGizmosSelected()
    {
        // Display the explosion radius when selected
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}
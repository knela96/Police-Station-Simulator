using UnityEngine;
using System.Collections;

[System.Serializable]
public class O_Ray
{
    public Vector3 direction = Vector3.forward;
    public float length = 2.0f;
}

public class SteeringObstacleAvoidance : SteeringAbstract {

    public LayerMask mask;
    public float avoid_distance = 5.0f;
    public O_Ray[] rays;

    Move move;
    SteeringSeek seek;

    // Use this for initialization
    void Awake () {
        move = GetComponent<Move>(); 
        seek = GetComponent<SteeringSeek>();
    }
    
    // Update is called once per frame
    void Update () 
    {
        float angle = Mathf.Atan2(move.current_velocity.x, move.current_velocity.z);
        Quaternion rotation = Quaternion.AngleAxis(Mathf.Rad2Deg * angle, Vector3.up);

        for(int i = 0; i < rays.Length; ++i)
        {
            O_Ray ray = rays[i];
            RaycastHit ray_hit;

            if (Physics.Raycast(transform.position, rotation * ray.direction.normalized, out ray_hit, ray.length, mask) == true)
            {
                Vector3 target = new Vector3(ray_hit.point.x, 0, ray_hit.point.z);
                target += ray_hit.normal * avoid_distance;

                seek.Steer(target, priority);
            }
        }
    }

    void OnDrawGizmosSelected() 
    {
        if(move && this.isActiveAndEnabled)
        {
            // Display the explosion radius when selected
            Gizmos.color = Color.red;
            float angle = Mathf.Atan2(move.current_velocity.x, move.current_velocity.z);
            Quaternion rotation = Quaternion.AngleAxis(Mathf.Rad2Deg * angle, Vector3.up);

            for (int i = 0; i < rays.Length; ++i)
            {
                O_Ray ray = rays[i];
                Gizmos.DrawLine(transform.position, transform.position + (rotation * ray.direction.normalized) * ray.length);
            }
        }
    }
}


using UnityEngine;
using System.Collections;

[System.Serializable]
public class rays
{
    public float length = 2.0f;
    public Vector3 direction = Vector3.forward;
}

public class SteeringObstacleAvoidance : SteeringAbstract {

    public LayerMask mask;
    public float avoid_distance = 5.0f;
    public rays[] l_rays;

    Move move;
    SteeringSeek seek;

    // Use this for initialization
    void Start () {
        move = GetComponent<Move>(); 
        seek = GetComponent<SteeringSeek>();
    }
    
    // Update is called once per frame
    void Update () 
    {
        float angle = Mathf.Atan2(move.current_velocity.x, move.current_velocity.z);
        Quaternion q = Quaternion.AngleAxis(Mathf.Rad2Deg * angle, Vector3.up);

        for(int i = 0; i < l_rays.Length; ++i)
        {
            rays ray = l_rays[i];
            RaycastHit hit;

            if(Physics.Raycast(new Vector3(transform.position.x, 1.0f, transform.position.z), q * ray.direction.normalized, out hit, ray.length, mask) == true)
                seek.Steer(new Vector3(hit.point.x, transform.position.y, hit.point.z) + hit.normal * avoid_distance,priority);
        }
    }

    void OnDrawGizmosSelected() 
    {
        if(move && this.isActiveAndEnabled)
        {
            // Display the explosion radius when selected
            Gizmos.color = Color.red;
            float angle = Mathf.Atan2(move.current_velocity.x, move.current_velocity.z);
            Quaternion q = Quaternion.AngleAxis(Mathf.Rad2Deg * angle, Vector3.up);

            for (int i = 0; i < l_rays.Length; ++i)
            {
                rays ray = l_rays[i];
                Gizmos.DrawLine(transform.position, transform.position + (q * ray.direction.normalized) * ray.length);
            }
        }
    }
}


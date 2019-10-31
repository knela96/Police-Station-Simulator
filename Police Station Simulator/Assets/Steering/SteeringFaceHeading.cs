using UnityEngine;
using System.Collections;

public class SteeringFaceHeading : MonoBehaviour
{

    Move move;
    SteeringAlign align;

    // Use this for initialization
    void Start()
    {
        move = GetComponent<Move>();
        align = GetComponent<SteeringAlign>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Steer(Vector3 target)
    {
        if (move.current_velocity.magnitude == 0)
            return;

        Vector3 direction = (target - transform.position).normalized;
        
        Quaternion lookRotation = Quaternion.LookRotation(direction);

        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5);
    }
}

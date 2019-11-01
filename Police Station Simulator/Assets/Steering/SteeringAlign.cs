using UnityEngine;
using System.Collections;

public class SteeringAlign : MonoBehaviour
{

    public float min_angle = 10.0f;
    public float slow_angle = 0.1f;
    public float time_to_accel = 1.0f;

    Move move;
    // Use this for initialization
    void Awake()
    {
        move = GetComponent<Move>();
    }

    // Update is called once per frame
    void Update()
    {
        if (move.target != null)
            Steering(move.target.transform);
    }

    public void Steering(Transform target)
    {
        float rotation = Vector3.SignedAngle(transform.forward, move.target.transform.forward, Vector3.up);
        float rotationSize = Mathf.Abs(rotation);

        if (rotationSize < min_angle)
        {
            move.SetRotationVelocity(0.0f);
            return;
        }

        float target_rotation;

        if (rotationSize < slow_angle)
            target_rotation = move.max_rot_speed * (rotationSize / slow_angle);
        else
            target_rotation = move.max_rot_speed;

        float target_accel = target_rotation / time_to_accel;

        if (rotation < 0)
            target_accel = -target_accel;

        move.AccelerateRotation(Mathf.Clamp(target_accel, -move.max_rot_acceleration, move.max_rot_acceleration));
    }
}

using UnityEngine;
using System.Collections;
using BansheeGz.BGSpline.Components;
using BansheeGz.BGSpline.Curve;

public class SteeringFollowPath : MonoBehaviour {

	Move move;
	SteeringSeek seek;

    public float ratio_increment = 0.1f;
    public float min_distance = 1.0f;
    float current_ratio = 0.0f;
    Vector3 closest_point;
    public BGCcMath path;

    // Use this for initialization
    void Start () {
		move = GetComponent<Move>();
		seek = GetComponent<SteeringSeek>();

        // TODO 1: Calculate the closest point from the tank to the curve
        closest_point = path.CalcPositionByClosestPoint(transform.position);
    }

    // Update is called once per frame
    void Update () 
	{
        // TODO 2: Check if the tank is close enough to the desired point
        // If so, create a new point further ahead in the path
        seek.Steer(closest_point);

        //if (path.CalcPositionByDistanceRatio(current_ratio) == Vector3(0; 0; 0))
         closest_point = path.CalcPositionByClosestPoint(transform.position);



    }

	void OnDrawGizmosSelected() 
	{

		if(isActiveAndEnabled)
		{
			// Display the explosion radius when selected
			Gizmos.color = Color.green;
			// Useful if you draw a sphere were on the closest point to the path
		}
        Gizmos.DrawSphere(closest_point, 1);
	}
}

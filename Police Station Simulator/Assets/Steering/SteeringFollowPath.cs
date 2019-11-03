using UnityEngine;
using System.Collections;
using BansheeGz.BGSpline.Components;
using BansheeGz.BGSpline.Curve;
using UnityEngine.AI;

public class SteeringFollowPath : MonoBehaviour {

	Move move;
	SteeringSeek seek;
    SteeringArrive arrive;
    SteeringFaceHeading face_heading;
    public NavMeshPath path;
    public Transform pivot;
    int cur_node;
    public GameObject patrol1;
    public GameObject patrol2;
    public GameObject path_prefab;
    BGCurve curve = null;
    BGCcMath curve_path;
    public bool patroling;
    public GameObject go;
    int cur_patrol = -1;

    public float ratio_increment = 0.01f;
    public float min_distance = 0.1f;
    float current_ratio = 0.0f;
    public bool arrived = false;
    public bool going = false;

	// Use this for initialization
	void Awake () {
		move = GetComponent<Move>();
		seek = GetComponent<SteeringSeek>();
        arrive = GetComponent<SteeringArrive>();
        face_heading = GetComponent<SteeringFaceHeading>();
        path = new NavMeshPath();
        cur_node = 0;
        move.move = true;
    }

    // Update is called once per frame
    void Update () 
	{
        if(curve != null)
        {
            if (patroling && arrived)
            {
                if (current_ratio >= 1)
                {
                    current_ratio = 0;
                    going = true;
                }
            }
            else
            {
                if (current_ratio >= 1)
                {
                    arrived = true;
                    going = false;
                    if (patroling)
                    {
                        createPatrol(cur_patrol,true);
                    }
                    else
                        move.move = false;
                }
                
            }

            Vector3 target = curve_path.CalcPositionByDistanceRatio(current_ratio);

            Vector3 distance = transform.position - target;

            if (current_ratio <= 1.0f)
            {
                if (distance.magnitude < min_distance)
                    IncreaseRatio();
            }

            if (curve.PointsCount != 0)
            {
                if ((curve.Points[curve.PointsCount - 1].PositionWorld - transform.position).magnitude < min_distance && !patroling)
                {
                    arrive.Steer(curve.Points[curve.PointsCount - 1].PositionWorld);
                }
                else
                    seek.Steer(target, seek.priority);
            }

            face_heading.Steer(target);
        }
    }

    public void IncreaseRatio()
    {
        if (move.current_velocity.magnitude != 0)
            current_ratio += (ratio_increment * move.current_velocity.magnitude * Time.deltaTime) * move.cur_run_multiplier;
        else
            current_ratio += ratio_increment * Time.deltaTime;
    }


    private void OnDestroy()
    {
        deleteCurve();
    }

    public void deleteCurve()
    {
        curve = null;
        curve_path = null;
        current_ratio = 0;
        Destroy(go.gameObject);
    }

    public void calcPath(Transform target)
    {
        deleteCurve();
        NavMesh.CalculatePath(pivot.transform.position, target.transform.position, NavMesh.AllAreas, path);
        arrived = false;
        going = true;
        move = GetComponent<Move>();
        move.move = true;
        //Debug.Log(path.corners.Length);

        go = Instantiate(path_prefab);

        curve = go.GetComponent<BGCurve>();
        curve_path = go.GetComponent<BGCcMath>();

        for (int i = 0; i < path.corners.Length; ++i)
        {
            curve.AddPoint(new BGCurvePoint(curve, new Vector3(path.corners[i].x,0, path.corners[i].z), BGCurvePoint.ControlTypeEnum.Absent, path.corners[i], path.corners[i], true));
        }

    }

    public void createPatrol(int patrol,bool position)
    {
        if (position)
        {
            deleteCurve();
            if (patrol == 1)
                go = Instantiate(patrol1);
            else
                go = Instantiate(patrol2);

            curve = go.GetComponent<BGCurve>();
            curve_path = go.GetComponent<BGCcMath>();
            current_ratio = 0;
            patroling = true;
        }
        else
        {
            deleteCurve();
            cur_patrol = patrol;
            if (patrol == 1)
                go = Instantiate(patrol1);
            else
                go = Instantiate(patrol2);

            curve = go.GetComponent<BGCurve>();
            curve_path = go.GetComponent<BGCcMath>();

            if (cur_patrol != -1)
            {
                patroling = true;

                GameObject ob = new GameObject();
                ob.transform.position = curve.Points[0].PositionWorld;

                calcPath(ob.transform);

                Destroy(ob);
            }
        }

        //for (int i = 0; i < path.corners.Length; ++i)
        //{
        //    curve.AddPoint(new BGCurvePoint(curve, path.corners[i], BGCurvePoint.ControlTypeEnum.BezierSymmetrical, path.corners[i], path.corners[i], true));
        //}
    }

    void OnDrawGizmosSelected() 
	{

		if(isActiveAndEnabled)
		{
			// Display the explosion radius when selected
			Gizmos.color = Color.green;
			// Useful if you draw a sphere were on the closest point to the path
		}

	}
}

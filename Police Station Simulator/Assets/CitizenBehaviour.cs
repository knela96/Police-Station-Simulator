using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CitizenBehaviour : MonoBehaviour {

    Move move;
    SteeringArrive arrive;
    bool action;
    public Transform pivot;
    public NavMeshPath path;
    SteeringFollowPath follow_path;
    LevelLoop level;


    // Use this for initialization
    void Awake () {
        move = GetComponent<Move>();
        arrive = GetComponent<SteeringArrive>();
        action = false;
        follow_path = gameObject.GetComponent<SteeringFollowPath>();
        follow_path.path = new NavMeshPath();
        move.target = GameObject.Find("Reception_Point");
        follow_path.calcPath(move.target.transform);
        move.move = true;
        level = GameObject.Find("Level").GetComponent<LevelLoop>();
    }
	
	// Update is called once per frame
	void Update ()
    {

        Vector3 distance = move.target.transform.position - transform.position;

        if (distance.magnitude <= arrive.min_distance + 0.2 && !action && move.current_velocity == Vector3.zero)
        {
            move.target = GameObject.Find("Exit");
            follow_path.calcPath(move.target.transform);
            action = true;
            Instantiate(level.citizens[Random.Range(0, level.citizens.Length - 1)], move.target.transform.position,Quaternion.Euler(0,90,0));
            return;
        }
    }

    private void OnDestroy()
    {
        follow_path.deleteCurve();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other == GameObject.Find("Exit").GetComponent<Collider>() && action)
        {
            Destroy(gameObject);
        }
    }
}

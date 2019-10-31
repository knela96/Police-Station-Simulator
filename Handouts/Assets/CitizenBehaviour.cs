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


    // Use this for initialization
    void Start () {
        move = GetComponent<Move>();
        arrive = GetComponent<SteeringArrive>();
        action = false;
        follow_path = gameObject.GetComponent<SteeringFollowPath>();
        follow_path.path = new NavMeshPath();
    }
	
	// Update is called once per frame
	void Update ()
    {
        follow_path.calcPath(move.target.transform);

        Vector3 distance = move.target.transform.position - transform.position;

        if (distance.magnitude <= arrive.min_distance + 0.2 && !action && move.current_velocity == Vector3.zero)
        {
            move.target = GameObject.Find("Exit");
            follow_path.calcPath(move.target.transform);
            action = true;
            return;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other == GameObject.Find("Exit").GetComponent<Collider>())
        {
            Destroy(gameObject);
        }
    }
}

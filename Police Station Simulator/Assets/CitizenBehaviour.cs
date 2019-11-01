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
    Animator anim;


    // Use this for initialization
    void Awake () {
        move = GetComponent<Move>();
        move.move = true;
        arrive = GetComponent<SteeringArrive>();
        action = false;
        follow_path = gameObject.GetComponent<SteeringFollowPath>();
        follow_path.path = new NavMeshPath();
        move.target = GameObject.Find("Reception_Point");
        follow_path.calcPath(move.target.transform);
        level = GameObject.Find("Level").GetComponent<LevelLoop>();
        anim = GetComponent<Animator>();
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
            //Instantiate(level.citizens[Random.Range(0, level.citizens.Length - 1)], GameObject.Find("Entrance").transform.position,Quaternion.Euler(0,90,0));
            return;
        }

        if (move.move == true)
        {
            anim.SetBool("moving", true);
            /*if (move.current_velocity.magnitude <= 12)
            {
                anim.SetBool("running", true);
            }*/
        }
        else if (move.move == false)
        {
            anim.SetBool("moving", false);
        }
    }

    private void OnDestroy()
    {
        follow_path.deleteCurve();
        level.citizens.Remove(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other == GameObject.Find("Exit").GetComponent<Collider>() && action)
        {
            Destroy(gameObject);
        }
    }

    private void scape(Collider other)
    {
        move.target = GameObject.Find("Exit");
        follow_path.calcPath(move.target.transform);
        if (other == GameObject.Find("Exit").GetComponent<Collider>())
        {
            Destroy(gameObject);
        }
    }
}
}

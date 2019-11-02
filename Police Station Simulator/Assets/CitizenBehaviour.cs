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
    float timer = 3.0f;

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

        if (follow_path.arrived && !action && move.current_velocity == Vector3.zero) //create timer
        {
            anim.SetBool("moving", false);
            timer -= Time.deltaTime; //timer
            if (timer < 0) {
             
                move.target = GameObject.Find("Exit");
                follow_path.calcPath(move.target.transform);
                action = true;
                //Instantiate(level.citizens[Random.Range(0, level.citizens.Length - 1)], GameObject.Find("Entrance").transform.position,Quaternion.Euler(0,90,0));
            }
          
            return;
        }

        if (move.move == true)
        {
            anim.SetBool("moving", true);
        }
        else if (move.move == false)
        {
            anim.SetBool("running", false);
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

        if (other == GameObject.Find("Entrance").GetComponent<Collider>())
        {
            gameObject.GetComponent<SteeringCollisionAvoidance>().enabled = false;
            gameObject.GetComponent<SteeringSeparation>().enabled = false;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other == GameObject.Find("Entrance").GetComponent<Collider>())
        {
            gameObject.GetComponent<SteeringCollisionAvoidance>().enabled = true;
            gameObject.GetComponent<SteeringSeparation>().enabled = true;
        }
    }

    public void Night()
    {
        action = true;
        anim.SetBool("running", true);
        follow_path.deleteCurve();
        follow_path.calcPath(GameObject.Find("Exit").transform);
    }
}

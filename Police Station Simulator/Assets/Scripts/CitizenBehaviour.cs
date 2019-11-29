using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CitizenBehaviour : MonoBehaviour {


    public enum TypeAction
    {
        None = -1,
        Wait,
        Reception,
        Exit
    }

    public Point point = null;
    Move move;
    SteeringArrive arrive;
    public bool action;
    public Transform pivot;
    public NavMeshPath path;
    SteeringFollowPath follow_path;
    public GameObject rP;
    LevelLoop level;
    public Animator anim;
    public float timer = 3.0f;
    public TypeAction behaviour;
    public AssignPoints assign = null;
    public bool free_point = false;
    public bool night;


    // Use this for initialization
    void Awake () {
        night = false;
        move = GetComponent<Move>();
        move.move = true;
        arrive = GetComponent<SteeringArrive>();
        action = false;
        follow_path = gameObject.GetComponent<SteeringFollowPath>();
        follow_path.path = new NavMeshPath();
        rP = GameObject.Find("Reception_Point");
        free_point = rP.GetComponent<Point>().isAvailable();
        assign = GameObject.Find("Sofas").GetComponent<AssignPoints>();
        behaviour = TypeAction.None;
        level = GameObject.Find("Level").GetComponent<LevelLoop>();
        anim = GetComponent<Animator>();
    }
	
	// Update is called once per frame
	void Update ()
    {
        //Changes the Animator booleans 
        if (move.move == true)
        {
            anim.SetBool("moving", true);
        }
        else if (move.move == false)
        {
            anim.SetBool("running", false);
            move.run = false;
            anim.SetBool("moving", false);
        }
    }

    public void AssignPoint(Point newPoint)
    {
        point = newPoint;
    }

    private void OnDestroy()
    {
        follow_path.deleteCurve();
        level.citizens.Remove(gameObject);
    }

    //Triggers steering behaviours
    private void OnTriggerEnter(Collider other)
    {
        if (point != null)
        {
            if (other == point.getPoint().parent.gameObject.GetComponent<Collider>())
            {
                gameObject.layer = 0;
                follow_path.deleteCurve();
                transform.position = new Vector3(point.transform.position.x - (1.3f * point.transform.forward.x),point.transform.position.y + 0.73f, point.transform.position.z - (1.3f * point.transform.forward.z));
                anim.SetBool("sitting", true);

                move.move = false;
            }
        }

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
            gameObject.GetComponent<SteeringSeparation>().enabled = false;
        }
    }


    //Set up for night mode
    public void Night()
    {
        night = true;
        //action = true;
        //anim.SetBool("running", true);
        //move.run = true;
        //follow_path.deleteCurve();
        //follow_path.calcPath(GameObject.Find("Exit").transform);
    }
}

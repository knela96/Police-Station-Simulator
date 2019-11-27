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
    bool action;
    public Transform pivot;
    public NavMeshPath path;
    SteeringFollowPath follow_path;
    GameObject rP;
    LevelLoop level;
    Animator anim;
    float timer = 3.0f;
    public TypeAction behaviour;
    public AssignPoints assign = null;


    // Use this for initialization
    void Awake () {
        move = GetComponent<Move>();
        move.move = true;
        arrive = GetComponent<SteeringArrive>();
        action = false;
        follow_path = gameObject.GetComponent<SteeringFollowPath>();
        follow_path.path = new NavMeshPath();
        rP = GameObject.Find("Reception_Point");
        assign = GameObject.Find("Sofas").GetComponent<AssignPoints>();
        if (assign.numAssigned == 0)
        {
            rP.GetComponent<Point>().setAgent(gameObject);
            behaviour = TypeAction.Reception;
            move.target = rP;
        }
        else
        {
            behaviour = TypeAction.Wait;
            AssignPoint(behaviour);
            move.target = point.gameObject;
        }
        follow_path.calcPath(move.target.transform);
        level = GameObject.Find("Level").GetComponent<LevelLoop>();
        anim = GetComponent<Animator>();
    }
	
	// Update is called once per frame
	void Update ()
    {
        if(behaviour == TypeAction.Reception)
        {
            Vector3 distance = move.target.transform.position - transform.position;

            if (follow_path.arrived && !action && move.current_velocity == Vector3.zero)
            {
                anim.SetBool("moving", false);
                timer -= Time.deltaTime;

                if (timer < 0)
                { // timer to make the citizen wait on the desk
                    rP.GetComponent<Point>().Release();
                    move.target = GameObject.Find("Exit");
                    follow_path.calcPath(move.target.transform);
                    behaviour = TypeAction.Exit;
                    action = true;
                }
                return;
            }
        }else if (behaviour == TypeAction.Wait)
        {
            Vector3 distance = move.target.transform.position - transform.position;

            if (follow_path.arrived && !action && move.current_velocity == Vector3.zero)
            {
                anim.SetBool("moving", false);
                return;
            }

            if (rP.GetComponent<Point>().isAvailable())
            {
                move.move = true;
                anim.SetBool("moving", true);
                anim.SetBool("sitting", false);
                transform.position = new Vector3(point.transform.position.x + (1.3f * point.transform.forward.x), 0.0f, point.transform.position.z + (1.3f * point.transform.forward.z));
                behaviour = TypeAction.Reception;
                rP.GetComponent<Point>().setAgent(gameObject);
                move.target = rP;
                follow_path.calcPath(move.target.transform);
            }
        }



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

    void AssignPoint(TypeAction type)
    {
        if (TypeAction.Reception == type)
        {
            Point c_point = rP.GetComponent<Point>();
            if (c_point.isAvailable())
            {
                point = c_point.setAgent(this.gameObject);
                return;
            }
        }
        else if (TypeAction.Wait == type)
        {
            for (int i = 0; i < assign.points.Count; ++i)
            {
                Point c_point = assign.points[i].gameObject.GetComponent<Point>();
                if (c_point.isAvailable())
                {
                    point = c_point.setAgent(this.gameObject);
                    return;
                }
            }
        }
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
                //align.Steering(desk.getPoint()); //Align the entity to the direction of the desk when it arrives
                if (behaviour == TypeAction.Wait)
                {
                    gameObject.layer = 0;
                    follow_path.deleteCurve();
                    transform.position = new Vector3(point.transform.position.x - (1.3f * point.transform.forward.x),point.transform.position.y + 0.73f, point.transform.position.z - (1.3f * point.transform.forward.z));
                    anim.SetBool("sitting", true);

                    move.move = false;
                }
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
        action = true;
        anim.SetBool("running", true);
        move.run = true;
        follow_path.deleteCurve();
        follow_path.calcPath(GameObject.Find("Exit").transform);
    }
}

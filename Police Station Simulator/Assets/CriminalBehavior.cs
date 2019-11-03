using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CriminalBehavior : MonoBehaviour {

    public GameObject cells;
    Move move;
    SteeringArrive arrive;
    bool action;
    public NavMeshPath path;
    SteeringFollowPath follow_path;
    public Cell cell;
    AssignCell assign;
    GameObject c_agent;
    Animator anim;
    bool to_cell = false;
    LevelLoop level;
    float timer = 60.0f;
    
    // Use this for initialization
    void Awake()
    {
        level = GameObject.Find("Level").GetComponent<LevelLoop>();
        move = GetComponent<Move>();
        arrive = GetComponent<SteeringArrive>();
        action = false;
        follow_path = gameObject.GetComponent<SteeringFollowPath>();
        follow_path.path = new NavMeshPath();
        c_agent = Instantiate(level.policemen_prebab[Random.Range(0, 2)], new Vector3(0, 0, 0), Quaternion.identity);
        c_agent.GetComponent<Move>().target = gameObject;
        c_agent.transform.position = transform.position + Vector3.back;
        c_agent.gameObject.layer = 0;

        c_agent.GetComponent<SteeringPursue>().enabled = true;
        c_agent.GetComponent<SteeringObstacleAvoidance>().enabled = true;
        c_agent.GetComponent<SteeringCollisionAvoidance>().enabled = true;
        c_agent.GetComponent<SteeringVelocityMatching>().enabled = true;
        c_agent.GetComponent<PoliceBehaviour>().behaviour = PoliceBehaviour.TypeAction.Capture;
        c_agent.GetComponent<PoliceBehaviour>().to_cell = true;
        anim = GetComponent<Animator>();
        cells = GameObject.Find("Cell");

        level.policemen.Add(c_agent);
    }

    // Update is called once per frame
    void Update()
    {
        if (cell == null)
        {
            AssignCell();
            if (cell != null)
            {
                move.target = cell.getPoint().gameObject;
                follow_path.calcPath(cell.getPoint());
            }
        }

        if (move.move == true)
        {
            anim.SetBool("moving", true);
        }
        else if (move.move == false)
        {
            anim.SetBool("moving", false);
            anim.SetBool("running", false);
            move.run = false;
        }
        if (to_cell)
            Night();
        
    }

    void AssignCell()
    {
        assign = cells.GetComponent<AssignCell>();
        for (int i = 0; i < assign.cells.Count; ++i)
        {
            Cell c_cell = assign.cells[i].gameObject.GetComponent<Cell>();
            if (c_cell.isAvailable())
            {
                cell = c_cell.setAgent(gameObject);
                return;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other == GameObject.Find("Exit").GetComponent<Collider>())
        {
            Destroy(gameObject);
        }
        else if (other == GameObject.Find("Entrance").GetComponent<Collider>())
        {
            gameObject.GetComponent<SteeringCollisionAvoidance>().enabled = false;
            //gameObject.GetComponent<SteeringSeparation>().enabled = false;
        }
        else if (move.target != null)
        {
            if (other == move.target.gameObject.GetComponent<Collider>())
            {
                arrive_cell();
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other == GameObject.Find("Entrance").GetComponent<Collider>())
        {
            gameObject.GetComponent<SteeringCollisionAvoidance>().enabled = true;
            //gameObject.GetComponent<SteeringSeparation>().enabled = true;
        }
    }

    private void OnDestroy()
    {
        cell = null;
    }

    public void arrive_cell()
    {
        follow_path.deleteCurve();
        move.move = false;
        anim.SetBool("moving", false);
        anim.SetBool("running", false);
        move.run = false;
        anim.SetBool("sitting", true);
        if (c_agent != null)
        {
            c_agent.GetComponent<Move>().target = null;
            c_agent.GetComponent<SteeringPursue>().enabled = false;
            c_agent.GetComponent<SteeringObstacleAvoidance>().enabled = true;
            c_agent.GetComponent<SteeringCollisionAvoidance>().enabled = true;
            c_agent.GetComponent<SteeringVelocityMatching>().enabled = false;
            c_agent.GetComponent<PoliceBehaviour>().to_cell = false;
            c_agent.gameObject.layer = 8;
            c_agent = null;
        }

    }

    public void Night()
    {

        if (c_agent == null && to_cell == true)
        {
            if (level.day == false)
            {
                timer -= Time.deltaTime; //timer
                if (timer < 0)
                {
                    move.move = true;
                    anim.SetBool("sitting", false);
                    anim.SetBool("moving", true);
                    anim.SetBool("running", true);
                    move.run = true;
                    move.target = GameObject.Find("Exit");
                    follow_path.calcPath(move.target.transform);
                    to_cell = false;

                    if (cell != null)
                        cell.Release();
                }
            }
          
        }
        else
        {
            to_cell = true;
        }
    }
}
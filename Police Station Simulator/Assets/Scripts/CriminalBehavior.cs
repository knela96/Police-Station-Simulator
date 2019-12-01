using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using NodeCanvas.Framework;

public class CriminalBehavior : MonoBehaviour {

    public GameObject cells;
    Move move;
    SteeringArrive arrive;
    bool action = false;
    public NavMeshPath path;
    public SteeringFollowPath follow_path;
    public Cell cell;
    AssignCell assign;
    public GameObject c_agent;
    public Animator anim;
    public bool to_cell = false;
    public LevelLoop level;
    public float timer = 60.0f;
    public bool free = false;
    public bool countdown = false;
    public bool toExit = false;
    public bool escape = false;
    public bool night = false;
    public bool detected = false;
    public bool assigned = false;
    // Use this for initialization
    void Awake()
    {
        toExit = false;
        to_cell = true;
        countdown = false;
        assigned = false;
        level = GameObject.Find("Level").GetComponent<LevelLoop>();
        move = GetComponent<Move>();
        arrive = GetComponent<SteeringArrive>();
        action = false;
        follow_path = gameObject.GetComponent<SteeringFollowPath>();
        follow_path.path = new NavMeshPath();
        cells = GameObject.Find("Cells");
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Exit(GameObject agent)
    {

        anim.SetBool("sitting", false);
        anim.SetBool("moving", true);
        anim.SetBool("running", false);
        move.move = true;
        toExit = true;
        setAgent(agent);
    }

    public void SpawnAgent()
    {
        c_agent = Instantiate(level.policemen_prebab[Random.Range(0, 2)], new Vector3(0, 0, 0), Quaternion.identity);
        c_agent.GetComponent<PoliceBehaviour>().to_cell = true;
        c_agent.GetComponent<Move>().target = gameObject;
        c_agent.transform.position = transform.position + Vector3.back;
        c_agent.gameObject.layer = 0;
        c_agent.GetComponent<SteeringPursue>().enabled = true;
        c_agent.GetComponent<SteeringObstacleAvoidance>().enabled = true;
        c_agent.GetComponent<SteeringCollisionAvoidance>().enabled = true;
        c_agent.GetComponent<SteeringVelocityMatching>().enabled = true;
        c_agent.GetComponent<GraphOwner>().enabled = true;
        level.policemen.Add(c_agent);
    }

    public void setAgent(GameObject agent)
    {
        c_agent = agent;
        c_agent.GetComponent<Move>().target = gameObject;
        c_agent.gameObject.layer = 0;
        c_agent.GetComponent<SteeringPursue>().enabled = true;
        c_agent.GetComponent<SteeringObstacleAvoidance>().enabled = true;
        c_agent.GetComponent<SteeringCollisionAvoidance>().enabled = true;
        c_agent.GetComponent<SteeringVelocityMatching>().enabled = true;
    }

    //Assign each criminal a point to stand on the cell
    public void AssignCell()
    {
        assign = cells.GetComponent<AssignCell>();
        for (int i = 0; i < assign.cells.Count; ++i)
        {
            Cell c_cell = assign.cells[i].gameObject.GetComponent<Cell>();
            if (c_cell.isAvailable())
            {
                cell = c_cell.setAgent(gameObject);
                break;
            }
        }

        if (cell != null)
        {
            to_cell = true;
            escape = false;
            toExit = false;
            move.target = cell.getPoint().gameObject;
            follow_path.calcPath(cell.getPoint());
            timer = 60;
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
                if (c_agent != null)
                {
                    c_agent.gameObject.layer = 8;
                    c_agent.GetComponent<Move>().target = null;
                    c_agent.GetComponent<SteeringPursue>().enabled = false;
                    c_agent.GetComponent<SteeringObstacleAvoidance>().enabled = true;
                    c_agent.GetComponent<SteeringCollisionAvoidance>().enabled = true;
                    c_agent.GetComponent<SteeringVelocityMatching>().enabled = false;
                    c_agent.GetComponent<PoliceBehaviour>().to_cell = false;
                    c_agent.GetComponent<PoliceBehaviour>().detected = false;
                    c_agent = null;
                }
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other == GameObject.Find("Entrance").GetComponent<Collider>())
        {
            gameObject.GetComponent<SteeringCollisionAvoidance>().enabled = true;
        }
    }

    private void OnDestroy()
    {
        cell = null;
    }
    

    //Set up night mode

    public void Night()
    {
        night = true;
        if(!free && !c_agent)
            escape = true;
    }


    public void Day()
    {
        night = false;
        escape = false;
    }

    public void AttackTarget(int message)
    {
        Debug.Log("Received Damage");
        anim.SetBool("attack", false);
        to_cell = true;
    }
}
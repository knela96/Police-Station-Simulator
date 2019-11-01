﻿using System.Collections;
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
    public GameObject agent_prefab;
    GameObject c_agent;

    // Use this for initialization
    void Awake()
    {
        move = GetComponent<Move>();
        arrive = GetComponent<SteeringArrive>();
        action = false;
        follow_path = gameObject.GetComponent<SteeringFollowPath>();
        follow_path.path = new NavMeshPath();
        assign = cells.GetComponent<AssignCell>();
        c_agent = (GameObject)Instantiate(agent_prefab, new Vector3(0, 0, 0), Quaternion.identity);
        c_agent.GetComponent<Move>().target = gameObject;
        c_agent.transform.position = transform.position + Vector3.back * 3;
        c_agent.gameObject.layer = 0;
        c_agent.GetComponent<PoliceBehaviour>().behaviour = PoliceBehaviour.TypeAction.Capture;
    }

    // Update is called once per frame
    void Update()
    {
        if (cell == null)
        {
            AssignCell();
            move.target = cell.getPoint().gameObject;
            follow_path.calcPath(cell.getPoint());
        }

    }

    void AssignCell()
    {
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
        else if (other == move.target.gameObject.GetComponent<Collider>())
        {
            doAction();
        }
    }

    public void doAction()
    {
        c_agent.GetComponent<Move>().target = null;
        c_agent.GetComponent<SteeringPursue>().enabled = false;
        c_agent.GetComponent<SteeringObstacleAvoidance>().enabled = true;
        c_agent.GetComponent<SteeringCollisionAvoidance>().enabled = true;
        c_agent.gameObject.layer = 8;
        c_agent.GetComponent<SteeringFollowPath>().calcPath(GameObject.Find("Exit").transform);
    }
}
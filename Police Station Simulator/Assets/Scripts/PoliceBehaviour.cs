﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;
using BansheeGz.BGSpline.Curve;

public class PoliceBehaviour : MonoBehaviour {

    public enum TypeAction
    {
        None = -1,
        Investigate,
        Patrol,
        Search,
        Receptionist,
        Capture,
        Exit
    }

    public GameObject desks = null;
    AssignDesk assign = null;
    public Desk desk = null;
    float time_task = 30;
    public float cur_time = 0;
    bool start = false;
    public TypeAction behaviour;
    public Slider slider_task;
    SteeringFollowPath follow_path;
    public SteeringAlign align;
    Move move = null;
    SteeringPursue pursue;
    LevelLoop level;
    Animator animator;
    int patrol = -1;
    GameObject light = null;
    public bool to_cell = false;
    SteeringObstacleAvoidance obstacle;

    // Use this for initialization
    void Awake()
    {
        level = GameObject.Find("Level").GetComponent<LevelLoop>();
        desks = GameObject.Find("Desks");
        move = GetComponent<Move>();
        align = GetComponent<SteeringAlign>();
        follow_path = GetComponent<SteeringFollowPath>();
        assign = desks.GetComponent<AssignDesk>();
        pursue = gameObject.GetComponent<SteeringPursue>();
        obstacle = gameObject.GetComponent<SteeringObstacleAvoidance>();
        animator = GetComponent<Animator>();
        start = false;
        move.move = true;
        desk = null;
        cur_time = 0;
        light = transform.Find("Light").gameObject;
        light.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {

        if (move.move)
            animator.SetBool("moving", true);
        else
        {
            animator.SetBool("moving", false);
            animator.SetBool("running", false);
            move.run = false;
        }

        if (behaviour == TypeAction.Investigate)
        {
            if (desk == null)
            {
                //Assign desk Available and create path
                AssignDesk(behaviour);
                if (desk != null)
                {
                    move.target = desk.getPoint().gameObject;
                    follow_path.calcPath(desk.getPoint());
                }
            }

            if (start && move.current_velocity == Vector3.zero)
            {
                //if the Entity has arrived to the desk, start the Routine of "Investigating"
                animator.SetBool("moving", false);
                move.move = false;
                follow_path.deleteCurve();
                cur_time += Time.deltaTime;
                slider_task.value = cur_time / time_task; //task completion
            }
            if (cur_time >= time_task)
            {
                //Ends the task and create a path to the Exit
                stopTask();
                move.target = GameObject.Find("Exit");
                follow_path.calcPath(move.target.transform);
                cur_time = 0;
            }
        }
        else if (behaviour == TypeAction.Capture)
        {
            if (move.target == null)
            {
                if (level.day)
                {
                    behaviour = TypeAction.Investigate;
                    desk = null;
                }
                else if (!to_cell)
                    Night(patrol);
            }
            if (move.target != null)
                pursue.Steer(move.target.transform.position, move.target.GetComponent<Move>().current_velocity); //Will pursue the Criminal until it arrives to the cell
        }
        else if(behaviour == TypeAction.Receptionist)
        {
            //Assign the entity to the reception desk
            if (desk == null)
            {
                AssignDesk(behaviour);
                if (desk != null)
                {
                    move.target = desk.getPoint().gameObject;
                    follow_path.calcPath(desk.getPoint());
                }
            }

            if (start && move.current_velocity == Vector3.zero)
            {
                animator.SetBool("moving", false);
                move.move = false;
                follow_path.deleteCurve();
            }
            if (!level.day)
            {
                stopTask();
                move.target = GameObject.Find("Exit");
                follow_path.calcPath(move.target.transform);
                cur_time = 0;
            }
        }
    }

    void AssignDesk(TypeAction type)
    {
        if (TypeAction.Receptionist == type)
        {
            Desk c_desk = GameObject.Find("Reception_Point_Stand").GetComponent<Desk>();
            if (c_desk.isAvailable())
            {
                desk = c_desk.setAgent(this.gameObject);
                return;
            }
        }
        else if (TypeAction.Investigate == type)
        {
            for (int i = 0; i < assign.desks.Count; ++i)
            {
                Desk c_desk = assign.desks[i].gameObject.GetComponent<Desk>();
                if (c_desk.isAvailable())
                {
                    desk = c_desk.setAgent(this.gameObject);
                    return;
                }
            }
        }
    }

    public void startTask()
    {
        start = true;
        slider_task.gameObject.SetActive(true); //active the progress bar UI
        cur_time = 0;
    }
    public void resumeTask()
    {
        start = true;
        slider_task.gameObject.SetActive(true);
    }
    public void stopTask()
    {
        start = false;
        move.move = true;
        slider_task.gameObject.SetActive(false);
        behaviour = TypeAction.None;
        desk.Release();
        desk = null;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (desk != null)
        {
            if (other == desk.getPoint().GetComponent<Collider>())
            {
                align.Steering(desk.getPoint()); //Align the entity to the direction of the desk when it arrives
                if (behaviour == TypeAction.Investigate)
                {
                    if (cur_time < time_task && cur_time > 0)
                        resumeTask();
                    else
                        startTask();
                }
            }
        }

        if (other == GameObject.Find("Entrance").GetComponent<Collider>())
        {
            gameObject.GetComponent<SteeringCollisionAvoidance>().enabled = false;
            gameObject.GetComponent<SteeringSeparation>().enabled = false;
        }

        if (other == GameObject.Find("Exit").GetComponent<Collider>())
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (desk != null)
        {
            if (other == desk.getPoint().GetComponent<Collider>())
            {
                if (follow_path.arrived)
                {
                    align.Steering(desk.getPoint());
                }
            }
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

    public void Night(int assign_patrol)
    {
        //Assign the routine on the Night Cycle
        patrol = assign_patrol;
        if (desk != null)
        {
            stopTask();
        }

        if (!to_cell) //if is not escorting any criminal assign the patrol depending on its availability
        {
            if (patrol < 2 && patrol >= 0)
            {
                behaviour = TypeAction.Patrol;
                follow_path.createPatrol(patrol,false); //Create a path to the start of the patrol path
                move.move = true;
                light.SetActive(true); //If is patrolling activate a Torchlight
            }
            else
            {
                behaviour = TypeAction.None;
                follow_path.calcPath(GameObject.Find("Exit").transform);
            }
        }
        else
        {
            to_cell = true;
        }
    }


    public void Day()
    {
        light.SetActive(false); //Turn off Torchlight
        if (follow_path.patroling)
        {
            behaviour = TypeAction.None;
            move.target = GameObject.Find("Exit");
            follow_path.calcPath(move.target.transform);
        }
    }

    private void OnDestroy()
    {
        if(desk != null)
            desk.Release();
    }
}
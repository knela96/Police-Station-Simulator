using System.Collections;
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

    public GameObject desks;
    AssignDesk assign;
    public Desk desk;
    float time_task = 30;
    public float cur_time;
    bool start;
    public TypeAction behaviour;
    public Slider slider_task;
    SteeringFollowPath follow_path;
    public SteeringAlign align;
    Move move;
    SteeringPursue pursue;
    LevelLoop level;
    Animator animator;
    int patrol = -1;
    public bool to_cell = false;
    public GameObject light;
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
                cur_time += Time.deltaTime;
                slider_task.value = cur_time / time_task;
            }
            if (cur_time >= time_task)
            {
                stopTask();
                move.target = GameObject.Find("Exit");
                follow_path.calcPath(move.target.transform);
                cur_time = 0;
            }
        }
        else if (behaviour == TypeAction.Patrol)
        {

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
                pursue.Steer(move.target.transform.position, move.target.GetComponent<Move>().current_velocity);
        }
        else if(behaviour == TypeAction.Receptionist)
        {
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
        slider_task.gameObject.SetActive(true);
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
                align.Steering(desk.getPoint());
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
        patrol = assign_patrol;
        if (desk != null)
        {
            stopTask();
        }

        if (!to_cell)
        {
            if (patrol < 2 && patrol >= 0)
            {
                behaviour = TypeAction.Patrol;
                follow_path.createPatrol(patrol,false);
                move.move = true;
                light.SetActive(true);
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
        light.SetActive(false);
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;
using BansheeGz.BGSpline.Curve;

public class PoliceBehaviour : MonoBehaviour {

    public enum TypeAction
    {
        Investigate,
        Patrol,
        Search,
        Capture
    }

    public GameObject desks;
    AssignDesk assign;
    public Desk desk;
    float time_task = 30;
    public float cur_time;
    bool start;
    public TypeAction behaviour;
    public Slider slider_task;
    private GameObject curve;
    SteeringFollowPath follow_path;
    public SteeringAlign align;
    Move move;
    SteeringPursue pursue;

    // Use this for initialization
    void Start () {

    }

    void Awake()
    {
        desks = GameObject.Find("Desks");
        move = gameObject.GetComponent<Move>();
        align = gameObject.GetComponent<SteeringAlign>();
        follow_path = gameObject.GetComponent<SteeringFollowPath>();
        assign = desks.GetComponent<AssignDesk>();
        pursue = gameObject.GetComponent<SteeringPursue>();
        follow_path.path = new NavMeshPath();
        start = false;
        move.move = true;
        cur_time = 0;
    }

    // Update is called once per frame
    void Update () {

        if (behaviour == TypeAction.Investigate)
        {
            if (desk == null)
            {
                AssignDesk();
                move.target = desk.getPoint().gameObject;
                follow_path.calcPath(desk.getPoint());
            }

            if (start && move.current_velocity == Vector3.zero)
            {
                cur_time += Time.deltaTime;
                slider_task.value = cur_time/time_task;
            }
            if (cur_time >= time_task)
            {
                stopTask();
                move.target = GameObject.Find("Exit");
                follow_path.calcPath(move.target.transform);
                cur_time = 0;
            }
        }else if (behaviour == TypeAction.Patrol)
        {

        }
        else if (behaviour == TypeAction.Capture)
        {
            if (move.target == null)
                behaviour = TypeAction.Investigate;
            pursue.Steer(move.target.transform.position, move.target.GetComponent<Move>().current_velocity);
        }

    }

    void AssignDesk()
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

    public void startTask()
    {
        start = true;
        move.move = false;
        slider_task.gameObject.SetActive(true);
        cur_time = 0;
    }
    public void resumeTask()
    {
        start = true;
        move.move = false;
        slider_task.gameObject.SetActive(true);
    }
    public void stopTask()
    {
        start = false;
        move.move = true;
        slider_task.gameObject.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other == desk.getPoint().GetComponent<Collider>())
        {
            align.Steering(desk.getPoint());
            if (cur_time < time_task && cur_time > 0)
                resumeTask();
            else
                startTask();
        }
        else if (other == GameObject.Find("Exit").GetComponent<Collider>())
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other == desk.getPoint().GetComponent<Collider>())
        {
            align.Steering(desk.getPoint());
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other == desk.getPoint().GetComponent<Collider>())
            stopTask();
    }

    
}

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

    public GameObject desks = null;
    AssignDesk assign = null;
    public Desk desk = null;
    //float time_task = 30;
    //public float cur_time = 0;
    public bool start = false;
    public Slider slider_task;
    SteeringFollowPath follow_path;
    public SteeringAlign align;
    public Move move = null;
    SteeringPursue pursue;
    LevelLoop level;
    public Animator animator;
    public int patrol = -1;
    public GameObject light = null;
    public bool to_cell = false;
    SteeringObstacleAvoidance obstacle;
    public bool night;
    public bool receptionist = false;
    public bool patrolling = false;
    public bool detected = false;
    public int numCriminals = 0;

    // Use this for initialization
    void Awake()
    {
        night = false;
        detected = false;
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
        //cur_time = 0;
        light = transform.Find("Light").gameObject;
        light.SetActive(false);
        if (level.receptionist == null)
        {
            receptionist = true;
            level.receptionist = gameObject;
        }
        else
            receptionist = false;

    }

    // Update is called once per frame
    void Update()
    {}

    public bool ArrivedDesk()
    {
        if (start && move.current_velocity == Vector3.zero)
        {
            animator.SetBool("moving", false);
            move.move = false;
            follow_path.deleteCurve();
            return true;
        }
        return false;
    }

    public void AssignDesk()
    {
        if (receptionist)
        {
            Desk c_desk = GameObject.Find("Reception_Point_Stand").GetComponent<Desk>();
            if (c_desk.isAvailable())
            {
                desk = c_desk.setAgent(this.gameObject);
                return;
            }
        }
        else
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
                startTask();
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
        night = true;
        //Assign the routine on the Night Cycle
        patrol = assign_patrol;
        if (desk != null)
        {
            stopTask();
        }
        
        if (patrol < 2 && patrol != -1)
        {
            patrolling = true;
        }
        else
        {
            patrolling = false;
            //behaviour = TypeAction.None;
            follow_path.calcPath(GameObject.Find("Exit").transform);
        }
    }


    public void Day()
    {
        night = false;
        light.SetActive(false); //Turn off Torchlight
    }

    private void OnDestroy()
    {
        if(desk != null)
            desk.Release();
    }
}

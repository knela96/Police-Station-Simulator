using UnityEngine;
using UnityEngine.AI;

using NodeCanvas.Framework;
using ParadoxNotion.Design;

namespace NodeCanvas.Tasks.Actions
{

    [Name("Wait Action")]
    [Category("Citizen")]
    public class WaitAction : ActionTask
    {
        CitizenBehaviour citizen;
        GameObject rP;
        Point point = null;
        Move move;
        SteeringFollowPath follow_path;
        AssignPoints assign;
        Animator anim;
        Point c_point;
        bool action;
        //Use for initialization. This is called only once in the lifetime of the task.
        //Return null if init was successfull. Return an error string otherwise
        protected override string OnInit()
        {
            move = agent.gameObject.GetComponent<Move>();
            move.move = true;
            citizen = agent.gameObject.GetComponent<CitizenBehaviour>();
            rP = GameObject.Find("Reception_Point");
            assign = GameObject.Find("Sofas").GetComponent<AssignPoints>();
            follow_path = agent.gameObject.GetComponent<SteeringFollowPath>();
            follow_path.path = new NavMeshPath();
            anim = agent.gameObject.GetComponent<Animator>();
            action = false;
            return null;
        }

        //This is called once each time the task is enabled.
        //Call EndAction() to mark the action as finished, either in success or failure.
        //EndAction can be called from anywhere.
        protected override void OnExecute()
        {
            if (point == null)
            {
                for (int i = 0; i < assign.points.Count; ++i)
                {
                    c_point = assign.points[i].gameObject.GetComponent<Point>();
                    if (c_point.isAvailable())
                    {
                        point = c_point.setAgent(agent.gameObject);
                        citizen.AssignPoint(point);
                        move.target = point.gameObject;
                        follow_path.calcPath(move.target.transform);
                        //EndAction(true);
                        return;
                    }
                }
            }
        }

        //Called once per frame while the action is active.
        protected override void OnUpdate()
        {
            
            Vector3 distance = move.target.transform.position - agent.gameObject.transform.position;
            citizen.free_point = c_point.isAvailable();

            if (follow_path.arrived && !action && move.current_velocity == Vector3.zero)
            {
                anim.SetBool("moving", false);
                return;
            }
        }

        //Called when the task is disabled.
        protected override void OnStop()
        {
            move.move = true;
            if(anim.GetBool("sitting"))
                agent.gameObject.transform.position = new Vector3(point.transform.position.x + (1.3f * point.transform.forward.x), 0.0f, point.transform.position.z + (1.3f * point.transform.forward.z));
            anim.SetBool("moving", true);
            anim.SetBool("sitting", false);
            follow_path.deleteCurve();
            c_point.Release();
        }

        //Called when the task is paused.
        protected override void OnPause()
        {

        }
    }
}
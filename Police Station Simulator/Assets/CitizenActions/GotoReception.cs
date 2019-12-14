using UnityEngine;
using UnityEngine.AI;

using NodeCanvas.Framework;
using ParadoxNotion.Design;

namespace NodeCanvas.Tasks.Actions{

    [Name("Go to Reception")]
    [Category("Citizen")]
    public class GotoReception : ActionTask{
        CitizenBehaviour citizen;
        GameObject rP;
        Point point;
        Move move;
        SteeringFollowPath follow_path;
        AssignPoints assign;
        Animator anim;
        bool action;
        public float timer;
        //Use for initialization. This is called only once in the lifetime of the task.
        //Return null if init was successfull. Return an error string otherwise
        protected override string OnInit()
        {
            move = agent.gameObject.GetComponent<Move>();
            move.move = true;
            action = false;
            citizen = agent.gameObject.GetComponent<CitizenBehaviour>();
            rP = GameObject.Find("Reception_Point");
            assign = GameObject.Find("Sofas").GetComponent<AssignPoints>();
            follow_path = agent.gameObject.GetComponent<SteeringFollowPath>();
            anim = agent.gameObject.GetComponent<Animator>();
            return null;
		}

		//This is called once each time the task is enabled.
		//Call EndAction() to mark the action as finished, either in success or failure.
		//EndAction can be called from anywhere.
		protected override void OnExecute()
        {
            Point c_point = rP.GetComponent<Point>();
            if (c_point.isAvailable())
            {
                agent.gameObject.layer = 8;
                timer = 3.0f;
                point = c_point.setAgent(agent.gameObject);
                citizen.AssignPoint(point);
                move.target = point.gameObject;
                follow_path.calcPath(move.target.transform);
            }
            else
                EndAction(false);
        }

		//Called once per frame while the action is active.
		protected override void OnUpdate(){
            Vector3 distance = move.target.transform.position - agent.gameObject.transform.position;

            if (follow_path.arrived && !action && move.current_velocity == Vector3.zero)
            {
                anim.SetBool("moving", false);
                timer -= Time.deltaTime;

                if (timer < 0)
                { // timer to make the citizen wait on the desk
                    citizen.action = true;
                    rP.GetComponent<Point>().Release();
                    EndAction(true);
                }
                return;
            }
        }

		//Called when the task is disabled.
		protected override void OnStop(){
            follow_path.deleteCurve();
            rP.GetComponent<Point>().Release();

        }

		//Called when the task is paused.
		protected override void OnPause(){
			
		}
	}
}
using UnityEngine;
using UnityEngine.AI;

using NodeCanvas.Framework;
using ParadoxNotion.Design;

namespace NodeCanvas.Tasks.Actions
{

    [Name("Freedom")]
    [Category("Criminal")]
    public class Freedom : ActionTask
    {
        CriminalBehavior criminal;
        public BBParameter<bool> Night;
        Point point;
        Move move;
        SteeringFollowPath follow_path;
        float timer;
        //Use for initialization. This is called only once in the lifetime of the task.
        //Return null if init was successfull. Return an error string otherwise
        protected override string OnInit()
        {
            criminal = agent.gameObject.GetComponent<CriminalBehavior>();
            move = agent.gameObject.GetComponent<Move>();
            move.move = true;
            follow_path = agent.gameObject.GetComponent<SteeringFollowPath>();
            //follow_path.path = new NavMeshPath();
            return null;
        }

        //This is called once each time the task is enabled.
        //Call EndAction() to mark the action as finished, either in success or failure.
        //EndAction can be called from anywhere.
        protected override void OnExecute()
        {
            criminal.anim.SetBool("moving", true);
            move.target = GameObject.Find("Exit");
            follow_path.calcPath(move.target.transform);
        }

        //Called once per frame while the action is active.
        protected override void OnUpdate()
        {
                
        }

        //Called when the task is disabled.
        protected override void OnStop()
        {

        }

        //Called when the task is paused.
        protected override void OnPause()
        {

        }
    }
}
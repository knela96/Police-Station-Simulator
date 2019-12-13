using UnityEngine;
using UnityEngine.AI;

using NodeCanvas.Framework;
using ParadoxNotion.Design;

namespace NodeCanvas.Tasks.Actions
{

    [Name("Go to Exit Police")]
    [Category("Police")]
    public class GotoExitPolice : ActionTask
    {
        PoliceBehaviour police;
        public BBParameter<bool> Night;
        Point point;
        Move move;
        SteeringFollowPath follow_path;
        SteeringSeek seek;
        public float timer;
        bool _seek = false;
        //Use for initialization. This is called only once in the lifetime of the task.
        //Return null if init was successfull. Return an error string otherwise
        protected override string OnInit()
        {
            police = agent.gameObject.GetComponent<PoliceBehaviour>();
            move = agent.gameObject.GetComponent<Move>();
            move.move = true;
            follow_path = agent.gameObject.GetComponent<SteeringFollowPath>();
            seek = agent.gameObject.GetComponent<SteeringSeek>();
            //follow_path.path = new NavMeshPath();
            return null;
        }

        //This is called once each time the task is enabled.
        //Call EndAction() to mark the action as finished, either in success or failure.
        //EndAction can be called from anywhere.
        protected override void OnExecute()
        {
            move.move = true;
            move.run = false;
            police.animator.SetBool("running", false);
            police.animator.SetBool("moving", true);
            move.target = GameObject.Find("Exit");
            if ((move.target.transform.position - agent.transform.position).magnitude <= 10)
            {
                _seek = true;
            }
            else
            {
                _seek = false;
                follow_path.calcPath(move.target.transform);
            }
        }

        //Called once per frame while the action is active.
        protected override void OnUpdate()
        {
        //    if (Night.value)
        //    {
        //        move.run = true;
        //        if(!police.patrolling)
        //            police.animator.SetBool("running", true);
        //    }
            if (_seek)
                seek.Steer(move.target.transform.position, 5); //Will pursue the Criminal until it arrives to the cell
            else if (!follow_path.followingPath())
            {
                follow_path.deleteCurve();
                follow_path.calcPath(move.target.transform);
            }

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
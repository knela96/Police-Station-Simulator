using UnityEngine;
using UnityEngine.AI;

using NodeCanvas.Framework;
using ParadoxNotion.Design;

namespace NodeCanvas.Tasks.Actions
{

    [Name("toPatrol")]
    [Category("Police")]
    public class toPatrol : ActionTask
    {
        PoliceBehaviour police;
        public BBParameter<bool> Night;
        Point point;
        Move move;
        SteeringFollowPath follow_path;
        public float timer;
        //Use for initialization. This is called only once in the lifetime of the task.
        //Return null if init was successfull. Return an error string otherwise
        protected override string OnInit()
        {
            police = agent.gameObject.GetComponent<PoliceBehaviour>();
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
            police.animator.SetBool("running", false);
            follow_path.createPatrol(police.patrol, false); //Create a path to the start of the patrol path
            move.move = true;
            move.run = false;
            police.light.SetActive(true); //If is patrolling activate a Torchlight
        }

        //Called once per frame while the action is active.
        protected override void OnUpdate()
        {
            
        }

        //Called when the task is disabled.
        protected override void OnStop()
        {
            follow_path.deleteCurve();
        }

        //Called when the task is paused.
        protected override void OnPause()
        {

        }
    }
}
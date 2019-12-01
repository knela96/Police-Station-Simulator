using UnityEngine;
using UnityEngine.AI;

using NodeCanvas.Framework;
using ParadoxNotion.Design;

namespace NodeCanvas.Tasks.Actions
{

    [Name("Follow")]
    [Category("Police")]
    public class Follow : ActionTask
    {
        BBParameter<GameObject> target;
        PoliceBehaviour police;
        Point point;
        Move move;
        SteeringPursue pursue;
        //Use for initialization. This is called only once in the lifetime of the task.
        //Return null if init was successfull. Return an error string otherwise
        protected override string OnInit()
        {
            police = agent.gameObject.GetComponent<PoliceBehaviour>();
            pursue = agent.gameObject.GetComponent<SteeringPursue>();
            move = agent.gameObject.GetComponent<Move>();
            //follow_path.path = new NavMeshPath();
            return null;
        }

        //This is called once each time the task is enabled.
        //Call EndAction() to mark the action as finished, either in success or failure.
        //EndAction can be called from anywhere.
        protected override void OnExecute()
        {
            move.move = true;
            police.detected = true;
            police.animator.SetBool("moving", true);
        }

        //Called once per frame while the action is active.
        protected override void OnUpdate()
        {
            if (move.target != null)
            {
                if (move.target.GetComponent<CriminalBehavior>().anim.GetBool("Sitting"))
                    EndAction(true);

                pursue.Steer(move.target.transform.position, move.target.GetComponent<Move>().current_velocity); //Will pursue the Criminal until it arrives to the cell
            }
            else
                EndAction(false);
        }

        //Called when the task is disabled.
        protected override void OnStop()
        {
            move.resetAccelerationRotation();
        }

        //Called when the task is paused.
        protected override void OnPause()
        {

        }
    }
}
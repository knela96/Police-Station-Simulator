using UnityEngine;
using UnityEngine.AI;

using NodeCanvas.Framework;
using ParadoxNotion.Design;

namespace NodeCanvas.Tasks.Actions
{

    [Name("Attack")]
    [Category("Police")]
    public class Attack : ActionTask
    {
        public BBParameter<GameObject> target;
        PoliceBehaviour police;
        Point point;
        Move move;
        SteeringPursue pursue;
        public LayerMask mask;
        bool inside = false;
        SteeringFaceHeading face_heading;
        SteeringFollowPath follow_path;

        //Use for initialization. This is called only once in the lifetime of the task.
        //Return null if init was successfull. Return an error string otherwise
        protected override string OnInit()
        {
            police = agent.gameObject.GetComponent<PoliceBehaviour>();
            pursue = agent.gameObject.GetComponent<SteeringPursue>();
            move = agent.gameObject.GetComponent<Move>();
            face_heading = agent.GetComponent<SteeringFaceHeading>();
            follow_path = agent.gameObject.GetComponent<SteeringFollowPath>();
            move.move = true;
            //follow_path.path = new NavMeshPath();
            return null;
        }

        //This is called once each time the task is enabled.
        //Call EndAction() to mark the action as finished, either in success or failure.
        //EndAction can be called from anywhere.
        protected override void OnExecute()
        {
            if (follow_path.going)
                follow_path.deleteCurve();
            police.GetComponent<SteeringAlign>().enabled = false;
            police.detected = true;
            police.animator.SetBool("moving", true);
            police.animator.SetBool("attack", false);
            police.animator.SetBool("running", false);
            move.run = false;
            move.target = target.value;
            move.resetAccelerationRotation();
        }

        //Called once per frame while the action is active.
        protected override void OnUpdate()
        {
            if ((!police.detected && !police.stun) || police.to_cell)
                EndAction(true);
            else if (police.stun)
                return;
            else if (move.target == null)
            {
                police.detected = false;
                police.to_exit = true;
            }
            Collider[] colliders = Physics.OverlapSphere(agent.transform.position, 2, mask);
            inside = false;
            for (int i = 0; i < colliders.Length; ++i)
            {
                GameObject entity = colliders[i].gameObject;

                //Accelerate the target with the curve form
                if (entity != agent.gameObject && entity == move.target && !inside)
                {
                    move.move = false;
                    police.animator.SetBool("running", false);
                    police.animator.SetBool("moving", false);
                    police.animator.SetBool("attack", true);
                    inside = true;
                }
            }

            if (move.target != null && !inside && !police.stun)
            {
                pursue.Steer(move.target.transform.position, move.target.GetComponent<Move>().current_velocity); //Will pursue the Criminal to where he is
                move.move = true;
                move.run = true;
                police.animator.SetBool("running", true);
                face_heading.Steer(move.target.transform.position);
            }
        }


        //Called when the task is disabled.
        protected override void OnStop()
        {
            police.GetComponent<AIPerceptionManager>().target = null;
            if (!police.stun)
            {
                police.GetComponent<AIPerceptionManager>().player_detected = false;
                police.detected = false;
            }
            police.GetComponent<SteeringAlign>().enabled = true;
            move.run = false;
            inside = false;
            police.animator.SetBool("attack", false);
            police.animator.SetBool("running", false);
            police.animator.SetBool("moving", true);
        }

        //Called when the task is paused.
        protected override void OnPause()
        {

        }
    }
}
using UnityEngine;
using UnityEngine.AI;

using NodeCanvas.Framework;
using ParadoxNotion.Design;

namespace NodeCanvas.Tasks.Actions
{

    [Name("Attack Criminal")]
    [Category("Criminal")]
    public class AttackCriminal : ActionTask
    {
        CriminalBehavior criminal;
        Move move;
        SteeringFaceHeading face_heading;
        public LayerMask mask;
        //Use for initialization. This is called only once in the lifetime of the task.
        //Return null if init was successfull. Return an error string otherwise
        protected override string OnInit()
        {
            criminal = agent.gameObject.GetComponent<CriminalBehavior>();
            move = agent.gameObject.GetComponent<Move>();
            face_heading = agent.GetComponent<SteeringFaceHeading>();
            move.move = true;
            //follow_path.path = new NavMeshPath();
            return null;
        }

        //This is called once each time the task is enabled.
        //Call EndAction() to mark the action as finished, either in success or failure.
        //EndAction can be called from anywhere.
        protected override void OnExecute()
        {
            move.move = false;
            move.run = false;
            move.resetAccelerationRotation();
            criminal.anim.SetBool("running", false);
            criminal.anim.SetBool("moving", false);
            criminal.GetComponent<SteeringAlign>().enabled = false;
        }

        //Called once per frame while the action is active.
        protected override void OnUpdate()
        {

            Collider[] colliders = Physics.OverlapSphere(agent.transform.position, 2, mask);

            for (int i = 0; i < colliders.Length; ++i)
            {
                GameObject entity = colliders[i].gameObject;

                //Accelerate the target with the curve form
                if (entity != agent.gameObject)
                {
                    criminal.anim.SetBool("attack", true);
                    face_heading.Steer(entity.transform.position);
                }
            }
        }


        //Called when the task is disabled.
        protected override void OnStop()
        {
            criminal.anim.SetBool("attack", false);
            criminal.GetComponent<SteeringAlign>().enabled = false;
        }

        //Called when the task is paused.
        protected override void OnPause()
        {

        }
    }
}
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

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
        GameObject police_GO = null;
        bool inside = false;
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
            inside = false;
            move.move = false;
            move.run = false;
            criminal.action = false;
            move.resetAccelerationRotation();
            criminal.anim.SetBool("running", false);
            criminal.anim.SetBool("moving", false);
            criminal.anim.SetBool("sitting", false);
            criminal.GetComponent<SteeringAlign>().enabled = false;
            criminal.attack_icon.gameObject.SetActive(true);
        }

        //Called once per frame while the action is active.
        protected override void OnUpdate()
        {
            criminal.attack_icon.GetComponent<Image>().sprite = criminal.sprite5;
            Collider[] colliders = Physics.OverlapSphere(agent.transform.position, 2, mask);

            for (int i = 0; i < colliders.Length; ++i)
            {
                GameObject entity = colliders[i].gameObject;

                //Accelerate the target with the curve form
                if (entity != agent.gameObject && !criminal.captured && entity.GetComponent<PoliceBehaviour>() != null)
                {
                    criminal.anim.SetBool("attack", true);
                    face_heading.Steer(entity.transform.position);
                    if (!inside)
                    {
                        criminal.action = false;
                        criminal.to_cell = false;
                        criminal.c_agent = entity;
                        criminal.StartAttack();
                    }
                    inside = true;
                }
            }

            if (criminal.action)//WON CRIMINAL
            {
                criminal.detected = false;
                criminal.escape = true;
                if (criminal.c_agent != null)
                {
                    PoliceBehaviour police = criminal.c_agent.GetComponent<PoliceBehaviour>();
                    police.StunPolice();
                }
                criminal.c_agent = null;
                criminal.attack_icon.GetComponent<Image>().sprite = criminal.sprite1;
                EndAction(true);
            }
            if (criminal.to_cell)
            {
                criminal.attack_icon.GetComponent<Image>().sprite = criminal.sprite2;
                criminal.captured = true;
                criminal.c_agent.GetComponent<PoliceBehaviour>().criminal2Cell();
                criminal.attack_icon.GetComponent<Image>().sprite = criminal.sprite2;
                EndAction(true);
            }
        }


        //Called when the task is disabled.
        protected override void OnStop()
        {
            criminal.anim.SetBool("attack", false);
            criminal.action = false;
            criminal.GetComponent<SteeringAlign>().enabled = false;
        }

        //Called when the task is paused.
        protected override void OnPause()
        {

        }
    }
}
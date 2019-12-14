using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

using NodeCanvas.Framework;
using ParadoxNotion.Design;

namespace NodeCanvas.Tasks.Actions
{

    [Name("Escape")]
    [Category("Criminal")]
    public class Escape : ActionTask
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
            return null;
        }

        //This is called once each time the task is enabled.
        //Call EndAction() to mark the action as finished, either in success or failure.
        //EndAction can be called from anywhere.
        protected override void OnExecute()
        {
            if (criminal.cell != null)
            {
                criminal.cell.Release();
                criminal.cell = null;
            }
            if (Night.value)
            {
                move.run = true;
                criminal.anim.SetBool("running", true);
            }
            criminal.anim.SetBool("sitting", false);
            criminal.anim.SetBool("moving", true);
            move.target = GameObject.Find("Exit");
            follow_path.calcPath(move.target.transform);
            criminal.attack_icon.gameObject.SetActive(true);
            criminal.attack_icon.GetComponent<Image>().sprite = criminal.sprite1;
        }

        //Called once per frame while the action is active.
        protected override void OnUpdate()
        {
            

        }

        //Called when the task is disabled.
        protected override void OnStop()
        {
            move.run = false;
            criminal.anim.SetBool("running", false);
        }

        //Called when the task is paused.
        protected override void OnPause()
        {

        }
    }
}
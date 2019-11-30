using UnityEngine;
using NodeCanvas.Framework;
using ParadoxNotion.Design;


namespace NodeCanvas.Tasks.Actions
{

    [Name("Free Criminal")]
    [Category("Police")]
    public class FreeCriminal : ActionTask
    {

        PoliceBehaviour police;
        bool receptionist;
        SteeringFollowPath follow_path;
        LevelLoop level;

        //Use for initialization. This is called only once in the lifetime of the task.
        //Return null if init was successfull. Return an error string otherwise
        protected override string OnInit()
        {
            police = agent.gameObject.GetComponent<PoliceBehaviour>();
            follow_path = agent.gameObject.GetComponent<SteeringFollowPath>();
            level = GameObject.Find("Level").GetComponent<LevelLoop>();
            return null;
        }

        //This is called once each time the task is enabled.
        //Call EndAction() to mark the action as finished, either in success or failure.
        //EndAction can be called from anywhere.
        protected override void OnExecute()
        {
            police.animator.SetBool("moving", true);
            police.move.target = level.getCriminal();
            if (police.move.target != null)
            {
                follow_path.calcPath(police.move.target.transform);
            }
            else
                EndAction(false);
        }

        //Called once per frame while the action is active.
        protected override void OnUpdate()
        {
            if (follow_path.arrived)
            {
                police.move.target.GetComponent<CriminalBehavior>().Exit(agent.gameObject);
                police.to_cell = true;
                follow_path.deleteCurve();
                EndAction(true);
            }
    
        }

        //Called when the task is disabled.
        protected override void OnStop()
        {
            if (follow_path.arrived)
                police.animator.SetBool("moving", false);
        }

        //Called when the task is paused.
        protected override void OnPause()
        {

        }
    }
}
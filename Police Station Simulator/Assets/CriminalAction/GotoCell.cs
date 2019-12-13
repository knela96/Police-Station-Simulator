using UnityEngine;
using UnityEngine.AI;

using NodeCanvas.Framework;
using ParadoxNotion.Design;

namespace NodeCanvas.Tasks.Actions
{

    [Name("Go to Cell")]
    [Category("Criminal")]
    public class GotoCell : ActionTask
    {
        CriminalBehavior criminal;
        Cell point;
        Move move;
        SteeringFollowPath follow_path;

        //Use for initialization. This is called only once in the lifetime of the task.
        //Return null if init was successfull. Return an error string otherwise
        protected override string OnInit()
        {
            criminal = agent.gameObject.GetComponent<CriminalBehavior>();
            move = agent.gameObject.GetComponent<Move>();
            follow_path = agent.gameObject.GetComponent<SteeringFollowPath>();
            criminal.SpawnAgent();
            move.move = true;
            return null;
        }

        //This is called once each time the task is enabled.
        //Call EndAction() to mark the action as finished, either in success or failure.
        //EndAction can be called from anywhere.
        protected override void OnExecute()
        {
            //Agent created to scort him to the cell, creates one each time a new criminal appears.
            criminal.AssignCell();
            criminal.to_cell = true;
            move.move = true;
            criminal.anim.SetBool("moving", true);
        }

        //Called once per frame while the action is active.
        protected override void OnUpdate()
        {
            if (follow_path.arrived)
            {
                criminal.countdown = true;
                EndAction(true);
            }
            if(!follow_path.followingPath())
                follow_path.calcPath(criminal.cell.getPoint());


        }

        //Called when the task is disabled.
        protected override void OnStop()
        {
            //FERRAN ADD MONEY

            follow_path.deleteCurve();
            move.move = false;
            criminal.anim.SetBool("moving", false);
            criminal.anim.SetBool("running", false);
            move.run = false;
            criminal.anim.SetBool("sitting", true);
            if (criminal.c_agent != null)
            {
                criminal.c_agent.GetComponent<Move>().target = null;
                criminal.c_agent.GetComponent<SteeringPursue>().enabled = false;
                criminal.c_agent.GetComponent<SteeringObstacleAvoidance>().enabled = true;
                criminal.c_agent.GetComponent<SteeringCollisionAvoidance>().enabled = true;
                criminal.c_agent.GetComponent<SteeringVelocityMatching>().enabled = false;
                criminal.c_agent.GetComponent<PoliceBehaviour>().to_cell = false;
                criminal.c_agent.GetComponent<PoliceBehaviour>().detected = false;
                criminal.c_agent.gameObject.layer = 8;
                criminal.c_agent = null;
            }
        }

        //Called when the task is paused.
        protected override void OnPause()
        {

        }
    }
}
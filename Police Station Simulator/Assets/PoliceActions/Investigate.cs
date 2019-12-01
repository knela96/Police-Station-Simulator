using UnityEngine;
using NodeCanvas.Framework;
using ParadoxNotion.Design;


namespace NodeCanvas.Tasks.Actions
{

    [Name("Investigate")]
    [Category("Police")]
    public class Investigate : ActionTask
    {
        PoliceBehaviour police;
        Desk desk;
        bool receptionist;
        SteeringFollowPath follow_path;
        float time_task = 30;
        public float cur_time = 0;
        LevelLoop level;
        MoneyBar money;
        float auxm;
        //Use for initialization. This is called only once in the lifetime of the task.
        //Return null if init was successfull. Return an error string otherwise
        protected override string OnInit()
        {
            police = agent.gameObject.GetComponent<PoliceBehaviour>();
            follow_path = agent.gameObject.GetComponent<SteeringFollowPath>();
            level = GameObject.Find("Level").GetComponent<LevelLoop>();
            money = GameObject.Find("Money").GetComponent<MoneyBar>();
            return null;
        }

        //This is called once each time the task is enabled.
        //Call EndAction() to mark the action as finished, either in success or failure.
        //EndAction can be called from anywhere.
        protected override void OnExecute()
        {
            cur_time = 0;
        }

        //Called once per frame while the action is active.
        protected override void OnUpdate()
        {
            if (police.start)
            {
                if (police.move.current_velocity == Vector3.zero)
                {
                    //if the Entity has arrived to the desk, start the Routine of "Investigating"
                    police.animator.SetBool("moving", false);
                    police.move.move = false;
                    follow_path.deleteCurve();
                    cur_time += Time.deltaTime;
                    police.slider_task.value = cur_time / time_task; //task completion
                }
                if (cur_time >= time_task)
                {
                    //Ends the task and create a path to the Exit
                    police.numCriminals = level.criminals.Count;
                    auxm = money.CurrentValue;
                    auxm = auxm + 7;
                    money.SetBar((int)auxm);
                    EndAction(true);
                    cur_time = 0;
                }
            }
        }

        //Called when the task is disabled.
        protected override void OnStop()
        {
            level.addCriminal();
            police.stopTask();
            police.animator.SetBool("moving", true);
        }

        //Called when the task is paused.
        protected override void OnPause()
        {

        }
    }
}
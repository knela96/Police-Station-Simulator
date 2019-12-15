﻿using UnityEngine;
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
            time_task = Random.Range(20,60);
            cur_time = 0;
            police.gameObject.layer = 0;
            police.move.move = false;
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
                    if (!level.addCriminal())
                        level.addPolicemen();
                    police.updateMoney(40);//adds 7
                    EndAction(true);
                    cur_time = 0;
                    if(police.numCriminals == 0)
                        police.to_exit = true;
                }
            }
        }

        //Called when the task is disabled.
        protected override void OnStop()
        {
            police.gameObject.layer = 8;
            police.move.move = true;
            police.stopTask();
            if (desk != null)
                desk.Release();
            desk = null;
            police.animator.SetBool("moving", true);
        }

        //Called when the task is paused.
        protected override void OnPause()
        {
            if (desk != null)
                desk.Release();
            desk = null;
        }
    }
}
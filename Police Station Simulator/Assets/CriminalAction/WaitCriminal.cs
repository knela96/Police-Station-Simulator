using UnityEngine;
using NodeCanvas.Framework;
using ParadoxNotion.Design;


namespace NodeCanvas.Tasks.Actions
{

    [Name("Wait in Cell")]
    [Category("Criminal")]
    public class WaitCriminal : ActionTask
    {
        public BBParameter<bool> Night;
        CriminalBehavior criminal;
        Move move;

        //Use for initialization. This is called only once in the lifetime of the task.
        //Return null if init was successfull. Return an error string otherwise
        protected override string OnInit()
        {
            criminal = agent.gameObject.GetComponent<CriminalBehavior>();
            move = criminal.GetComponent<Move>();
            return null;
        }

        //This is called once each time the task is enabled.
        //Call EndAction() to mark the action as finished, either in success or failure.
        //EndAction can be called from anywhere.
        protected override void OnExecute()
        {
            criminal.timer = Random.Range(5.0f, 40.0f);
            criminal.free = false;
            criminal.to_cell = true;
            move.move = false;
        }

        //Called once per frame while the action is active.
        protected override void OnUpdate()
        {
            if(criminal.countdown)
                criminal.timer -= Time.deltaTime;
            criminal.night = Night.value;
            if (criminal.timer <= 0)
            {
                if (criminal.night && !criminal.captured)
                    criminal.escape = true;

                criminal.free = true;


                EndAction(true);
            }

            if(criminal.c_agent != null)
            {
                criminal.ReleaseAgent();
            }

        }

        //Called when the task is disabled.
        protected override void OnStop()
        {
            criminal.cell.Release();
            criminal.cell = null;
            criminal.to_cell = false;
        }

        //Called when the task is paused.
        protected override void OnPause()
        {

        }
    }
}
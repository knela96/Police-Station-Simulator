using UnityEngine;
using NodeCanvas.Framework;
using ParadoxNotion.Design;


namespace NodeCanvas.Tasks.Actions{

    [Name("Go to Desk")]
    [Category("Police")]
    public class GotoDesk : ActionTask{

        PoliceBehaviour police;
        Desk desk;
        bool receptionist;
        SteeringFollowPath follow_path;

        //Use for initialization. This is called only once in the lifetime of the task.
        //Return null if init was successfull. Return an error string otherwise
        protected override string OnInit(){
            police = agent.gameObject.GetComponent<PoliceBehaviour>();
            follow_path = agent.gameObject.GetComponent<SteeringFollowPath>();
            return null;
		}

		//This is called once each time the task is enabled.
		//Call EndAction() to mark the action as finished, either in success or failure.
		//EndAction can be called from anywhere.
		protected override void OnExecute()
        {
            police.animator.SetBool("moving", true);
            if (desk == null)
            {
                //Assign desk Available and create path
                police.AssignDesk();
                if (police.desk != null)
                {
                    desk = police.desk;
                    police.move.target = desk.getPoint().gameObject;
                    follow_path.calcPath(desk.getPoint());
                }
            }
		}

		//Called once per frame while the action is active.
		protected override void OnUpdate(){
			
		}

		//Called when the task is disabled.
		protected override void OnStop(){
            if(follow_path.arrived)
                police.animator.SetBool("moving", false);
        }

		//Called when the task is paused.
		protected override void OnPause(){
			
		}
	}
}
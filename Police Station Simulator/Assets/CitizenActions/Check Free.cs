using UnityEngine;
using UnityEngine.AI;

using NodeCanvas.Framework;
using ParadoxNotion.Design;

namespace NodeCanvas.Tasks.Actions
{

    [Name("Reception Free?")]
    [Category("Citizen")]
    public class CheckFree : ActionTask
    {
        GameObject rP;
        CitizenBehaviour citizen;
        Point c_point;
        //Use for initialization. This is called only once in the lifetime of the task.
        //Return null if init was successfull. Return an error string otherwise
        protected override string OnInit()
        {
            c_point = GameObject.Find("Reception_Point").GetComponent<Point>();
            citizen = agent.gameObject.GetComponent<CitizenBehaviour>();
            return null;
        }

        protected override void OnUpdate()
        {
            citizen.free_point = c_point;
            if (c_point.isAvailable())
                EndAction(true);
        }
    }
}
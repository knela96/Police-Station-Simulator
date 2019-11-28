using UnityEngine;
using NodeCanvas.Framework;
using ParadoxNotion.Design;
[Name("To Reception")]
[Category("Citizen")] // this is the location in the task menu
public class ToReception : ActionTask
{
    protected override void OnExecute()
    {
        Debug.Log("executing code when action starts");
    }
    protected override void OnStop()
    {
        Debug.Log("executing code when action ends");
    }
}
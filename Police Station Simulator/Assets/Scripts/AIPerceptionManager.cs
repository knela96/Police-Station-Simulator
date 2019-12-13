using UnityEngine;
using System.Collections;

public class AIPerceptionManager : MonoBehaviour {

    public GameObject target = null;
	public bool player_detected = false;

	// Update is called once per frame
	void PerceptionEvent (PerceptionEvent ev) {

		if(ev.type == global::PerceptionEvent.types.NEW)
		{
            if (target == null)
            {
                target = ev.go;
                player_detected = true;
            }
		}
		else
        {
            //if (ev.go == target)
            //{
            //    target = null;
            //    player_detected = false;
            //}
        }
	}
}

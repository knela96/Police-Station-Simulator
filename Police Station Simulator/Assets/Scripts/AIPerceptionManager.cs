using UnityEngine;
using System.Collections;

public class AIPerceptionManager : MonoBehaviour {

    public GameObject target = null;
	public bool player_detected = false;

	// Update is called once per frame
	void PerceptionEvent (PerceptionEvent ev) {

		if(ev.type == global::PerceptionEvent.types.NEW)
		{
            target = ev.go;
			player_detected = true;
		}
		else
        {
            target = null;
            player_detected = false;
		}
	}
}

using UnityEngine;
using System.Collections;

public class AIPerceptionManager : MonoBehaviour {

	public bool player_detected = false;

	// Update is called once per frame
	void PerceptionEvent (PerceptionEvent ev) {

		if(ev.type == global::PerceptionEvent.types.NEW)
		{
			player_detected = true;
		}
		else
		{
			player_detected = false;
		}
	}
}

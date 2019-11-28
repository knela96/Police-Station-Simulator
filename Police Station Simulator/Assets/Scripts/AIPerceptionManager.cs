using UnityEngine;
using System.Collections;

public class AIPerceptionManager : MonoBehaviour {

	public GameObject Alert;
	public bool player_detected = false;

	// Update is called once per frame
	void PerceptionEvent (PerceptionEvent ev) {

		if(ev.type == global::PerceptionEvent.types.NEW)
		{
			Debug.Log("Saw something NEW");
			Alert.SetActive(true);
			player_detected = true;
		}
		else
		{
			Debug.Log("LOST something");
			Alert.SetActive(false);
			player_detected = false;
		}
	}
}

using UnityEngine;
using System.Collections;

public class FSM_Alarm : MonoBehaviour
{
    private bool player_detected = false;
    private bool in_alarm = false;
    private Vector3 patrol_pos;
    private WaitForSeconds wait = new WaitForSeconds(1.0f / 20.0f);

    delegate IEnumerator State();
    private State state;

    public GameObject alarm;
    public BansheeGz.BGSpline.Curve.BGCurve path;


    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == alarm)
            in_alarm = true;
    }

    // Update is called once per frame
    void PerceptionEvent(PerceptionEvent ev)
    {
        if (ev.type == global::PerceptionEvent.types.NEW)
        {
            player_detected = true;
        }
    }

    // Use this for initialization
    IEnumerator Start()
    {
        state = Patrol;

        while (enabled)
            yield return StartCoroutine(state());
    }

    IEnumerator Patrol()
    {
        Debug.Log("Start Patrol state");

        while (player_detected == false)
            yield return wait;

        path.gameObject.SetActive(false);
        state = GoAlarm;
    }

    IEnumerator GoAlarm()
    {
        Debug.Log("Start Go Alarm");
        patrol_pos = transform.position;
        GetComponent<UnityEngine.AI.NavMeshAgent>().destination = alarm.transform.position;
        while (in_alarm == false)
            yield return wait;

        state = BackToAlarm;
    }

    IEnumerator BackToAlarm()
    {
        Debug.Log("Back to Alarm");
        GetComponent<UnityEngine.AI.NavMeshAgent>().destination = patrol_pos;
        while ((patrol_pos - transform.position).magnitude > 1.0f)
            yield return wait;

        in_alarm = false;
        player_detected = false;
        GetComponent<UnityEngine.AI.NavMeshAgent>().ResetPath();
        path.gameObject.SetActive(true);

        state = Patrol;
    }

}
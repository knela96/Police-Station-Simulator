using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class timeonstage : MonoBehaviour
{

    public Text timertxt;
    private float startTime;
    LevelLoop level;
    int hours;
    int min;
    string minutes;
    string seconds;

    // Start is called before the first frame update
    void Start()
    {
        level = GameObject.Find("Level").GetComponent<LevelLoop>();

        startTime = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        float t = Time.time - startTime;
        minutes = ((9 + (int)t / 10) % 24).ToString();
        seconds = (t * 6 % 60).ToString("f0");

        timertxt.text = minutes + ":" + seconds;

    }
}

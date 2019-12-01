using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class timeonstage : MonoBehaviour
{

    public Text timertxt;
    private float startTime;
    LevelLoop level;

    // Start is called before the first frame update
    void Start()
    {
        level = GameObject.Find("Level").GetComponent<LevelLoop>();

        startTime = 0;
    }

    // Update is called once per frame
    void Update()
    {
        float t = level.getCycle() - startTime;

        string minutes = ((int)t / 60).ToString();
        string seconds = (t % 60).ToString("f0");

        timertxt.text = minutes + ":" + seconds;

    }
}

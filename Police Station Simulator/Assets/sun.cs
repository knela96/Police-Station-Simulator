using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sun : MonoBehaviour
{
    LevelLoop level;
    Light light;
    Color day;
    Color night;
    // Start is called before the first frame update
    void Awake()
    {
        level = GameObject.Find("Level").GetComponent<LevelLoop>();
        light = GetComponent<Light>();

        day.a = 255f/255f;
        day.r = 255f/255f;
        day.b = 255f/255f;
        day.g = 255f/255f;

        night.a = 255f/255f;
        night.r = 165f/255f;
        night.b = 255f/255f;
        night.g = 246f/255f;
    }

    // Update is called once per frame
    void Update()
    {
        //transform.RotateAround(Vector3.zero, Vector3.right, 4.5f * Time.deltaTime);
        if (light.intensity == 1F && level.day == false) {

            light.intensity = 0.2F;
            light.color = night;

        } else if (light.intensity == 0.2F && level.day == true) {

            light.intensity = 1F;
            light.color = day;

        }

    }
}

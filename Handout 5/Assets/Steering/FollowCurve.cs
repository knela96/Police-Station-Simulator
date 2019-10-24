using System.Collections;
using System.Collections.Generic;
using BansheeGz;
using BansheeGz.BGSpline.Components;
using UnityEngine;

public class FollowCurve : SteeringAbstract
{

    public BGCcMath path;
    float ratio = 0.0f;

    public float speed = 5;

    // Start is called before the first frame update
    void Start()
    {
        //transform.position = path.CalcPositionByClosestPoint(transform.position);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(ratio >= 1.0f)
            ratio = 0.0f;

        if (ratio <= 1.0f)
        {
            transform.position = path.CalcPositionByDistanceRatio(ratio);

            ratio += 0.1f * speed * Time.deltaTime;
        }
    }
}

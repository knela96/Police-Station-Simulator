using System.Collections;
using System.Collections.Generic;
using UnityEngine;

static public class SteeringConf
{
    public const int num_priorities = 5;
}


public class SteeringAbstract : MonoBehaviour
{
    [Range(0, SteeringConf.num_priorities)]
    public int priority = 0;
}
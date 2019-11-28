using System.Collections;
using System.Collections.Generic;
using UnityEngine;

static public class SteeringConf
{
    public const int num_priorities = 10;
}


public class SteeringAbstract : MonoBehaviour
{
    [Range(0, SteeringConf.num_priorities - 1)]
    public int priority = 0;
}
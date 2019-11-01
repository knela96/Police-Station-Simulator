using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Entity : MonoBehaviour
{
    public Move move;
    public bool action;
    public NavMeshPath path;
    public SteeringFollowPath follow_path;
    public SteeringArrive arrive;
    public SteeringAlign align;


    // Start is called before the first frame update
    protected virtual void Awake()
    {
        move = GetComponent<Move>();
        move.move = true;
        arrive = GetComponent<SteeringArrive>();
        action = false;
        follow_path = gameObject.GetComponent<SteeringFollowPath>();
        follow_path.path = new NavMeshPath();
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        
    }
}

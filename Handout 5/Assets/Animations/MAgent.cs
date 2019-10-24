using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MAgent : MonoBehaviour
{
    NavMeshAgent agent;
    public GameObject Target;
    Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        agent.SetDestination(Target.transform.position);
        if (agent.remainingDistance > 0)
            animator.SetBool("Movement", true);
        else
            animator.SetBool("Movement", false);

        Vector3 vel = agent.desiredVelocity.normalized;
 

        animator.SetFloat("z", agent.velocity.z);
        animator.SetFloat("x", agent.velocity.x);

    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenDoor : MonoBehaviour
{
    Animator animator;
    // Start is called before the first frame update
    void Start()
    {
        animator = transform.Find("AI_Env_Bunker_Door").GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 8)
        {
            animator.SetBool("Open", true);
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.layer == 8)
        {
            animator.SetBool("Open", true);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == 8)
        {
            animator.SetBool("Open", false);
        }
    }
}

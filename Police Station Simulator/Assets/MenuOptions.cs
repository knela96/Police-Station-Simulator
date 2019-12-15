using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuOptions : MonoBehaviour
{
    public bool show;

    // Start is called before the first frame update
    void Start()
    {
        show = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("escape"))
        {
            show = !show;
        }
        if (show)
        {
            transform.Find("Options Menu").gameObject.SetActive(true);
            GameObject.Find("Main Camera").GetComponent<CameraMovement>().enabled = false;
        }
        else
        {
            transform.Find("Options Menu").gameObject.SetActive(false);
            GameObject.Find("Main Camera").GetComponent<CameraMovement>().enabled = true;
        }

    }
}

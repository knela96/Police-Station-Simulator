using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonDesk7 : MonoBehaviour
{
    LevelLoop level;

    GameObject Desk13;

    // Start is called before the first frame update
    void Awake()
    {
        level = GameObject.Find("Level").GetComponent<LevelLoop>();

        Desk13 = GameObject.Find("desk13");

    }
    // Update is called once per frame

    public void Activenow()
    {

        Desk13.SetActive(true);
        gameObject.SetActive(false);

    }
}

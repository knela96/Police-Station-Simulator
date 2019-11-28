using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonDesk1 : MonoBehaviour
{
    LevelLoop level;

    GameObject Desk7;

    // Start is called before the first frame update
    void Awake()
    {
        level = GameObject.Find("Level").GetComponent<LevelLoop>();

        Desk7 = GameObject.Find("desk7");

    }
    // Update is called once per frame

    public void Activenow()
    {

        Desk7.SetActive(true);
        gameObject.SetActive(false);

    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonDesk6 : MonoBehaviour
{
    LevelLoop level;

    GameObject Desk12;

    // Start is called before the first frame update
    void Awake()
    {
        level = GameObject.Find("Level").GetComponent<LevelLoop>();

        Desk12 = GameObject.Find("desk12");

    }
    // Update is called once per frame

    public void Activenow()
    {

        Desk12.SetActive(true);
        gameObject.SetActive(false);

    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonCell6 : MonoBehaviour
{
    LevelLoop level;

    GameObject Cell6;

    // Start is called before the first frame update
    void Awake()
    {
        level = GameObject.Find("Level").GetComponent<LevelLoop>();

        Cell6 = GameObject.Find("CellP 6");

    }
    // Update is called once per frame

    public void Activenow()
    {

        Cell6.SetActive(true);


    }
}

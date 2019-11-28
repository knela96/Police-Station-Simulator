using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonCell5 : MonoBehaviour
{
    LevelLoop level;

    GameObject Cell5;

    // Start is called before the first frame update
    void Awake()
    {
        level = GameObject.Find("Level").GetComponent<LevelLoop>();

        Cell5 = GameObject.Find("CellP 5");

    }
    // Update is called once per frame

    public void Activenow()
    {

        Cell5.SetActive(true);


    }
}

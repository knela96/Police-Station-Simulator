using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonCell4 : MonoBehaviour
{

    LevelLoop level;

    GameObject Cell4;

    // Start is called before the first frame update
    void Awake()
    {
        level = GameObject.Find("Level").GetComponent<LevelLoop>();

        Cell4 = GameObject.Find("CellP 3");

    }
    // Update is called once per frame

    public void Activenow()
    {

        Cell4.SetActive(true);


    }
}

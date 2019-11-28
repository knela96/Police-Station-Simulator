using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonCell7 : MonoBehaviour
{
    LevelLoop level;

    GameObject Cell7;

    // Start is called before the first frame update
    void Awake()
    {
        level = GameObject.Find("Level").GetComponent<LevelLoop>();

        Cell7 = GameObject.Find("CellP 7");

    }
    // Update is called once per frame

    public void Activenow()
    {

        Cell7.SetActive(true);


    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonCell8 : MonoBehaviour
{
    LevelLoop level;

    GameObject Cell8;

    // Start is called before the first frame update
    void Awake()
    {
        level = GameObject.Find("Level").GetComponent<LevelLoop>();

        Cell8 = GameObject.Find("CellP 8");

    }
    // Update is called once per frame

    public void Activenow()
    {

        Cell8.SetActive(true);


    }
}

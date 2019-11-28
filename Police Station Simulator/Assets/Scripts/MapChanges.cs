using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapChanges : MonoBehaviour
{
    int countdown = 0;
    LevelLoop level;
    GameObject Cell3;
    GameObject Cell4;
    GameObject Cell5;
    GameObject Cell6;
    GameObject Cell7;
    GameObject Cell8;
    GameObject Cell9;
    // Start is called before the first frame update
    void Awake()
    {
        level = GameObject.Find("Level").GetComponent<LevelLoop>();
        Cell3 = GameObject.Find("CellP 3");
        Cell4 = GameObject.Find("CellP 4");
        Cell5 = GameObject.Find("CellP 5");
        Cell6 = GameObject.Find("CellP 6");
        Cell7 = GameObject.Find("CellP 7");
        Cell8 = GameObject.Find("CellP 8");
        Cell9 = GameObject.Find("CellP 9");


        Cell3.SetActive(false);
        Cell4.SetActive(false);
        Cell5.SetActive(false);
        Cell6.SetActive(false);
        Cell7.SetActive(false);
        Cell8.SetActive(false);
        Cell9.SetActive(false);


    }
    // Update is called once per frame

    void Update()
    {

    }
}

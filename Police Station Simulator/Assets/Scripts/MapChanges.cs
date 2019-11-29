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

    GameObject Desk7;
    GameObject Desk8;
    GameObject Desk9;
    GameObject Desk10;
    GameObject Desk11;
    GameObject Desk12;
    GameObject Desk13;
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

        Desk7 = GameObject.Find("desk7");
        Desk8 = GameObject.Find("desk8");
        Desk9 = GameObject.Find("desk9");
        Desk10 = GameObject.Find("desk10");
        Desk11 = GameObject.Find("desk11");
        Desk12 = GameObject.Find("desk12");
        Desk13 = GameObject.Find("desk13");

        Cell3.SetActive(false);
        Cell4.SetActive(false);
        Cell5.SetActive(false);
        Cell6.SetActive(false);
        Cell7.SetActive(false);
        Cell8.SetActive(false);
        Cell9.SetActive(false);

        Desk7.SetActive(false);
        Desk8.SetActive(false);
        Desk9.SetActive(false);
        Desk10.SetActive(false);
        Desk11.SetActive(false);
        Desk12.SetActive(false);
        Desk13.SetActive(false);

    }
    // Update is called once per frame

    void Update()
    {

    }
}

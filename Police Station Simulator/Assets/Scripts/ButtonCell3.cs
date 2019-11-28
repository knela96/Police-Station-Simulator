using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonCell3 : MonoBehaviour
{
   
    LevelLoop level;

    GameObject Cell3;

    // Start is called before the first frame update
    void Awake()
    {
        level = GameObject.Find("Level").GetComponent<LevelLoop>();

        Cell3 = GameObject.Find("CellP 4");

    }
    // Update is called once per frame

    public void Activenow()
    {
       
        Cell3.SetActive(true);
       

    }
}

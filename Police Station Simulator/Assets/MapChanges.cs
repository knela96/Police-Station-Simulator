using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapChanges : MonoBehaviour
{
    int countdown = 0;
    LevelLoop level;
    GameObject Levelaugm;
    GameObject Wall1;
    GameObject Wall2;
    // Start is called before the first frame update
    void Awake()
    {
        level = GameObject.Find("Level").GetComponent<LevelLoop>();
        Levelaugm = GameObject.Find("Phase2");
        Wall1 = GameObject.Find("StWall");
        Wall2 = GameObject.Find(" Phase2 Wall");
       
        Levelaugm.SetActive(false);
        Wall1.SetActive(true);
        Wall2.SetActive(false);
    }
    // Update is called once per frame
    void Update()
    {

        if (level.getCycle() >= 119.9f) { 
            countdown++;
        }

        if (countdown == 4) {
            Levelaugm.SetActive(true);
            Wall1.SetActive(false);
        }

    }
}

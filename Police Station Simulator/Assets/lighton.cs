using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class lighton : MonoBehaviour
{
    LevelLoop level;
   
    
    void Awake() {
        level = GameObject.Find("Level").GetComponent<LevelLoop>();
       // gameObject.SetActive(true);
    }
    // Update is called once per frame
    void Update()
    {
        Debug.Log(level.day);
        if (level.day) {
            gameObject.GetComponent<Light>().enabled = false;
            Debug.Log("DAY DEACTIVATE");
        } else if (!level.day){
            gameObject.GetComponent<Light>().enabled = true;
            Debug.Log("Night ACTIVATE");
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonCell9 : MonoBehaviour
{
    LevelLoop level;

    GameObject Cell9;

    // Start is called before the first frame update
    void Awake()
    {
        level = GameObject.Find("Level").GetComponent<LevelLoop>();

        Cell9 = GameObject.Find("CellP 9");

    }
    // Update is called once per frame

    public void Activenow()
    {

        Cell9.SetActive(true);
        gameObject.SetActive(false);

    }
}

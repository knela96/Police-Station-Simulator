using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonDesk5 : MonoBehaviour
{
    LevelLoop level;

    GameObject Desk11;

    // Start is called before the first frame update
    void Awake()
    {
        level = GameObject.Find("Level").GetComponent<LevelLoop>();

        Desk11 = GameObject.Find("desk11");

    }
    // Update is called once per frame

    public void Activenow()
    {

        Desk11.SetActive(true);
        gameObject.SetActive(false);

    }
}

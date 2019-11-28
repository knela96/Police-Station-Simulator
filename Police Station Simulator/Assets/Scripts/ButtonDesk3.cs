using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonDesk3 : MonoBehaviour
{
    LevelLoop level;

    GameObject Desk9;

    // Start is called before the first frame update
    void Awake()
    {
        level = GameObject.Find("Level").GetComponent<LevelLoop>();

        Desk9 = GameObject.Find("desk9");

    }
    // Update is called once per frame

    public void Activenow()
    {

        Desk9.SetActive(true);
        gameObject.SetActive(false);

    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonDesk4 : MonoBehaviour
{
    LevelLoop level;
    MoneyBar money;
    float auxm;
    public float cost;
    GameObject Desk10;

    // Start is called before the first frame update
    void Awake()
    {
        level = GameObject.Find("Level").GetComponent<LevelLoop>();
        money = GameObject.Find("Money").GetComponent<MoneyBar>();
        Desk10 = GameObject.Find("desk10");

    }
    // Update is called once per frame

    public void Activenow()
    {

        Desk10.SetActive(true);

        gameObject.SetActive(false);
        auxm = money.CurrentValue;
        auxm = auxm - cost;
        money.SetBar((int)auxm);
    }
}

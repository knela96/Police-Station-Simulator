using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonDesk6 : MonoBehaviour
{
    LevelLoop level;
    MoneyBar money;
    float auxm;
    public float cost;
    GameObject Desk12;

    // Start is called before the first frame update
    void Awake()
    {
        level = GameObject.Find("Level").GetComponent<LevelLoop>();
        money = GameObject.Find("Money").GetComponent<MoneyBar>();
        Desk12 = GameObject.Find("desk12");

    }
    // Update is called once per frame

    public void Activenow()
    {

        Desk12.SetActive(true);
        gameObject.SetActive(false);
        auxm = money.CurrentValue;
        auxm = auxm - cost;
        money.SetBar((int)auxm);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonDesk1 : MonoBehaviour
{
    LevelLoop level;
    MoneyBar money;
    AssignDesk desks;
    float auxm;
    public float cost;
    GameObject Desk7;
    GameObject FDesk7;
    // Start is called before the first frame update
    void Awake()
    {
        level = GameObject.Find("Level").GetComponent<LevelLoop>();
        money = GameObject.Find("Money").GetComponent<MoneyBar>();
        desks = GameObject.Find("Desks").GetComponent<AssignDesk>();
        Desk7 = GameObject.Find("desk7");
        FDesk7 = GameObject.Find("fdesk7");
    }
    // Update is called once per frame

    public void Activenow()
    {
        
        Desk7.SetActive(true);
        FDesk7.SetActive(false);
        desks.desksav++;
        gameObject.SetActive(false);
        auxm = money.CurrentValue;
        auxm = auxm - cost;
        money.SetBar((int)auxm);
    }
}

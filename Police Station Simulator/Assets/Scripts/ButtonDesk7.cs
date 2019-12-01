using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonDesk7 : MonoBehaviour
{
    LevelLoop level;
    MoneyBar money;
    AssignDesk desks;
    float auxm;
    public float cost;
    GameObject Desk13;
    GameObject FDesk13;
    // Start is called before the first frame update
    void Awake()
    {
        level = GameObject.Find("Level").GetComponent<LevelLoop>();
        money = GameObject.Find("Money").GetComponent<MoneyBar>();
        desks = GameObject.Find("Desks").GetComponent<AssignDesk>();
        Desk13 = GameObject.Find("desk13");
        FDesk13 = GameObject.Find("fdesk13");
    }
    // Update is called once per frame

    public void Activenow()
    {

        Desk13.SetActive(true);
        FDesk13.SetActive(false);
        gameObject.SetActive(false);
        auxm = money.CurrentValue;
        auxm = auxm - cost;
        money.SetBar((int)auxm);
    }
}

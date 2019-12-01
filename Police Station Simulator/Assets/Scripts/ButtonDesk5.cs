using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonDesk5 : MonoBehaviour
{
    LevelLoop level;
    MoneyBar money;
    AssignDesk desks;
    float auxm;
    public float cost;
    GameObject Desk11;
    GameObject FDesk11;
    // Start is called before the first frame update
    void Awake()
    {
        level = GameObject.Find("Level").GetComponent<LevelLoop>();
        money = GameObject.Find("Money").GetComponent<MoneyBar>();
        desks = GameObject.Find("Desks").GetComponent<AssignDesk>();
        Desk11 = desks.transform.Find("desk11").gameObject;
        FDesk11 = GameObject.Find("FalseDesks").transform.Find("fdesk11").gameObject;
    }
    // Update is called once per frame

    public void Activenow()
    {

        Desk11.SetActive(true);
        FDesk11.SetActive(false);
        gameObject.SetActive(false);
        auxm = money.CurrentValue;
        auxm = auxm - cost;
        money.SetBar((int)auxm);
    }
}

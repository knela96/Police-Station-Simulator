using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonDesk6 : MonoBehaviour
{
    LevelLoop level;
    MoneyBar money;
    AssignDesk desks;
    float auxm;
    public float cost;
    GameObject Desk12;
    GameObject FDesk12;
    // Start is called before the first frame update
    void Awake()
    {
        level = GameObject.Find("Level").GetComponent<LevelLoop>();
        money = GameObject.Find("Money").GetComponent<MoneyBar>();
        desks = GameObject.Find("Desks").GetComponent<AssignDesk>();
        Desk12 = GameObject.Find("desk12");
        FDesk12 = GameObject.Find("fdesk12");
    }
    // Update is called once per frame

    public void Activenow()
    {

        Desk12.SetActive(true);
        FDesk12.SetActive(false);
        desks.desksav++;
        gameObject.SetActive(false);
        auxm = money.CurrentValue;
        auxm = auxm - cost;
        money.SetBar((int)auxm);
    }
}

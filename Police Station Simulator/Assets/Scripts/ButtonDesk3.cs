using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonDesk3 : MonoBehaviour
{
    LevelLoop level;
    MoneyBar money;
    AssignDesk desks;
    float auxm;
    public float cost;
    GameObject Desk9;
    GameObject FDesk9;

    // Start is called before the first frame update
    void Awake()
    {
        level = GameObject.Find("Level").GetComponent<LevelLoop>();
        money = GameObject.Find("Money").GetComponent<MoneyBar>();
        desks = GameObject.Find("Desks").GetComponent<AssignDesk>();
        Desk9 = GameObject.Find("desk9");
        FDesk9 = GameObject.Find("fdesk9");
    }
    // Update is called once per frame

    public void Activenow()
    {

        Desk9.SetActive(true);
        FDesk9.SetActive(false);
        gameObject.SetActive(false);
        auxm = money.CurrentValue;
        auxm = auxm - cost;
        money.SetBar((int)auxm);
    }
}

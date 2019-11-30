using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonDesk2 : MonoBehaviour
{
    LevelLoop level;
    MoneyBar money;
    float auxm;
    public float cost;
    GameObject Desk8;
    GameObject FDesk8;
    // Start is called before the first frame update
    void Awake()
    {
        level = GameObject.Find("Level").GetComponent<LevelLoop>();
        money = GameObject.Find("Money").GetComponent<MoneyBar>();
        Desk8 = GameObject.Find("desk8");
        FDesk8 = GameObject.Find("fdesk8");
    }
    // Update is called once per frame

    public void Activenow()
    {

        Desk8.SetActive(true);
        FDesk8.SetActive(false);
        gameObject.SetActive(false);
        auxm = money.CurrentValue;
        auxm = auxm - cost;
        money.SetBar((int)auxm);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonCell6 : MonoBehaviour
{
    LevelLoop level;
    MoneyBar money;
    float auxm;
    public float cost;
    GameObject Cell6;
    GameObject FCell6;
    // Start is called before the first frame update
    void Awake()
    {
        level = GameObject.Find("Level").GetComponent<LevelLoop>();
        money = GameObject.Find("Money").GetComponent<MoneyBar>();
        Cell6 = GameObject.Find("CellP 6");
        FCell6 = GameObject.Find("fCellP 6");
    }
    // Update is called once per frame

    public void Activenow()
    {

        Cell6.SetActive(true);
        FCell6.SetActive(false);
        gameObject.SetActive(false);
        auxm = money.CurrentValue;
        auxm = auxm - cost;
        money.SetBar((int)auxm);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonCell5 : MonoBehaviour
{
    LevelLoop level;
    MoneyBar money;
    float auxm;
    public float cost;
    AssignCell cells;
    GameObject Cell5;
    GameObject FCell5;
    // Start is called before the first frame update
    void Awake()
    {
        level = GameObject.Find("Level").GetComponent<LevelLoop>();
        money = GameObject.Find("Money").GetComponent<MoneyBar>();
        cells = GameObject.Find("Cells").GetComponent<AssignCell>();
        Cell5 = GameObject.Find("CellP 5");
        FCell5 = GameObject.Find("fCellP 5");
    }
    // Update is called once per frame

    public void Activenow()
    {

        Cell5.SetActive(true);
        FCell5.SetActive(false);
        gameObject.SetActive(false);
        auxm = money.CurrentValue;
        auxm = auxm - cost;
        money.SetBar((int)auxm);
    }
}

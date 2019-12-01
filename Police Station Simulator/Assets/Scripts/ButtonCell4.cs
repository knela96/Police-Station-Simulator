using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonCell4 : MonoBehaviour
{

    LevelLoop level;
    MoneyBar money;
    float auxm;
    public float cost;
    AssignCell cells;
    GameObject Cell4;
    GameObject FCell4;
    // Start is called before the first frame update
    void Awake()
    {
        level = GameObject.Find("Level").GetComponent<LevelLoop>();
        money = GameObject.Find("Money").GetComponent<MoneyBar>();
        cells = GameObject.Find("Cells").GetComponent<AssignCell>();
        Cell4 = GameObject.Find("CellP 3");
        FCell4 = GameObject.Find("fCellP 3");
    }
    // Update is called once per frame

    public void Activenow()
    {

        Cell4.SetActive(true);
        FCell4.SetActive(false);
        cells.cellsav++;
        gameObject.SetActive(false);
        auxm = money.CurrentValue;
        auxm = auxm - cost;
        money.SetBar((int)auxm);
    }
}

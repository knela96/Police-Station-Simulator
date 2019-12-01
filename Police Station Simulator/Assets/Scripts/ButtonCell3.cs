using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonCell3 : MonoBehaviour
{
   
    LevelLoop level;
    MoneyBar money;
    float auxm;
    public float cost;
    AssignCell cells;
    GameObject Cell3;
    GameObject FCell3;
    // Start is called before the first frame update
    void Awake()
    {
        level = GameObject.Find("Level").GetComponent<LevelLoop>();
        money = GameObject.Find("Money").GetComponent<MoneyBar>();
        cells = GameObject.Find("Cells").GetComponent<AssignCell>();
        Cell3 = GameObject.Find("CellP 4");
        FCell3 = GameObject.Find("fCellP 4");
    }
    // Update is called once per frame

    public void Activenow()
    {
       
        Cell3.SetActive(true);
        FCell3.SetActive(false);
        cells.cellsav++;
        gameObject.SetActive(false);
        auxm = money.CurrentValue;
        auxm = auxm - cost;
        money.SetBar((int)auxm);
    }
}

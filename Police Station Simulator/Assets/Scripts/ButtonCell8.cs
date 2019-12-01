using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonCell8 : MonoBehaviour
{
    LevelLoop level;
    MoneyBar money;
    float auxm;
    public float cost;
    AssignCell cells;
    GameObject Cell8;
    GameObject FCell8;
    // Start is called before the first frame update
    void Awake()
    {
        level = GameObject.Find("Level").GetComponent<LevelLoop>();
        money = GameObject.Find("Money").GetComponent<MoneyBar>();
        cells = GameObject.Find("Cells").GetComponent<AssignCell>();
        Cell8 = GameObject.Find("CellP 8");
        FCell8 = GameObject.Find("fCellP 8");
    }
    // Update is called once per frame

    public void Activenow()
    {

        Cell8.SetActive(true);
        FCell8.SetActive(false);
        cells.cellsav++;
        gameObject.SetActive(false);
        auxm = money.CurrentValue;
        auxm = auxm - cost;
        money.SetBar((int)auxm);
    }
}

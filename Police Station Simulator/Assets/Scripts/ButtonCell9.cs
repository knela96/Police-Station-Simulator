using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonCell9 : MonoBehaviour
{
    LevelLoop level;
    MoneyBar money;
    float auxm;
    public float cost;
    GameObject Cell9;
    GameObject FCell9;
    // Start is called before the first frame update
    void Awake()
    {
        level = GameObject.Find("Level").GetComponent<LevelLoop>();
        money = GameObject.Find("Money").GetComponent<MoneyBar>();
        Cell9 = GameObject.Find("CellP 9");
        FCell9 = GameObject.Find("fCellP 9");
    }
    // Update is called once per frame

    public void Activenow()
    {

        Cell9.SetActive(true);
        FCell9.SetActive(false);
        gameObject.SetActive(false);
        auxm = money.CurrentValue;
        auxm = auxm - cost;
        money.SetBar((int)auxm);
    }
}

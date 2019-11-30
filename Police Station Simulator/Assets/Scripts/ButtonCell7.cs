using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonCell7 : MonoBehaviour
{
    LevelLoop level;
    MoneyBar money;
    float auxm;
    public float cost;
    GameObject Cell7;
    GameObject FCell7;
    // Start is called before the first frame update
    void Awake()
    {
        level = GameObject.Find("Level").GetComponent<LevelLoop>();
        money = GameObject.Find("Money").GetComponent<MoneyBar>();
        Cell7 = GameObject.Find("CellP 7");
        FCell7 = GameObject.Find("fCellP 7");
    }
    // Update is called once per frame

    public void Activenow()
    {

        Cell7.SetActive(true);
        FCell7.SetActive(false);
        gameObject.SetActive(false);
        auxm = money.CurrentValue;
        auxm = auxm - cost;
        money.SetBar((int)auxm);
    }
}

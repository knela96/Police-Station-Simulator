using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonCell8 : MonoBehaviour
{
    LevelLoop level;
    MoneyBar money;
    float auxm;
    public float cost;
    GameObject Cell8;

    // Start is called before the first frame update
    void Awake()
    {
        level = GameObject.Find("Level").GetComponent<LevelLoop>();
        money = GameObject.Find("Money").GetComponent<MoneyBar>();
        Cell8 = GameObject.Find("CellP 8");

    }
    // Update is called once per frame

    public void Activenow()
    {

        Cell8.SetActive(true);
        gameObject.SetActive(false);
        auxm = money.CurrentValue;
        auxm = auxm - cost;
        money.SetBar((int)auxm);
    }
}

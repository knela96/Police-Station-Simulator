using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonAction : MonoBehaviour
{
    LevelLoop level;
    MoneyBar money;
    AssignDesk desks;
    Button policeButton;
    public GameObject FDesk;
    public GameObject Desk;
    float auxm;
    public float cost = 10;

    void Start()
    {
        policeButton = gameObject.GetComponent<Button>();
        level = GameObject.Find("Level").GetComponent<LevelLoop>();
        money = GameObject.Find("Money").GetComponent<MoneyBar>();
        desks = GameObject.Find("Desks").GetComponent<AssignDesk>();
        policeButton.onClick.AddListener(TaskOnClick);
    }

    // Update is called once per frame
    void Update()
    {

    }

    void TaskOnClick()
    {
        Desk.SetActive(true);
        FDesk.SetActive(false);
        gameObject.SetActive(false);
        auxm = money.CurrentValue;
        auxm = auxm - cost;
        money.SetBar((int)auxm);
    }
}
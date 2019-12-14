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
    public int cost;

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
        money.updateMoney(-cost);
        money.StartAnim(-cost,0);
        desks.num_active++;
        level.addPolicemen();
        money.SetBar((int)auxm);
    }
}
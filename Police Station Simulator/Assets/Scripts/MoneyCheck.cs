using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoneyCheck : MonoBehaviour
{
    // Start is called before the first frame update
    LevelLoop level;
    MoneyBar money;
    public int cost;
    public bool update = false;
    // Update is called once per frame

    void Awake()
    {
        level = GameObject.Find("Level").GetComponent<LevelLoop>();
        money = GameObject.Find("Money").GetComponent<MoneyBar>();
    }

    void Update()
    {
        if (money.CurrentValue < cost && update == false) {
            foreach (Transform child in gameObject.transform)
            {
                child.gameObject.SetActive(false);
            }
            update = true;
        }
        else if(money.CurrentValue >= cost && update == true)
        {
            foreach (Transform child in gameObject.transform)
            {
                child.gameObject.SetActive(true);
            }
            update = false;
        }

    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoneyCheck : MonoBehaviour
{
    // Start is called before the first frame update
    LevelLoop level;
    MoneyBar money;
    // Update is called once per frame

    void Awake()
    {
        level = GameObject.Find("Level").GetComponent<LevelLoop>();
        money = GameObject.Find("Money").GetComponent<MoneyBar>();
      
    }

    void Update()
    {
        if (money.CurrentValue <= 10) {

            gameObject.SetActive(false);

        }

    }
}

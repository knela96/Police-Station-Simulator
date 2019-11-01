using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelLoop : MonoBehaviour
{
    public GameObject[] citizens_prebab;
    public GameObject[] policemen_prebab;
    public GameObject[] criminals_prebab;

    public List<GameObject> citizens;
    public List<GameObject> policemen;
    public List<GameObject> criminals;

    float timer1 = 0;
    float timer2 = 0;
    float timer3 = 0;
    float cycle = 0;

    int c1 = 0;
    int c2 = 0;
    int c3 = 0;

    int patrol = 0;

    public bool day = true;
    bool actions = false;
    Vector3 vec;

    // Start is called before the first frame update
    void Awake()
    {
        //citizens.Add(Instantiate(citizens_prebab[Random.Range(0, citizens_prebab.Length - 1)], GameObject.Find("Entrance").transform.position, Quaternion.Euler(0, 90, 0)));
        //policemen.Add(Instantiate(policemen_prebab[Random.Range(0, policemen_prebab.Length - 1)], GameObject.Find("Point").transform.position, Quaternion.Euler(0, 90, 0)));
        criminals.Add(Instantiate(criminals_prebab[c3], GameObject.Find("Entrance").transform.position, Quaternion.Euler(0, 90, 0)));
        vec = GameObject.Find("Entrance").transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(cycle);
        if (day)
        {
            if (cycle - timer1 > 10)
            {
                timer1 = cycle;
                addCitizen(vec);
            }
            if (cycle - timer2 > 20)
            {
                timer2 = cycle;
                addPolicemen(vec);
            }
            if (cycle - timer3 > 30)
            {
                timer3 = cycle;
                addCriminal(vec);
            }
        }
        else if(!actions && !day)
        {
            foreach(GameObject go in citizens)
            {
                if (go != null)
                    go.GetComponent<CitizenBehaviour>().Night();
            }
            foreach (GameObject go in policemen)
            {
                if(go != null)
                    go.GetComponent<PoliceBehaviour>().Night(patrol++);
            }
            foreach (GameObject go in criminals)
            {
                if (go != null)
                    go.GetComponent<CriminalBehavior>().Night();
            }
            actions = true;
        }

        cycle += Time.deltaTime;

        if (c1 == 2)
            c1 = 0;
        if (c2 == 2)
            c2 = 0;
        if (c3 == 2)
            c3 = 0;

        if (cycle >= 10)
            day = false;
    }

    public void addPolicemen(Vector3 pos)
    {
        policemen.Add(Instantiate(policemen_prebab[c1], pos, Quaternion.Euler(0, 90, 0)));
        c1++;
    }
    public void addCitizen(Vector3 pos)
    {
        citizens.Add(Instantiate(citizens_prebab[c2], pos, Quaternion.Euler(0, 90, 0)));
        c2++;
    }
    public void addCriminal(Vector3 pos)
    {
        criminals.Add(Instantiate(criminals_prebab[c3], pos, Quaternion.Euler(0, 90, 0)));
        c3++;
    }
}

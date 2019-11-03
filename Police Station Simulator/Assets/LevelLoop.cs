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
    float timer2 = -15;
    float timer3 = 0;
    float cycle = 0;

    int c1 = 0;
    int c2 = 0;
    int c3 = 0;

    int patrol = 0;

    public bool day = true;
    bool actions = true;
    Vector3 vec;

    // Start is called before the first frame update
    void Awake()
    {
       vec = GameObject.Find("Entrance").transform.position;
       policemen.Add(Instantiate(policemen_prebab[Random.Range(0, policemen_prebab.Length - 1)], GameObject.Find("Entrance").transform.position, Quaternion.Euler(0, 90, 0)));
       policemen[0].GetComponent<PoliceBehaviour>().behaviour = PoliceBehaviour.TypeAction.Receptionist;
    }

    // Update is called once per frame
    void Update()
    {
        if (day)
        {
            if (cycle - timer1 > 15)
            {
                timer1 = cycle;
                addCitizen(vec);
            }
            if (cycle - timer2 > 20)
            {
                timer2 = cycle;
                addPolicemen(vec);
            }
            if (cycle - timer3 > 35)
            {
                timer3 = cycle;
               addCriminal(vec);
            }
            if (!actions)
            {
                //foreach (GameObject go in citizens)
                //{
                //    if (go != null)
                //        go.GetComponent<CitizenBehaviour>().Day();
                //}
                foreach (GameObject go in policemen)
                {
                    if (go != null)
                        go.GetComponent<PoliceBehaviour>().Day();
                }

                GameObject ob = Instantiate(policemen_prebab[Random.Range(0, policemen_prebab.Length - 1)], GameObject.Find("Entrance").transform.position, Quaternion.Euler(0, 90, 0));
                ob.GetComponent<PoliceBehaviour>().behaviour = PoliceBehaviour.TypeAction.Receptionist;
                policemen.Add(ob);
                //foreach (GameObject go in criminals)
                //{
                //    if (go != null)
                //        go.GetComponent<CriminalBehavior>().Day();
                //}

                timer1 = 0;
                timer2 = -15;
                timer3 = 0;

                actions = true;
            }

        }
        else if(!actions && !day)
        {
            patrol = 0;

            foreach (GameObject go in citizens)
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

        if (cycle >= 90)
        {
            day = !day;
            cycle = 0;
            actions = false;
        }
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

    public float getCycle()
    {
        return cycle;
    }
}

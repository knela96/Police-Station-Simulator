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
            //Spawn Entites evey x time
            if (cycle - timer1 > 3)//15
            {
                timer1 = cycle;
                addCitizen(vec);
            }
            if (cycle - timer2 > 22)
            {
                timer2 = cycle;
                addPolicemen(vec);
            }
            if (cycle - timer3 > 31)
            {
                timer3 = cycle;
               addCriminal(vec);
            }
            if (!actions)
            {
                //Change the behaviour to Day
                foreach (GameObject go in policemen)
                {
                    if (go != null)
                        go.GetComponent<PoliceBehaviour>().Day();
                }

                GameObject ob = Instantiate(policemen_prebab[Random.Range(0, policemen_prebab.Length - 1)], GameObject.Find("Entrance").transform.position, Quaternion.Euler(0, 90, 0));
                ob.GetComponent<PoliceBehaviour>().behaviour = PoliceBehaviour.TypeAction.Receptionist;
                policemen.Add(ob);

                timer1 = 0;
                timer2 = -15;
                timer3 = 0;

                actions = true;
            }

        }
        else if(!actions && !day)
        {
            patrol = 0;
            //Change the behavior of all entities to Night
            foreach (GameObject go in citizens)
            {
                if (go != null)
                    go.GetComponent<CitizenBehaviour>().Night();
            }
            for (int i = policemen.Count - 1 ; i >= 0; --i)
            {
                GameObject go = policemen[i];
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

        //Resets the counter to show all possible agents
        if (c1 == citizens_prebab.Length)
            c1 = 0;
        if (c2 == policemen_prebab.Length)
            c2 = 0;
        if (c3 == criminals_prebab.Length)
            c3 = 0;

        //Changes the cycle of day and night
        if (cycle >= 120)
        {
            day = !day;
            cycle = 0;
            actions = false;
        }
    }

    public void addCitizen(Vector3 pos)
    {
        citizens.Add(Instantiate(citizens_prebab[c1], pos, Quaternion.Euler(0, 90, 0)));
        c1++;
    }
    public void addPolicemen(Vector3 pos)
    {
        policemen.Add(Instantiate(policemen_prebab[c2], pos, Quaternion.Euler(0, 90, 0)));
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

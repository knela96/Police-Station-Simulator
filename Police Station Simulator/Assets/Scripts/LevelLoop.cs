using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NodeCanvas.Framework;

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
    bool actions = true;
    Vector3 vec;

    public GameObject receptionist = null;

    public AssignPoints assign;
    public AssignDesk desks;
    public AssignCell cells;

    MoneyBar money;
    HealthBar popul;
    public int spawnagents;

    // Start is called before the first frame update
    void Awake()
    {
        assign = GameObject.Find("Sofas").GetComponent<AssignPoints>();
        vec = GameObject.Find("Entrance").transform.position;
        desks = GameObject.Find("Desks").GetComponent<AssignDesk>();
        cells = GameObject.Find("Cells").GetComponent<AssignCell>();
        popul = GameObject.Find("Healthbar").GetComponent<HealthBar>();
        money = GameObject.Find("Money").GetComponent<MoneyBar>();
    }

    private void Start()
    {
        policemen.Add(Instantiate(policemen_prebab[Random.Range(0, policemen_prebab.Length - 1)], GameObject.Find("Entrance").transform.position, Quaternion.Euler(0, 90, 0)));
        policemen[0].GetComponent<PoliceBehaviour>().receptionist = true;
        policemen[0].GetComponent<GraphOwner>().enabled = true;
        spawnagents = desks.desksav;
    }

    // Update is called once per frame
    void Update()
    {
        if (day)
        {
            //Spawn Entites evey x time
            if (cycle - timer1 > 5)//15
            {
                if (assign.numAssigned <= assign.points.Count)
                {
                    timer1 = cycle;
                    addCitizen();
                }
            }
            if (cycle - timer2 > 1.5)
            {
                timer2 = cycle;
                if (spawnagents > 0)
                {
                    addPolicemen();
                    spawnagents--;
                }
            }
            //if (cycle - timer3 > 10)//31
            //{
            //    timer3 = cycle;
            //    addCriminal();
            //}
            if (!actions)
            {
                spawnagents = desks.desksav;
                //Change the behaviour to Day
                foreach (GameObject go in policemen)
                {
                    if (go != null)
                        go.GetComponent<PoliceBehaviour>().Day();
                }

                foreach (GameObject go in criminals)
                {
                    if (go != null)
                        go.GetComponent<CriminalBehavior>().Day();
                }

                GameObject ob = Instantiate(policemen_prebab[Random.Range(0, policemen_prebab.Length - 1)], GameObject.Find("Entrance").transform.position, Quaternion.Euler(0, 90, 0));
                ob.GetComponent<PoliceBehaviour>().receptionist = true;
                ob.GetComponent<GraphOwner>().enabled = true;
                policemen.Add(ob);

                timer1 = cycle;
                timer2 = cycle;
                timer3 = cycle;

                actions = true;
            }

        }
        else if(!actions && !day)
        {
            patrol = 0;

            money.updateMoney(desks.num_active * -10);

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

        //Changes the cycle of day and night
        if (cycle >= 120)
        {
            day = !day;
            cycle = 0;
            actions = false;
        }
    }

    

    public void addCitizen()
    {
        GameObject go = Instantiate(citizens_prebab[c1], vec, Quaternion.Euler(0, 90, 0));
        go.GetComponent<GraphOwner>().enabled = true;
        citizens.Add(go);
        c1++;
        if (c1 == citizens_prebab.Length)
            c1 = 0;
    }
    public bool addPolicemen()
    {
        if (desks.FreeDesks())
        {
            GameObject go = Instantiate(policemen_prebab[c2], vec, Quaternion.Euler(0, 90, 0));
            go.GetComponent<GraphOwner>().enabled = true;
            policemen.Add(go);
            c2++;
            if (c2 == policemen_prebab.Length)
                c2 = 0;
            return true;
        }
        return false;
    }
    public bool addCriminal()
    {
        if (cells.FreeCells())
        {
            GameObject go = Instantiate(criminals_prebab[c3], vec, Quaternion.Euler(0, 90, 0));
            go.GetComponent<GraphOwner>().enabled = true;
            criminals.Add(go);
            c3++;
            if (c3 == criminals_prebab.Length)
                c3 = 0;
            return true;
        }
        return false;
    }

    public GameObject getCriminal()
    {
        for(int i = 0;i < criminals.Count; ++i)
        {
            if (criminals[i] != null && criminals[i].GetComponent<CriminalBehavior>().free && !criminals[i].GetComponent<CriminalBehavior>().toExit && !criminals[i].GetComponent<CriminalBehavior>().assigned)
            {
                criminals[i].GetComponent<CriminalBehavior>().assigned = true;
                return criminals[i];
            }
        }
        return null;
    }

    public float getCycle()
    {
        return cycle;
    }

    
}

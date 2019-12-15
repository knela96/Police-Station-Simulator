using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using NodeCanvas.Framework;

public class LevelLoop : MonoBehaviour
{
    public GameObject[] citizens_prebab;
    public GameObject[] policemen_prebab;
    public GameObject[] criminals_prebab;

    public List<GameObject> citizens;
    public List<GameObject> policemen;
    public List<GameObject> criminals;

    public Text obj1;
    public Text obj2;
    public int num_liberated;
    public int num_escaped;

    public AudioSource office_lt;
    public AudioSource cell_lt;

    public AudioClip office_day;
    public AudioClip office_night;

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
    float timer;

    public GameObject receptionist = null;

    public AssignPoints assign;
    public AssignDesk desks;
    public AssignCell cells;

    public MoneyBar money;
    public HealthBar popul;
    public int spawnagents;

    public bool stop_Game;

    // Start is called before the first frame update
    void Awake()
    {
        assign = GameObject.Find("Sofas").GetComponent<AssignPoints>();
        vec = GameObject.Find("Entrance").transform.position;
        desks = GameObject.Find("Desks").GetComponent<AssignDesk>();
        cells = GameObject.Find("Cells").GetComponent<AssignCell>();
        popul = GameObject.Find("Healthbar").GetComponent<HealthBar>();
        money = GameObject.Find("Money").GetComponent<MoneyBar>();
        stop_Game = false;
    }

    private void Start()
    {
        policemen.Add(Instantiate(policemen_prebab[Random.Range(0, policemen_prebab.Length - 1)], GameObject.Find("Entrance").transform.position, Quaternion.Euler(0, 90, 0)));
        policemen[0].GetComponent<PoliceBehaviour>().receptionist = true;
        policemen[0].GetComponent<GraphOwner>().enabled = true;
        spawnagents = desks.desksav;
        office_lt.PlayDelayed(12);
        office_lt.volume = 0;
        timer = Time.time;
        num_liberated = 0;
        num_escaped = 0;
}

    // Update is called once per frame
    void Update()
    {
        if (!stop_Game) {
            if (office_lt.volume < 0.15 && office_lt.isPlaying && Time.time - timer < 17)
                office_lt.volume = office_lt.volume + 0.01f * Time.deltaTime;

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
                if (!actions)
                {
                    spawnagents = desks.desksav;
                    office_lt.spatialBlend = 1;
                    office_lt.clip = office_day;
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
                office_lt.volume = 0;
                office_lt.spatialBlend = 0;
                office_lt.clip = office_night;
                money.updateMoney(desks.num_active * -10);
                money.StartAnim(desks.num_active * -10,1);

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

            if (!day)
            {
                if (office_lt.volume > 0.0f)
                    office_lt.volume = office_lt.volume - 0.1f * Time.deltaTime;
            }

            cycle += Time.deltaTime;

            //Resets the counter to show all possible agents

            //Changes the cycle of day and night
            if (cycle >= 120)
            {
                timer = Time.time;
                day = !day;
                cycle = 0;
                actions = false;
            }
        }
        else
        {
            if (!actions)
            {
                for (int i = 0; i < citizens.Count; ++i)
                {
                    GameObject go = citizens[i];
                    if (go != null)
                        Destroy(go);
                }
                for (int i = 0; i < policemen.Count; ++i)
                {
                    GameObject go = policemen[i];
                    if (go != null)
                        Destroy(go);
                }
                for (int i = 0; i < criminals.Count; ++i)
                {
                    GameObject go = criminals[i];
                    if (go != null)
                        Destroy(go);
                }
                GameObject.Find("Environment_Office").SetActive(false);
                GameObject.Find("Environment_Cells").SetActive(false);
                GameObject.Find("Environment_Outside").SetActive(false);
                actions = true;
            }
        }
        UpdateObj1();
        UpdateObj2();
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

    public void UpdateObj1()
    {
        if (Input.GetKeyDown("f1"))
        {
            num_liberated = 5;
        }
        obj1.text = string.Format("Liberated: ({0}/5)",num_liberated);
        GameObject go = GameObject.Find("Mission");
        if (num_liberated == 5 && go != null)
        {
            popul.updatePopul(30);
            //WIN CONDITION
            GameObject.Find("Mission").SetActive(false);
            stop_Game = true;
            actions = false;
            GameObject.Find("Effects").GetComponent<AudioSource>().Play();
        }

    }
    public void UpdateObj2()
    {
        if (Input.GetKeyDown("f2"))
        {
            num_escaped = 3;
        }
        obj2.text = string.Format("Escaped: ({0}/2)",num_escaped);
        GameObject go = GameObject.Find("Mission");
        if (num_escaped == 3 && go != null)
        {
            popul.updatePopul(-30);
            GameObject.Find("Mission").SetActive(false);
            stop_Game = true;
            actions = false;
            GameObject.Find("Effects").GetComponent<AudioSource>().Play();
            //LOSE CONDITION
        }
    }

}

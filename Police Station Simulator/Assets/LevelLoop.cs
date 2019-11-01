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

    float timer1 = 10;
    float timer2 = 20;
    float timer3 = 30;


    bool day = true;

    // Start is called before the first frame update
    void Awake()
    {
        Vector3 vec = GameObject.Find("Entrance").transform.position;

        citizens.Add(Instantiate(citizens_prebab[Random.Range(0, citizens_prebab.Length - 1)], GameObject.Find("Entrance").transform.position, Quaternion.Euler(0, 90, 0)));
        policemen.Add(Instantiate(policemen_prebab[Random.Range(0, policemen_prebab.Length - 1)], GameObject.Find("Point").transform.position, Quaternion.Euler(0, 90, 0)));

        StartCoroutine(addCitizen(vec,timer1));
        StartCoroutine(addPolicemen(vec, timer2));
        StartCoroutine(addCriminal(vec, timer3));
    }

    // Update is called once per frame
    void Update()
    {
        if (day)
        {
        }
        else
        {

        }
    }

    IEnumerator addPolicemen(Vector3 pos, float delay)
    {
        yield return new WaitForSeconds(delay);
        policemen.Add(Instantiate(policemen_prebab[Random.Range(0, policemen_prebab.Length - 1)], pos, Quaternion.Euler(0, 90, 0)));
    }
    IEnumerator addCitizen(Vector3 pos, float delay)
    {
        yield return new WaitForSeconds(delay);
        citizens.Add(Instantiate(citizens_prebab[Random.Range(0, citizens_prebab.Length - 1)], pos, Quaternion.Euler(0, 90, 0)));
    }
    IEnumerator addCriminal(Vector3 pos, float delay)
    {
        yield return new WaitForSeconds(delay);
        criminals.Add(Instantiate(criminals_prebab[Random.Range(0, criminals_prebab.Length - 1)], pos, Quaternion.Euler(0, 90, 0)));
    }
}

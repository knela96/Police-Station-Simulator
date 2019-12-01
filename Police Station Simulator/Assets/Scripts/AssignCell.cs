using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AssignCell : MonoBehaviour
{

    public List<GameObject> cells;
    public int cellsav = 0;

    // Use this for initialization
    void Awake()
    {
        foreach (Transform child in gameObject.transform)
        {
            Transform c = child.transform.Find("Point");
            cells.Add(c.gameObject); //Stores the current desk points
            if (c.gameObject.active == true)
                cellsav++;
        }

    }

    public bool FreeCells()
    {
        for (int i = 0; i < cells.Count; ++i)
        {
            if (cells[i].GetComponent<Cell>().isAvailable() && cells[i].active)
                return true;
        }
        return false;
    }

    // Update is called once per frame
    void Update()
    {

    }
}

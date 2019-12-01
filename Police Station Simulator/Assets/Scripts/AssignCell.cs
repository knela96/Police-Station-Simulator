using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AssignCell : MonoBehaviour
{

    public List<GameObject> cells;
    public int cellsav = 2;
    // Use this for initialization
    void Awake()
    {
        foreach (Transform child in gameObject.transform)
        {
            Transform c = child.transform;
            cells.Add(c.gameObject);
            //for (int i = 0; i < child.childCount; ++i)
            //{
            //    Transform c1 = c.GetChild(i);
            //    if (c1.name == "Point")
            //        cells.Add(c1.gameObject); //Stores the current desk points
            //}
        }

    }

    public bool FreeCells()
    {
        foreach (Transform child in gameObject.transform)
        {
            Transform c = child.transform;
            if (c.GetComponent<Cell>().isAvailable())
                return true;
        }
        return false;
    }

    // Update is called once per frame
    void Update()
    {

    }
}

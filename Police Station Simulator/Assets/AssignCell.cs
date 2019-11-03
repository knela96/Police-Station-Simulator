using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AssignCell : MonoBehaviour
{

    public List<GameObject> cells;
    // Use this for initialization
    void Awake()
    {
        foreach (Transform child in gameObject.transform)
        {
            cells.Add(child.gameObject); //Stores all cells
        }

    }

    // Update is called once per frame
    void Update()
    {

    }
}

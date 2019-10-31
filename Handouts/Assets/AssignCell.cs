using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AssignCell : MonoBehaviour
{

    public List<GameObject> cells;
    // Use this for initialization
    void Start()
    {
        foreach (Transform child in gameObject.transform)
        {
            cells.Add(child.gameObject);
        }

    }

    // Update is called once per frame
    void Update()
    {

    }
}

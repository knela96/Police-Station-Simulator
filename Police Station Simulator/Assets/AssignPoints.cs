﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AssignPoints : MonoBehaviour
{
    public int numAssigned = 0;
    public List<GameObject> points;
    // Use this for initialization
    void Awake()
    {
        foreach (Transform child in gameObject.transform)
        {
            Transform c = child.transform;
            for (int i = 0; i < child.childCount; ++i) {
                Transform c1 = c.GetChild(i);
                if(c1.name == "Point")
                    points.Add(c1.gameObject); //Stores the current desk points
            }
        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

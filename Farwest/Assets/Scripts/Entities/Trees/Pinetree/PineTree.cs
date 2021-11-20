using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PineTree : MonoBehaviour
{

    private GameObject self;
    private int firstStage_hacks = 30; // How many chops/hacks are needed to cut down the tree

    public bool IsOccupied = false;

    void Awake()
    {
        self = gameObject;
    }

   
    void Update()
    {
        
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tree : MonoBehaviour
{

    private GameObject self;
    public int firstStage_hacks = 10; // How many chops/hacks are needed to cut down the tree

    public bool IsOccupied = false;
    public bool FirstStageDone = false;


    // GameObjects that will be instantiatet for later stages of the tree chopping:
    public GameObject RootStump;

    void Awake()
    {
        self = gameObject;
    }

    void CutDown()
    {
        if(!FirstStageDone)
        {

        }
    }


    void Update()
    {
        
    }
}

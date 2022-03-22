using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreePart : MonoBehaviour
{


    private GameObject self;

    public bool IsOccupied = false;

    public int SawingHealth = 5; // The health of the treepart - Used for sawing sequence

    public GameObject LeftPart;
    public GameObject RightPart;

    void Awake()
    {
        self = gameObject;
        self.tag = "treepart";
    }

    
    void Update()
    {
        
    }
}

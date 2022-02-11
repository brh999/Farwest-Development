using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreePart : MonoBehaviour
{


    private GameObject self;

    public bool IsOccupied = false;

    void Awake()
    {
        self = gameObject;
        self.tag = "treepart";
    }

    
    void Update()
    {
        
    }
}

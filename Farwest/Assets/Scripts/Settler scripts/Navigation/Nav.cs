using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Nav : MonoBehaviour
{

    private GameObject self;
    private NavMeshAgent selfNav;
    public bool isRunning = false;

    void Awake()
    {
        self = gameObject;
        selfNav = GetComponent<NavMeshAgent>();
    }

    // Walk to a destination 
    public void WalkToDestination(Vector3 dest)
    {
        selfNav.destination = dest;
    }

    
}

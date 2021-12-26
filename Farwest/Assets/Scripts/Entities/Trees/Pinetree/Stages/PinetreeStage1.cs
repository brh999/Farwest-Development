using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class PinetreeStage1 : MonoBehaviour
{

    private GameObject self;

    private Rigidbody selfRB;

    private Sound selfS;

    private bool treeHasFallen = false;
    public bool IsOccupied = false;

    void Awake()
    {
        self = gameObject;
        selfRB = self.GetComponent<Rigidbody>();
        selfS = self.GetComponent<Sound>();
    }

    void Start()
    {
        Invoke("FreezePosition", 5f);
    }
    
    void FreezePosition()
    {
        selfRB.constraints = RigidbodyConstraints.FreezeAll;
    }

    void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "outsideGround" && !treeHasFallen)
        {
            treeHasFallen = true;
            selfS.PlaySound("treefall_end", 0.5f, 1, 0);
        }
    }

    void FixedUpdate()
    {
       
    }
}

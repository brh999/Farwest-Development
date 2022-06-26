using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PinetreeStage2 : MonoBehaviour
{
    // Entities/data to move on:
    public GameObject Stage2_UpperPart;
    public GameObject Stage2_LowerPart;

    private GameObject self;
    public GameObject OccupiedOwner;

    private Anim occupiedOwnerAnim;

    private Rigidbody selfRB;

    private CapsuleCollider selfCC;

    public bool IsOccupied = false;

    public int HacksLeft;

    public Vector3 TreeRightSide;


    void Awake()
    {
        self = gameObject;
    }

    
    public void UpdateOwner(GameObject owner)
    {
        if (owner)
        {
            OccupiedOwner = owner;
            occupiedOwnerAnim = owner.GetComponent<Anim>();
        }
        else if (owner == null)
        {
            OccupiedOwner = null;
        }
    }


    public void CutDown()
    {
        self.GetComponent<Sound>().Tree_CutDownAudio();

        GameObject stage3 = Instantiate(Stage2_LowerPart, self.transform.position, self.transform.rotation);
        GameObject stage2_UpperPart = Instantiate(Stage2_UpperPart, self.transform.position + self.transform.forward * 5.5f, self.transform.rotation);
       
        stage3.GetComponent<PinetreeStage3>().TreeRightSide = TreeRightSide;
        Destroy(self);
    }



    void Update()
    {
        
    }
}

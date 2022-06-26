using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PinetreeStage5 : MonoBehaviour
{

    //Entities/data to move on/use:
    public GameObject Stage5_UpperPart;
    public GameObject Stage5_LowerPart;
   
    public Vector3 TreeRightSide;

    public GameObject OccupiedOwner;
    private GameObject self;

    public bool IsOccupied = false;

    public float HacksLeft;

    private Anim occupiedOwnerAnim;

    void Awake()
    {
        self = gameObject;
        HacksLeft = 1; // The amount of hacks that's needed in order to chop down the tree part
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
        GameObject UpperPart = Instantiate(Stage5_UpperPart, self.transform.position + self.transform.forward * 1.52f, self.transform.rotation);
        GameObject LowerPart = Instantiate(Stage5_LowerPart, self.transform.position + self.transform.forward * 0.05f, self.transform.rotation);
        Destroy(self);
    }

    

    void Update()
    {
        
    }
}

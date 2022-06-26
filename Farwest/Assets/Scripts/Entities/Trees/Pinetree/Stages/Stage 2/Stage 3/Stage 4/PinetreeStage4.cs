using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PinetreeStage4 : MonoBehaviour
{
    //Entities/data to move on/use:
    public GameObject Stage4_UpperPart;
    public GameObject Stage4_LowerPart;
    
    public GameObject OccupiedOwner;
    private GameObject self;

    public bool IsOccupied = false;

    public float HacksLeft;

    public Vector3 TreeRightSide;

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

        GameObject UpperPart = Instantiate(Stage4_LowerPart, self.transform.position + self.transform.forward * 1.27f, self.transform.rotation);
        GameObject LowerPart = Instantiate(Stage4_LowerPart, self.transform.position + self.transform.forward * 0.05f, self.transform.rotation);
        Destroy(self);
    }


}

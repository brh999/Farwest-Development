using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PinetreeStage3 : MonoBehaviour
{
    //Entities/data to move on/use:
    public GameObject Stage3_UpperPart;
    public GameObject Stage3_LowerPart;
  
    public GameObject OccupiedOwner;
    private GameObject self;

    public bool IsOccupied = false;

    public float HacksLeft;

    public Vector3 TreeRightSide;


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
        }
        else if (owner == null)
        {
            OccupiedOwner = null;
        }
    }


    public void CutDown()
    {
        self.GetComponent<Sound>().Tree_CutDownAudio();
        GameObject s4u = Instantiate(Stage3_UpperPart, self.transform.position + self.transform.forward * 2.94f, self.transform.rotation);
        s4u.GetComponent<PinetreeStage4>().TreeRightSide = TreeRightSide;

        GameObject s4l = Instantiate(Stage3_LowerPart, self.transform.position, self.transform.rotation);
        s4l.GetComponent<PinetreeStage5>().TreeRightSide = TreeRightSide;
        Destroy(self);
    }


    void Update()
    {
        
    }
}

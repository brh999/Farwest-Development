using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lumberworkstation : MonoBehaviour
{
    public bool IsOccupied = false;
    public bool RightValveIsAdjusted = false;
    public bool LeftValveIsAdjusted = false;
    public bool SawIsTaken = false;

    public GameObject CurrentHoldingRS; // The current treepart the workstation is holding
    public GameObject CurrentHoldingRS2; // The second treepart the workstation is holding

    public Transform SawTransform;

    private Animator selfAnim;

    void Awake()
    {
        selfAnim = GetComponent<Animator>();
        SawTransform = gameObject.transform.Find("carpenter_saw");
    }

   public GameObject GetResource()
    {
        if(CurrentHoldingRS2 != null)
        {
            return CurrentHoldingRS2;
        }
        else if(CurrentHoldingRS != null)
        {
            return CurrentHoldingRS;
        }
        else
        {
            return null;
        }
    }

    public void RemoveResource(int num)
    {
        switch(num)
        {
            case 1:
                CurrentHoldingRS = null;
            break;


            case 2:
                CurrentHoldingRS2 = null;
            break;
        }
    }

    public bool IsLastResource()
    {
        if(CurrentHoldingRS2)
        {
            return false;
        }
        else if(CurrentHoldingRS)
        {
            return true;
        }
        else
        {
            return true;
        }
    }
    
    void Update()
    {
        
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Axe : MonoBehaviour
{

    private GameObject owner;

    private LogicTasks owner_LogicTasks;

    private Sound owner_Sound;

    private BoxCollider owner_Collider;



    void Awake()
    {
        // We loop through the axe's parents until we get the core parent (Which is the settler)
        GameObject currentParent = gameObject;
        while (!currentParent.name.Contains("Settler"))
        {
            owner = currentParent.transform.parent.gameObject;
            currentParent = owner;
        }

        owner.GetComponent<Logic>().CurrentTool = gameObject;

        owner_LogicTasks = owner.GetComponent<LogicTasks>();
        owner_Sound = owner.GetComponent<Sound>();
        owner_Collider = owner.GetComponent<BoxCollider>();

    }


     void OnCollisionEnter(Collision collision)
    {
        GameObject tree = collision.transform.gameObject;
        if(tree.tag == "tree")
        {
            if(tree == owner_LogicTasks.taskObject)
            {
                Tree treeScript = tree.GetComponent<Tree>();
                if (treeScript.FirstStage_hacks > 0)
                {
                    owner_Sound.PlaySound("axechop", 0.1f, 1, 0);
                    treeScript.FirstStage_hacks -= 1;
                    treeScript.WoodFlakeSequence();
                }
                else if(treeScript.CurrentStage == 0)
                {
                    treeScript.CutDown();
                }
            }
        }
    }

    void Update()
    {
       
    }
}

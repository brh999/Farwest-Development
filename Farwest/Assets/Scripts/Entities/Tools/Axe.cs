using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Axe : MonoBehaviour
{

    private GameObject owner;

    private LogicTasks owner_LogicTasks;

    private Sound owner_Sound;

    private BoxCollider owner_Collider;

    private float chopDelay = 8f; // The delay to prevent the axe from chopping multiple times within a frame
    private float chopDelayTimer;

    private bool readyToChop = true;
    public bool shouldChop = false;

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

        chopDelay = chopDelayTimer;
    }


    private string PickRandomWoodBreakSound()
    {
        int choosenint = Random.Range(0, 4);
        string[] sounds = {"wood_break1", "wood_break2", "wood_break3", "wood_break4", "wood_break5"};
        return sounds[choosenint];
    }

     void OnCollisionEnter(Collision collision)
    {
        if (readyToChop && shouldChop)
        {
            GameObject tree = collision.transform.gameObject;
            if (tree.tag == "tree")
            {
                if (tree == owner_LogicTasks.TaskObject)
                {
                    Tree treeScript = tree.GetComponent<Tree>();
                    if (treeScript.FirstStage_hacks > 0)
                    {
                        owner_Sound.PlaySound("axechop", 0.1f, 1, 0);
                        treeScript.FirstStage_hacks -= 1;
                        treeScript.WoodFlakeSequence();
                    }
                    else if (treeScript.CurrentStage == 0)
                    {
                        owner_Sound.PlaySound("axechop", 0.1f, 1, 0);
                        owner_Sound.PlaySound(PickRandomWoodBreakSound(), 0.1f, 1, 0);
                        treeScript.WoodFlakeSequence();
                        treeScript.CutDown();
                    }
                    readyToChop = false;
                }
            }
            else if (tree.tag == "treestage2")
            {
                PinetreeStage2 pinetreeStage2 = tree.GetComponent<PinetreeStage2>();
                if (pinetreeStage2.HacksLeft > 0)
                {
                    owner_Sound.PlaySound("axechop", 0.1f, 1, 0);
                    pinetreeStage2.HacksLeft -= 1;
                    pinetreeStage2.WoodFlakeSequence();
                }
                else
                {
                    owner_Sound.PlaySound("axechop", 0.1f, 1, 0);
                    owner_Sound.PlaySound(PickRandomWoodBreakSound(), 0.1f, 1, 0);
                    pinetreeStage2.WoodFlakeSequence();
                    pinetreeStage2.CutDown();
                }
                readyToChop = false;
            }
            else if (tree.tag == "treestage3")
            {
                PinetreeStage3 pinetreeStage3 = tree.GetComponent<PinetreeStage3>();
                if (pinetreeStage3.HacksLeft > 0)
                {
                    owner_Sound.PlaySound("axechop", 0.1f, 1, 0);
                    pinetreeStage3.HacksLeft -= 1;
                    pinetreeStage3.WoodFlakeSequence();
                }
                else
                {
                    owner_Sound.PlaySound("axechop", 0.1f, 1, 0);
                    owner_Sound.PlaySound(PickRandomWoodBreakSound(), 0.1f, 1, 0);
                    pinetreeStage3.WoodFlakeSequence();
                    pinetreeStage3.CutDown();
                }
                readyToChop = false;
            }
        }
    }

    void FixedUpdate()
    {
       if(!readyToChop)
        {
            if(chopDelayTimer > 0)
            {
                chopDelayTimer -= Time.deltaTime;
            }
            else if(chopDelayTimer <= 0)
            {
                readyToChop = true;
                chopDelayTimer = chopDelay;
            }
        }
    }
}

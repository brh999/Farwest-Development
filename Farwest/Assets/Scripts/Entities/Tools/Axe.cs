using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Axe : MonoBehaviour
{

    private GameObject owner;
    public GameObject Woodflake1;
    public GameObject Woodflake2;
    public GameObject Woodflake3;

    private LumberjackTask owner_LumberTask;

    private Sound owner_Sound;

    private BoxCollider owner_Collider;

    private float chopDelay = 8f; // The delay to prevent the axe from chopping multiple times within a frame
    private float chopDelayTimer;

    private bool readyToChop = true;
    public bool shouldChop = false;

    void Start()
    {
        // We loop through the axe's parents until we get the core parent (Which is the settler)
        GameObject currentParent = gameObject;
        while (!currentParent.name.Contains("Settler"))
        {
            owner = currentParent.transform.parent.gameObject;
            currentParent = owner;
        }

        owner_LumberTask = owner.GetComponent<LumberjackTask>();
        owner_Sound = owner.GetComponent<Sound>();
        owner_Collider = owner.GetComponent<BoxCollider>();

        chopDelay = chopDelayTimer;
    }

 

     void OnCollisionEnter(Collision collision)
    {
        if (readyToChop && shouldChop)
        {
            GameObject tree = collision.transform.gameObject;
            if (tree.tag == "tree")
            {
                if (tree == owner.GetComponent<Logic>().TaskObject)
                {
                    Tree treeScript = tree.GetComponent<Tree>();
                    if (treeScript.FirstStage_hacks > 0)
                    {
                        owner_Sound.PlaySound("axechop", 0.1f, 1, 0);
                        treeScript.FirstStage_hacks -= 1;
                        WoodFlakeSequence();
                    }
                    else
                    {
                        owner_Sound.PlaySound("axechop", 0.1f, 1, 0);
                        WoodFlakeSequence();
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
                    WoodFlakeSequence();
                }
                else
                {
                    owner_Sound.PlaySound("axechop", 0.1f, 1, 0);
                    WoodFlakeSequence();
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
                    WoodFlakeSequence();
                }
                else
                {
                    owner_Sound.PlaySound("axechop", 0.1f, 1, 0);
                    WoodFlakeSequence();
                    pinetreeStage3.CutDown();
                }
                readyToChop = false;
            }
            else if (tree.tag == "treestage4")
            {
                PinetreeStage4 pinetreeStage4 = tree.GetComponent<PinetreeStage4>();
                if (pinetreeStage4.HacksLeft > 0)
                {
                    owner_Sound.PlaySound("axechop", 0.1f, 1, 0);
                    pinetreeStage4.HacksLeft -= 1;
                    WoodFlakeSequence();
                }
                else
                {
                    owner_Sound.PlaySound("axechop", 0.1f, 1, 0);
                    WoodFlakeSequence();
                    pinetreeStage4.CutDown();
                }
                readyToChop = false;
            }
            else if (tree.tag == "treestage5")
            {
                PinetreeStage5 pinetreeStage5 = tree.GetComponent<PinetreeStage5>();
                if (pinetreeStage5.HacksLeft > 0)
                {
                    owner_Sound.PlaySound("axechop", 0.1f, 1, 0);
                    pinetreeStage5.HacksLeft -= 1;
                    WoodFlakeSequence();
                }
                else
                {
                    owner_Sound.PlaySound("axechop", 0.1f, 1, 0);
                    WoodFlakeSequence();
                    pinetreeStage5.CutDown();
                }
                readyToChop = false;
            }
        }
    }


    public void WoodFlakeSequence() // Simulate wood flaking off from the wood the axe is hitting
    {
        Transform ownerAxePosition = owner.GetComponent<Logic>().CurrentTool.transform;
        GameObject flake1 = Instantiate(Woodflake1, ownerAxePosition.position + ownerAxePosition.right * 0.1f + ownerAxePosition.up * -0.1f + ownerAxePosition.forward * 0.3f, ownerAxePosition.rotation);
        flake1.AddComponent<Rigidbody>();
        flake1.AddComponent<MeshCollider>();
        GameObject flake2 = Instantiate(Woodflake2, ownerAxePosition.position + ownerAxePosition.right * 0.1f + ownerAxePosition.up * -0.1f + ownerAxePosition.forward * 0.3f, ownerAxePosition.rotation);
        flake2.AddComponent<Rigidbody>();
        flake2.AddComponent<MeshCollider>();
        GameObject flake3 = Instantiate(Woodflake3, ownerAxePosition.position + ownerAxePosition.right * 0.1f + ownerAxePosition.up * -0.1f + ownerAxePosition.forward * 0.3f, ownerAxePosition.rotation);
        flake3.AddComponent<Rigidbody>();
        flake3.AddComponent<MeshCollider>();

        flake1.GetComponent<MeshCollider>().convex = true;
        flake2.GetComponent<MeshCollider>().convex = true;
        flake3.GetComponent<MeshCollider>().convex = true;

        // Add force to the rb of the wood flakes
        Rigidbody flake1RB = flake1.GetComponent<Rigidbody>();
        Rigidbody flake2RB = flake2.GetComponent<Rigidbody>();
        Rigidbody flake3RB = flake3.GetComponent<Rigidbody>();

        int woodflakeIndex = 3; // The amount of wood flakes we have to flake off
        int woodflakeToGo = 0;
        while (woodflakeToGo <= woodflakeIndex - 1)
        {
            switch (woodflakeToGo)
            {
                case 0:
                    flake1RB.AddForce(ownerAxePosition.right * Random.Range(-3f, -1f) + ownerAxePosition.up * Random.Range(-2f, -1f) + ownerAxePosition.forward * Random.Range(-2f, 2f), ForceMode.Impulse);
                    break;

                case 1:
                    flake2RB.AddForce(ownerAxePosition.right * Random.Range(-3f, -1f) + ownerAxePosition.forward * Random.Range(-4f, 0f), ForceMode.Impulse);
                    break;

                case 2:
                    flake3RB.AddForce(ownerAxePosition.right * Random.Range(-3f, -1f) + ownerAxePosition.forward * Random.Range(0f, 4f), ForceMode.Impulse);
                    break;
            }
            woodflakeToGo += 1;
        }

        IEnumerator WoodFlakeSequenceTimer()
        {
            yield return new WaitForSeconds(2f);
            Destroy(flake1);
            Destroy(flake2);
            Destroy(flake3);
            StopCoroutine(WoodFlakeSequenceTimer());
        }
        StartCoroutine(WoodFlakeSequenceTimer());
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

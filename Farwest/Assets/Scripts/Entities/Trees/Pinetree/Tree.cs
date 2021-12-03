using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tree : MonoBehaviour
{

    private GameObject self;
    public GameObject OccupiedOwner;

    public int firstStage_hacks = 10; // How many chops/hacks are needed to cut down the tree

    public bool IsOccupied = false;
    public bool FirstStageDone = false;
    


    // GameObjects that will be instantiatet for later stages of the tree chopping:
    public GameObject RootStump;
    public GameObject UpperPart;
    public GameObject Woodflake1;
    public GameObject Woodflake2;
    public GameObject Woodflake3;

    void Awake()
    {
        self = gameObject;

    }

    void CutDown()
    {
        if(!FirstStageDone)
        {

        }
    }

    public void WoodFlakeSequence()
    {
        Transform ownerAxePosition = OccupiedOwner.GetComponent<Logic>().CurrentTool.transform;
        GameObject flake1 = Instantiate(Woodflake1, ownerAxePosition.position + new Vector3(ownerAxePosition.localPosition.x * 3000, 0, 0), ownerAxePosition.localRotation);
        flake1.AddComponent<Rigidbody>();
        GameObject flake2 = Instantiate(Woodflake2, ownerAxePosition.position + new Vector3(ownerAxePosition.localPosition.x * 3000, 0, 0), ownerAxePosition.rotation);
        flake2.AddComponent<Rigidbody>();
        GameObject flake3 = Instantiate(Woodflake3, ownerAxePosition.position + new Vector3(ownerAxePosition.localPosition.x * 3000, 0, 0), ownerAxePosition.rotation);
        flake3.AddComponent<Rigidbody>();

        IEnumerator WoodFlakeSequenceTimer()
        {
            while (true)
            {
                yield return new WaitForSeconds(2f);
                Destroy(flake1);
                Destroy(flake2);
                Destroy(flake3);
                StopCoroutine(WoodFlakeSequenceTimer());
            }
        }
        StartCoroutine(WoodFlakeSequenceTimer());
    }


    void Update()
    {
        
    }
}

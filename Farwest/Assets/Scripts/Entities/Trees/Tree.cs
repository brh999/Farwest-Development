using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tree : MonoBehaviour
{

    private GameObject self;
    public GameObject OccupiedOwner;

    public int FirstStage_hacks; // How many chops/hacks are needed to cut down the tree
    public int CurrentStage = 0;

    public string TreeType; // The type of tree

    public bool IsOccupied = false;

    private float UpperPartCollisionHeight;

    // GameObjects that will be instantiatet for later stages of the tree chopping:
    public GameObject TreeStump;

    public GameObject Stage1_UpperPart;
    public GameObject Stage1_UpperPartPlucked;

    public GameObject Woodflake1;
    public GameObject Woodflake2;
    public GameObject Woodflake3;

    public AudioClip TreeFallOffStartSound;
    public AudioClip TreeFallOffEndSound;

    void Awake()
    {
        self = gameObject;

        // We set the collision height of the upper part of the tree, when it tilts off
        switch(TreeType)
        {
            case("pinetree"):
            UpperPartCollisionHeight = 0.08f;
            break;
        }
    }

    public void CutDown()
    {
        if(CurrentStage == 0)
        {
            // This part will simulate the tree to tilt down, by removing the main tree, and adding in a seperate root
            // + upper tree part, and then adding force to the upper part to make it fall off/tilt off
            CurrentStage = 1;
            GameObject stump = Instantiate(TreeStump, self.transform.position, self.transform.rotation);
            stump.AddComponent<Stump>();
            stump.AddComponent<MeshCollider>();
            
            GameObject stage1 = Instantiate(Stage1_UpperPart, self.transform.position + self.transform.forward * 1.5f, self.transform.rotation);

            stage1.AddComponent<Rigidbody>();
            stage1.AddComponent<CapsuleCollider>();
            stage1.AddComponent<AudioSource>();
            stage1.AddComponent<Sound>();
            stage1.AddComponent<PinetreeStage1>();

            stage1.GetComponent<Sound>().AddAudioClip(TreeFallOffStartSound);
            stage1.GetComponent<Sound>().AddAudioClip(TreeFallOffEndSound);

            stage1.tag = "treestage1";

            Sound stage1S = stage1.GetComponent<Sound>();
            CapsuleCollider stage1CC = stage1.GetComponent<CapsuleCollider>();
            Rigidbody stage1RB = stage1.GetComponent<Rigidbody>();
            PinetreeStage1 stage1PTS = stage1.GetComponent<PinetreeStage1>();
            stage1PTS.stage1_UpperPartPlucked = Stage1_UpperPartPlucked;
            stage1S.PlaySound("treefall_start", 0.5f, 1f, 0);
            stage1CC.direction = self.GetComponent<CapsuleCollider>().direction;
            stage1CC.height = UpperPartCollisionHeight;
            stage1CC.radius = self.GetComponent<CapsuleCollider>().radius;
            stage1CC.center = new Vector3(self.GetComponent<CapsuleCollider>().center.x, self.GetComponent<CapsuleCollider>().center.y, 0.04f);
            stage1RB.AddForce(OccupiedOwner.transform.forward * 0.1f, ForceMode.Impulse);
            

            Object.Destroy(self);
        }
    }

   


    public void WoodFlakeSequence()
    {
        // Add in the wood flakes to be bursted off the tree
        Transform ownerAxePosition = OccupiedOwner.GetComponent<Logic>().CurrentTool.transform;
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
            switch(woodflakeToGo)
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


    void Update()
    {
      
    }
}

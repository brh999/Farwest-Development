using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PinetreeStage2 : MonoBehaviour
{
    // Entities/data to move on:
    public GameObject Woodflake1;
    public GameObject Woodflake2;
    public GameObject Woodflake3;
    public GameObject Stage2_UpperPart;
    public GameObject Stage2_LowerPart;
    public GameObject Stage3_UpperPart;
    public GameObject Stage3_LowerPart;
    public GameObject Stage4_UpperPart;
    public GameObject Stage4_LowerPart;
    public GameObject Stage5_UpperPart;
    public GameObject Stage5_LowerPart;
    public AudioClip WoodBreak1;
    public AudioClip WoodBreak2;
    public AudioClip WoodBreak3;
    public AudioClip WoodBreak4;
    public AudioClip WoodBreak5;


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
        HacksLeft = 1; // The amount of hacks that's needed in order to chop down the tree part
        self.AddComponent<Rigidbody>();
        self.AddComponent<CapsuleCollider>();
        selfCC = self.GetComponent<CapsuleCollider>();
        selfRB = self.GetComponent<Rigidbody>();

        selfRB.constraints = RigidbodyConstraints.FreezeAll;

        selfCC.direction = 2;
        selfCC.center = new Vector3(-0.00012f, -0.00012f, 0.04f);
        selfCC.radius = 0.00228712F;
        selfCC.height = 0.08f;

        self.AddComponent<AudioSource>();
        self.AddComponent<Sound>();

        self.tag = "treestage2";
    }

    public void InitSounds()
    {
        Sound soundscript = self.GetComponent<Sound>();
        soundscript.sounds[0] = WoodBreak1;
        soundscript.sounds[1] = WoodBreak2;
        soundscript.sounds[2] = WoodBreak3;
        soundscript.sounds[3] = WoodBreak4;
        soundscript.sounds[4] = WoodBreak5;
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
        stage2_UpperPart.AddComponent<TreePart>();
        stage3.AddComponent<PinetreeStage3>();

        PinetreeStage3 stage3script = stage3.GetComponent<PinetreeStage3>();
        stage3script.Stage3_UpperPart = Stage3_UpperPart;
        stage3script.Stage3_LowerPart = Stage3_LowerPart;
        stage3script.Stage4_UpperPart = Stage4_UpperPart;
        stage3script.Stage4_LowerPart = Stage4_LowerPart;
        stage3script.Stage5_UpperPart = Stage5_UpperPart;
        stage3script.Stage5_LowerPart = Stage5_LowerPart;
        stage3script.TreeRightSide = TreeRightSide;
        stage3script.Woodflake1 = Woodflake1;
        stage3script.Woodflake2 = Woodflake2;
        stage3script.Woodflake3 = Woodflake3;

        stage3script.WoodBreak1 = WoodBreak1;
        stage3script.WoodBreak2 = WoodBreak2;
        stage3script.WoodBreak3 = WoodBreak3;
        stage3script.WoodBreak4 = WoodBreak4;
        stage3script.WoodBreak5 = WoodBreak5;
        stage3script.InitSounds();

        Destroy(self);
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

    void Update()
    {
        
    }
}

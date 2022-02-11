using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class PinetreeStage1 : MonoBehaviour
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
    public GameObject stage1_UpperPartPlucked;

    private Anim occupiedOwnerAnim;

    private Rigidbody selfRB;

    private Sound selfS;

    private bool treeHasFallen = false;
    private bool hasBeenPlucked = false;
    private bool shouldBePlucked = false;
    public bool IsOccupied = false;

    public Vector3 TreeRightSide;

    private float pluckTime; // The time for how long the plucking process should take, should be set in "Awake" method

    void Awake()
    {
        self = gameObject;
        selfRB = self.GetComponent<Rigidbody>();
        selfS = self.GetComponent<Sound>();
        pluckTime = Random.Range(10, 20);
        self.tag = "treestage1";
    }

    void Start()
    {
        Invoke("FreezePosition", 5f);
    }
    
    // Freeze rigidbody on all constraints 
    void FreezePosition()
    {
        selfRB.constraints = RigidbodyConstraints.FreezeAll;
    }

    // Update the current occupied owner of this entity (Which settler is plucking it currently)
    public void UpdateOwner(GameObject owner)
    {
        if(owner)
        {
            OccupiedOwner = owner;
            occupiedOwnerAnim = owner.GetComponent<Anim>();
            TreeRightSide = owner.transform.right;
        }
        else if(owner == null)
        {
            OccupiedOwner = null;
        }
    }


    // The countdown for the tree to be plucked, has succeded 
    private void plucktree_done()
    {
        if (!hasBeenPlucked)
        {
            hasBeenPlucked = true;
            GameObject stage1Plucked = Instantiate(stage1_UpperPartPlucked, self.transform.position, self.transform.rotation);
            stage1Plucked.AddComponent<PinetreeStage2>();
            PinetreeStage2 s1p = stage1Plucked.GetComponent<PinetreeStage2>();

            s1p.Woodflake1 = Woodflake1;
            s1p.Woodflake2 = Woodflake2;
            s1p.Woodflake3 = Woodflake3;
            s1p.Stage2_UpperPart = Stage2_UpperPart;
            s1p.Stage2_LowerPart = Stage2_LowerPart;
            s1p.Stage3_UpperPart = Stage3_UpperPart;
            s1p.Stage3_LowerPart = Stage3_LowerPart;
            s1p.Stage4_UpperPart = Stage4_UpperPart;
            s1p.Stage4_LowerPart = Stage4_LowerPart;
            s1p.Stage5_UpperPart = Stage5_UpperPart;
            s1p.Stage5_LowerPart = Stage5_LowerPart;
            s1p.TreeRightSide = TreeRightSide;

            s1p.WoodBreak1 = WoodBreak1;
            s1p.WoodBreak2 = WoodBreak2;
            s1p.WoodBreak3 = WoodBreak3;
            s1p.WoodBreak4 = WoodBreak4;
            s1p.WoodBreak5 = WoodBreak5;
            s1p.InitSounds();

            Destroy(self);
        }
    }

    // Start/continue the countdown to pluck the tree 
    public void plucktree_start()
    {
        if(!shouldBePlucked)
        {
            shouldBePlucked = true;
        }
    }

    // Stop the countdown in FixedUpdate to pluck the tree, and make the owner play "plucking_end" animation
    public void plucktree_stop()
    {
        if(shouldBePlucked)
        {
            shouldBePlucked = false;
            if(OccupiedOwner)
            {
                occupiedOwnerAnim.PlayAnimation("plucking_end", 0);
            }
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "outsideGround" && !treeHasFallen)
        {
            treeHasFallen = true;
            selfS.PlaySound("treefall_end", 0.5f, 1, 0);
        }
    }

    void FixedUpdate()
    {
        if (shouldBePlucked)
        {
            // If we reach countdown of zero of pluckTime
            if (pluckTime <= 0 && !hasBeenPlucked)
            {
                plucktree_done();
            }
            else  // Else substract pluckTime towards 0 
            {
                pluckTime -= Time.deltaTime;
            }
        }
    }
}

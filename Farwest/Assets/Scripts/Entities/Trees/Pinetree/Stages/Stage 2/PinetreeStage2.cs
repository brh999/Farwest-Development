using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PinetreeStage2 : MonoBehaviour
{
    public GameObject Woodflake1;
    public GameObject Woodflake2;
    public GameObject Woodflake3;

    private GameObject self;
    public GameObject OccupiedOwner;

    private Anim occupiedOwnerAnim;

    private Rigidbody selfRB;

    private CapsuleCollider selfCC;

    public bool IsOccupied = false;

    public int HacksLeft;

    void Awake()
    {
        self = gameObject;
        HacksLeft = Random.Range(5, 8); // The amount of hacks that's needed in order to chop down the tree part
        self.AddComponent<Rigidbody>();
        self.AddComponent<CapsuleCollider>();
        selfCC = self.GetComponent<CapsuleCollider>();
        selfRB = self.GetComponent<Rigidbody>();

        selfRB.constraints = RigidbodyConstraints.FreezeAll;

        selfCC.direction = 2;
        selfCC.center = new Vector3(-0.00012f, -0.00012f, 0.04f);
        selfCC.radius = 0.00228712F;
        selfCC.height = 0.08f;
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

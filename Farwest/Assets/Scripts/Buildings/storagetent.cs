using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class storagetent : MonoBehaviour
{
    // Wood resource:
    public int Wood = 0;
    public GameObject woodDeco1; 
    public GameObject woodDeco12;
    public GameObject woodDeco2;
    public GameObject woodDeco3;

    private GameObject woodDeco1_object;
    private GameObject woodDeco12_object;
    private GameObject woodDeco2_object;
    private GameObject woodDeco3_object;

    private GameObject woodDeco1_Placement; 
    private GameObject woodDeco12_Placement;
    private GameObject woodDeco2_Placement;
    private GameObject woodDeco3_Placement;

    // Script variables
    private GameObject self;

    void Awake()
    {
        self = gameObject;
        woodDeco1_Placement = self.transform.Find("woodDeco1_placement").gameObject;
        woodDeco12_Placement = self.transform.Find("woodDeco12_placement").gameObject;
        woodDeco2_Placement = self.transform.Find("woodDeco2_placement").gameObject;
        woodDeco3_Placement = self.transform.Find("woodDeco3_placement").gameObject;

    }


    private void UpdateResources()
    {
        if(Wood >= 4)
        {
            if (!woodDeco3_object)
            {
                woodDeco3_object = Instantiate(woodDeco3, woodDeco3_Placement.transform.position, woodDeco3_Placement.transform.rotation); 
            }
        }
        else if(Wood >= 3)
        {
            if (!woodDeco2_object)
            {
                woodDeco2_object = Instantiate(woodDeco2, woodDeco2_Placement.transform.position, woodDeco2_Placement.transform.rotation); 
            }

            if (woodDeco3_object)
            {
                Destroy(woodDeco3_object);
            }
        }
        else if(Wood >= 2)
        {
            if (!woodDeco12_object)
            {
                woodDeco12_object = Instantiate(woodDeco12, woodDeco12_Placement.transform.position, woodDeco12_Placement.transform.rotation);
            }

            if (woodDeco2_object)
            {
                Destroy(woodDeco2_object);
            }

            if (woodDeco3_object)
            {
                Destroy(woodDeco3_object);
            }
        }
        else if(Wood >= 1)
        {
            
            if(!woodDeco1_object)
            {
                woodDeco1_object = Instantiate(woodDeco1, woodDeco1_Placement.transform.position, woodDeco1_Placement.transform.rotation);
            }

            if (woodDeco12_object)
            {
                Destroy(woodDeco12_object);
            }

            if(woodDeco2_object)
            {
                Destroy(woodDeco2_object);
            }

            if (woodDeco3_object)
            {
                Destroy(woodDeco3_object);
            }
        }
    }

    public void AddResource(string type)
    {
        switch(type)
        {
            case "wood":
                Wood += 1;
                break;

            case "resource2":

                break;

        }
        UpdateResources();
    }

    public void RemoveResource(string type)
    {
        switch (type)
        {
            case "wood":
                Wood -= 1;
                break;

            case "resource2":

                break;

        }
        UpdateResources();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

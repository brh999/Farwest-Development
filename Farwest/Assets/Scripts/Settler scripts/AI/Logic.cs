using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Logic : MonoBehaviour
{
    private GameObject self;

    public GameObject CurrentTool;
    public GameObject CurrentCarrying;

    public GameObject RightHand;

    public GameObject Spine2;

    public GameObject TaskObject;

    public GameObject UsingWorkstation;

    public GameObject[] Tools = new GameObject[20];

    private LumberjackTask lumberjackTask;

    private Anim selfAnim;

    public string Work = "idle";
    public string Task = "none";

    public bool HasDestination = false;
    public bool CurrentToolIsHolstered;
    public bool IsCarryingResource = false;


    public string[] ExistingTools = {"tools_axe"}; // All the existing tools a settler can use


    // ----------- TOOL HOLSTER/UNHOLSTER POSITION AND QUATERNION DATA --------------------
    // tools_axe
    private Vector3 toolsAxe_unholstered_pos = new Vector3(0.14f, 0.0f, 0.25f);
    private Quaternion toolsAxe_unholstered_qua = new Quaternion(-14f, -8f, 78.228f, 0);
    private Vector3 toolsAxe_holstered_pos = new Vector3(0f, 0.2f, -0.00025f);
    private Quaternion toolsAxe_holstered_qua = new Quaternion(-90f, 0f, 0f, 0);


    private void Awake()
    {
        self = gameObject;
        selfAnim = GetComponent<Anim>();
    }

    private void Start()
    {
        GameObject findRightHand = self.transform.Find("Armature").Find("Root").Find("Spine1").Find("Spine2").Find("Spine3").Find("Shoulder.R").Find("UpperArm.R").Find("LowerArm.R").Find("Hand.R").gameObject;
        if(findRightHand)
        {
            RightHand = findRightHand;
        }

        GameObject findSpine2 = self.transform.Find("Armature").Find("Root").Find("Spine1").Find("Spine2").gameObject;
        if(findSpine2)
        {
            Spine2 = findSpine2;
        }

        CurrentToolIsHolstered = true;

        SetWork(Work);
    }



    // Check if a GameObject is within the world from a tag, and with custom properties for each GameObject type
    public bool isGameObjectInWorld(string objectTag)
    {
        bool isInWorld = false;
        GameObject[] objects = GameObject.FindGameObjectsWithTag(objectTag);
        if (objects.Length != 0)
        {
            foreach (GameObject o in objects)
            {
                switch (objectTag) // Use this switch to customize the properties of the GameObject, before returning true
                {
                    case "treepart":
                        if (!o.GetComponent<TreePart>().IsOccupied)
                        {
                            isInWorld = true;
                        }
                        break;

                    case "treepartcut":
                        if (!o.transform.Find("lumberworkstation").gameObject.GetComponent<Lumberworkstation>().IsOccupied)
                        {

                        }
                        break;


                    case "treestage5":
                        if (!o.GetComponent<PinetreeStage5>().IsOccupied)
                        {
                            isInWorld = true;
                        }
                        break;


                    case "treestage4":
                        if (!o.GetComponent<PinetreeStage4>().IsOccupied)
                        {
                            isInWorld = true;
                        }
                        break;


                    case "treestage3":
                        if (!o.GetComponent<PinetreeStage3>().IsOccupied)
                        {
                            isInWorld = true;
                        }
                        break;


                    case "treestage2":
                        if (!o.GetComponent<PinetreeStage2>().IsOccupied)
                        {
                            isInWorld = true;
                        }
                        break;


                    case "treestage1":
                        if (!o.GetComponent<PinetreeStage1>().IsOccupied)
                        {
                            isInWorld = true;
                        }
                        break;


                    case "tree":
                        if (!o.GetComponent<Tree>().IsOccupied)
                        {
                            isInWorld = true;
                        }
                        break;

                }
            }
        }

        if (isInWorld)
        {
            return true;
        }
        else
        {
            return false;
        }
    }



    // Find an object by tag, and return the closest one to a settler/player
    public GameObject FindClosestObject(string tag)
    {

        GameObject[] objects = GameObject.FindGameObjectsWithTag(tag);
        if (objects.Length != 0) // Is the array empty?
        {
            float[] objectDistances = new float[objects.Length];
            int index = 0;
            foreach (GameObject i in objects)
            {
                objectDistances[index] = Vector3.Distance(self.transform.position, i.transform.position);
                index++;
            }

            bool objectHasntBeenFound = true;
            int indexToUse = -1;
            while (objectHasntBeenFound)
            {
                int indexFound = 0;
                foreach (float distance in objectDistances)
                {
                    if (distance == Mathf.Min(objectDistances))
                    {
                        if (tag == "tree" && objects[indexFound]) //Tree Entity
                        {
                            if (!objects[indexFound].GetComponent<Tree>().IsOccupied)
                            {
                                objects[indexFound].GetComponent<Tree>().IsOccupied = true;
                                indexToUse = indexFound;
                                objectHasntBeenFound = false;
                                break;
                            }
                            else
                            {
                                objects[indexFound] = null;
                                objectDistances[indexFound] = 99999999;
                                break;
                            }
                        }
                        else if (tag == "treestage1" && objects[indexFound])  //A tree entity in stage 1
                        {
                            if (!objects[indexFound].GetComponent<PinetreeStage1>().IsOccupied)
                            {
                                objects[indexFound].GetComponent<PinetreeStage1>().IsOccupied = true;
                                objects[indexFound].GetComponent<PinetreeStage1>().UpdateOwner(self);
                                indexToUse = indexFound;
                                objectHasntBeenFound = false;
                                break;
                            }
                            else
                            {
                                objects[indexFound] = null;
                                objectDistances[indexFound] = 99999999;
                                break;
                            }
                        }
                        else if (tag == "treestage2" && objects[indexFound])  //A tree entity in stage 2
                        {
                            if (!objects[indexFound].GetComponent<PinetreeStage2>().IsOccupied)
                            {
                                objects[indexFound].GetComponent<PinetreeStage2>().IsOccupied = true;
                                objects[indexFound].GetComponent<PinetreeStage2>().UpdateOwner(self);
                                indexToUse = indexFound;
                                objectHasntBeenFound = false;
                                break;
                            }
                            else
                            {
                                objects[indexFound] = null;
                                objectDistances[indexFound] = 99999999;
                                break;
                            }
                        }
                        else if (tag == "treestage3" && objects[indexFound])  //A tree entity in stage 3
                        {
                            if (!objects[indexFound].GetComponent<PinetreeStage3>().IsOccupied)
                            {
                                objects[indexFound].GetComponent<PinetreeStage3>().IsOccupied = true;
                                objects[indexFound].GetComponent<PinetreeStage3>().UpdateOwner(self);
                                indexToUse = indexFound;
                                objectHasntBeenFound = false;
                                break;
                            }
                            else
                            {
                                objects[indexFound] = null;
                                objectDistances[indexFound] = 99999999;
                                break;
                            }
                        }
                        else if (tag == "treestage4" && objects[indexFound])  //A tree entity in stage 4
                        {
                            if (!objects[indexFound].GetComponent<PinetreeStage4>().IsOccupied)
                            {
                                objects[indexFound].GetComponent<PinetreeStage4>().IsOccupied = true;
                                objects[indexFound].GetComponent<PinetreeStage4>().UpdateOwner(self);
                                indexToUse = indexFound;
                                objectHasntBeenFound = false;
                                break;
                            }
                            else
                            {
                                objects[indexFound] = null;
                                objectDistances[indexFound] = 99999999;
                                break;
                            }
                        }
                        else if (tag == "treestage5" && objects[indexFound])  //A tree entity in stage 5
                        {
                            if (!objects[indexFound].GetComponent<PinetreeStage5>().IsOccupied)
                            {
                                objects[indexFound].GetComponent<PinetreeStage5>().IsOccupied = true;
                                objects[indexFound].GetComponent<PinetreeStage5>().UpdateOwner(self);
                                indexToUse = indexFound;
                                objectHasntBeenFound = false;
                                break;
                            }
                            else
                            {
                                objects[indexFound] = null;
                                objectDistances[indexFound] = 99999999;
                                break;
                            }
                        }
                        else if (tag == "treepart" && objects[indexFound])  //A part of a tree
                        {
                            if (!objects[indexFound].GetComponent<TreePart>().IsOccupied)
                            {
                                objects[indexFound].GetComponent<TreePart>().IsOccupied = true;
                                indexToUse = indexFound;
                                objectHasntBeenFound = false;
                                break;
                            }
                            else
                            {
                                objects[indexFound] = null;
                                objectDistances[indexFound] = 99999999;
                                break;
                            }
                        }
                        else if (tag == "lumberworkstation" && objects[indexFound])  //A lumber workstation
                        {
                            if (!objects[indexFound].GetComponent<Lumberworkstation>().IsOccupied)
                            {
                                objects[indexFound].GetComponent<Lumberworkstation>().IsOccupied = true;
                                indexToUse = indexFound;
                                objectHasntBeenFound = false;
                                break;
                            }
                            else
                            {
                                objects[indexFound] = null;
                                objectDistances[indexFound] = 99999999;
                                break;
                            }
                        }
                        else
                        {
                            indexToUse = indexFound;
                            objectHasntBeenFound = false;
                            break;
                        }
                    }
                    indexFound++;
                }
            }

            return objects[indexToUse];

        }
        else
        {
            print("No object found with tag: " + tag);
            return null;
        }
    }



    public void ToggleHolsterTool() // Toggle holster the current tool the AI has
    {
        if (CurrentTool)
        {
            IEnumerator ToggleHolsterTool()
            {
                yield return new WaitForSeconds(1f);
                Transform CTT = CurrentTool.transform;
                switch (CurrentToolIsHolstered)
                {
                    case true:
                        switch (CurrentTool.name)
                        {
                            case "tools_axe":
                                CTT.SetParent(RightHand.transform);
                                CTT.rotation = RightHand.transform.rotation;
                                CTT.RotateAround(CTT.position, CTT.right, toolsAxe_unholstered_qua.x);
                                CTT.RotateAround(CTT.position, CTT.up, toolsAxe_unholstered_qua.y);
                                CTT.RotateAround(CTT.position, CTT.forward, toolsAxe_unholstered_qua.z);
                                CTT.position = RightHand.transform.position + CTT.right * toolsAxe_unholstered_pos.x + CTT.up * toolsAxe_unholstered_pos.y + CTT.forward * toolsAxe_unholstered_pos.z;
                                break;
                        }
                        CurrentToolIsHolstered = false;
                        break;

                    case false:
                        switch (CurrentTool.name)
                        {
                            case "tools_axe":
                                CTT.SetParent(Spine2.transform);
                                CTT.rotation = Spine2.transform.rotation;
                                CTT.RotateAround(CTT.position, CTT.right, toolsAxe_holstered_qua.x);
                                CTT.RotateAround(CTT.position, CTT.up, toolsAxe_holstered_qua.y);
                                CTT.RotateAround(CTT.position, CTT.forward, toolsAxe_holstered_qua.z);
                                CTT.position = Spine2.transform.position + CTT.right * toolsAxe_holstered_pos.x + CTT.up * toolsAxe_holstered_pos.y + CTT.forward * toolsAxe_holstered_pos.z;
                                break;
                        }
                        CurrentToolIsHolstered = true;
                        break;
                }
                StopCoroutine(ToggleHolsterTool());
            }
            StartCoroutine(ToggleHolsterTool());
            selfAnim.PlayAnimation("toggleHolster", 0);
        }
    }

    public GameObject GetCurrentTool() // Get the current tool the settler has equipped
    {
        if(RightHand)
        {
            foreach (string tool in ExistingTools)
            {
                GameObject thetool = RightHand.transform.Find(tool).gameObject;
                if(thetool)
                {
                    return thetool;
                }
            }
        }
        if(Spine2)
        {
            foreach (string tool in ExistingTools)
            {
                GameObject thetool = Spine2.transform.Find(tool).gameObject;
                if (thetool)
                {
                    return thetool;
                }
            }
        }

        print("Could not find any tool on settler:" + gameObject.name);
        return null;
    }

    private GameObject FindTool(string tool) // Returns the gameobject of the tool you are looking for
    {
        foreach(GameObject q in Tools)
        {
            if(q.name == tool)
            {
                return q;
            }
        }
        return null;
    }

    public void GiveTool(string toolname) // Give tool to a settler
    {
        if(!CurrentTool)
        {
            switch(toolname)
            {
                case "tools_axe":
                    GameObject tool = Instantiate(FindTool(toolname));
                    Transform toolT = tool.transform;
                    toolT.SetParent(Spine2.transform);
                    toolT.rotation = Spine2.transform.rotation;
                    toolT.RotateAround(toolT.position, toolT.right, toolsAxe_holstered_qua.x);
                    toolT.RotateAround(toolT.position, toolT.up, toolsAxe_holstered_qua.y);
                    toolT.RotateAround(toolT.position, toolT.forward, toolsAxe_holstered_qua.z);
                    toolT.position = Spine2.transform.position + toolT.right * toolsAxe_holstered_pos.x + toolT.up * toolsAxe_holstered_pos.y + toolT.forward * toolsAxe_holstered_pos.z;
                    CurrentTool = tool;
                    tool.name = toolname;
                    break;
            }
            CurrentToolIsHolstered = true;
        }
    }

    public void SetWork(string work) // Set the work a settler has to do
    {
        switch (work)
        {
            case "lumberjack":
                Destroy(CurrentTool);
                GiveTool("tools_axe");
                Work = "lumberjack";
                self.AddComponent<LumberjackTask>();
                break;

            case "idle":
                Destroy(CurrentTool);
                Work = "idle";
                self.AddComponent<IdleTask>();
                break;
        }
        StartWorkTask();
    }

    public void StartWorkTask() // Starts the settler's task
    {
        switch(Work)
        {
            case "lumberjack": // Tasks for lumberjack
                self.GetComponent<LumberjackTask>().CalcLumberTask();
                break;

            case "idle":
                self.GetComponent<IdleTask>().CalcIdleTask();
                break;
        }
    }

    

}

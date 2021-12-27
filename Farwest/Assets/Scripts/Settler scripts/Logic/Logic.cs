using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Logic : MonoBehaviour
{
    private GameObject self;

    private LogicTasks selfLogicTasks;

    private Anim selfAnim;

    public string Work = "none";
    public string Task = "none";

    public bool HasDestination = false;
    public bool ToolIsHolstered;

    public GameObject CurrentTool;

    public string CurrentToolName;

    public GameObject RightHand;

    public string[] ExistingTools = {"tools_axe"}; // All the existing tools a settler can use

    private void Awake()
    {
        self = gameObject;
        selfLogicTasks = GetComponent<LogicTasks>();
        selfAnim = GetComponent<Anim>();
        Work = "lumberjack";
    }

    private void Start()
    {
        GameObject findRightHand = self.transform.Find("Armature").Find("Root").Find("Spine1").Find("Spine2").Find("Spine3").Find("Shoulder.R").Find("UpperArm.R").Find("LowerArm.R").Find("Hand.R").gameObject;
        if(findRightHand)
        {
            RightHand = findRightHand;
        }
        CurrentTool = GetCurrentTool();
        CurrentToolName = CurrentTool.name;
        ToolIsHolstered = false;
        selfLogicTasks.LumberTask("choptree");
    }

    public void ToggleHolsterTool()
    {
        if (CurrentTool)
        {
            switch (ToolIsHolstered)
            {
                case true:
                    ToolIsHolstered = false;
                    break;

                case false:
                    ToolIsHolstered = true;
                    break;
            }
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
        print("Could not find any tool on settler:" + gameObject.name);
        return null;
    }


}

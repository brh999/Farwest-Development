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
    public bool CurrentToolIsHolstered;

    public GameObject CurrentTool;

    public GameObject RightHand;

    public GameObject Spine2;

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

        GameObject findSpine2 = self.transform.Find("Armature").Find("Root").Find("Spine1").Find("Spine2").gameObject;
        if(findSpine2)
        {
            Spine2 = findSpine2;
        }

        CurrentTool = GetCurrentTool();
        CurrentToolIsHolstered = false;
        selfLogicTasks.LumberTask("choptree");
    }


    // tools_axe
    private Vector3 toolsAxe_unholstered_pos = new Vector3(-0.00011f, 0.00576f, 0.00563f);
    private Quaternion toolsAxe_unholstered_qua = new Quaternion(-16.135f, -8.512f, 78.228f, 0);
    private Vector3 toolsAxe_holstered_pos = new Vector3(0f, 0f, -0.00025f);
    private Quaternion toolsAxe_holstered_qua = new Quaternion(-90f, 0f, 0f, 0);

    public void ToggleHolsterTool()
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
                                CTT.position = RightHand.transform.position;
                                CTT.localPosition = toolsAxe_unholstered_pos;
                                CTT.rotation = RightHand.transform.rotation;
                                CTT.RotateAround(CTT.position, CTT.right, toolsAxe_unholstered_qua.x);
                                CTT.RotateAround(CTT.position, CTT.up, toolsAxe_unholstered_qua.y);
                                CTT.RotateAround(CTT.position, CTT.forward, toolsAxe_unholstered_qua.z);
                                CTT.position += CTT.right * 0f + CTT.up * 0f + CTT.forward * 0f;
                                break;
                        }
                        CurrentToolIsHolstered = false;
                        break;

                    case false:
                        switch (CurrentTool.name)
                        {
                            case "tools_axe":
                                CTT.SetParent(Spine2.transform);
                                CTT.position = Spine2.transform.position;
                                CTT.localPosition = toolsAxe_holstered_pos;
                                CTT.rotation = Spine2.transform.rotation;
                                CTT.RotateAround(CTT.position, CTT.right, toolsAxe_holstered_qua.x);
                                CTT.RotateAround(CTT.position, CTT.up, toolsAxe_holstered_qua.y);
                                CTT.RotateAround(CTT.position, CTT.forward, toolsAxe_holstered_qua.z);
                                CTT.position += CTT.right * 0 + CTT.up * 0.2f + CTT.forward * 0;
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


}

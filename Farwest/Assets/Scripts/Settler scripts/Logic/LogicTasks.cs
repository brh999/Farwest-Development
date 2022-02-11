using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class LogicTasks : MonoBehaviour
{
    

    private GameObject self;
    public GameObject TaskObject;

    private NavMeshAgent selfAgent;

    private Logic selfLogic;

    private Nav selfNavigation;

    private Anim selfAnim;

    private Sound selfSound;


    private bool hasReachedTaskObject;
    public bool isChopping = false;



    void Awake()
    {

        self = gameObject;

        selfAgent = GetComponent<NavMeshAgent>();
        selfLogic = GetComponent<Logic>();
        selfAnim = GetComponent<Anim>();
        selfNavigation = GetComponent<Nav>();
        selfSound = GetComponent<Sound>();
      
    }



    // Lumberjack methods:

    public void CalcLumberTask()
    {
        bool treepart = isGameObjectInWorld("treepart");
        if(treepart)
        {
            LumberTask("collectwood");
        }
        else
        {
            LumberTask("choptree");
        }
    }

    public void LumberTask(string stage)
    {
        if(stage == "choptree") // Find and chop a tree
        {
            GameObject treeobject = FindClosestObject("tree");
            if(treeobject)
            {
                selfLogic.Task = "choptree";
                treeobject.GetComponent<Tree>().OccupiedOwner = self;
                selfNavigation.WalkToDestination(treeobject.transform.position, 3f);
                TaskObject = treeobject;
                selfLogic.HasDestination = true;
                hasReachedTaskObject = false;
            }
        }
        else if(stage == "choptree_stage1") // Find and work with a tree in stage 1
        {
            GameObject treeObject = FindClosestObject("treestage1");
            if (treeObject)
            {
                selfLogic.Task = "choptree_stage1";
                selfNavigation.WalkToDestination(treeObject.transform.position + treeObject.transform.forward * 3 + self.transform.right * 1, 0f);
                TaskObject = treeObject;
            }
        }
        else if(stage == "choptree_stage2") // Find and work with a tree in stage 2
        {
            GameObject treeObject = FindClosestObject("treestage2");
            if(treeObject)
            {
                selfLogic.Task = "choptree_stage2";
                TaskObject = treeObject;
                if (selfLogic.CurrentToolIsHolstered)
                {
                    selfLogic.ToggleHolsterTool();
                    IEnumerator DelayForNav()
                    {
                        yield return new WaitForSeconds(2f);
                        selfNavigation.WalkToDestination(treeObject.transform.position + treeObject.transform.forward * 5.4f + TaskObject.GetComponent<PinetreeStage2>().TreeRightSide * 1f, 0f);
                        StopCoroutine(DelayForNav());
                    }
                    StartCoroutine(DelayForNav());
                }
                else
                {
                    selfNavigation.WalkToDestination(treeObject.transform.position + treeObject.transform.forward * 5.4f + TaskObject.GetComponent<PinetreeStage2>().TreeRightSide * 1f, 0f);
                }
            }
        }
        else if(stage == "choptree_stage3")
        {
            GameObject treeObject = FindClosestObject("treestage3");
            if(treeObject)
            {
                TaskObject = treeObject;
                selfLogic.Task = "choptree_stage3";
                if (selfLogic.CurrentToolIsHolstered)
                {
                    selfLogic.ToggleHolsterTool();
                    IEnumerator DelayForNav()
                    {
                        yield return new WaitForSeconds(2f);
                        selfNavigation.WalkToDestination(treeObject.transform.position + treeObject.transform.forward * 3f + TaskObject.GetComponent<PinetreeStage3>().TreeRightSide * 1f, 0f);
                        StopCoroutine(DelayForNav());
                    }
                    StartCoroutine(DelayForNav());
                }
                else
                {
                    selfNavigation.WalkToDestination(treeObject.transform.position + treeObject.transform.forward * 3f + TaskObject.GetComponent<PinetreeStage3>().TreeRightSide * 1f, 0f);
                }
            }
        }
        else if (stage == "choptree_stage4")
        {
            GameObject treeObject = FindClosestObject("treestage4");
            if (treeObject)
            {
                TaskObject = treeObject;
                selfLogic.Task = "choptree_stage4";
                if (selfLogic.CurrentToolIsHolstered)
                {
                    selfLogic.ToggleHolsterTool();
                    IEnumerator DelayForNav()
                    {
                        yield return new WaitForSeconds(2f);
                        selfNavigation.WalkToDestination(treeObject.transform.position + treeObject.transform.forward * 1.15f + TaskObject.GetComponent<PinetreeStage4>().TreeRightSide * 1f, 0f);
                        StopCoroutine(DelayForNav());
                    }
                    StartCoroutine(DelayForNav());
                }
                else
                {
                    selfNavigation.WalkToDestination(treeObject.transform.position + treeObject.transform.forward * 1.15f + TaskObject.GetComponent<PinetreeStage4>().TreeRightSide * 1f, 0f);
                }
            }
        }
        else if (stage == "choptree_stage5")
        {
            GameObject treeObject = FindClosestObject("treestage5");
            if (treeObject)
            {
                TaskObject = treeObject;
                selfLogic.Task = "choptree_stage5";
                if (selfLogic.CurrentToolIsHolstered)
                {
                    selfLogic.ToggleHolsterTool();
                    IEnumerator DelayForNav()
                    {
                        yield return new WaitForSeconds(2f);
                        selfNavigation.WalkToDestination(treeObject.transform.position + treeObject.transform.forward * 1.4f + TaskObject.GetComponent<PinetreeStage5>().TreeRightSide * 1f, 0f);
                        StopCoroutine(DelayForNav());
                    }
                    StartCoroutine(DelayForNav());
                }
                else
                {
                    selfNavigation.WalkToDestination(treeObject.transform.position + treeObject.transform.forward * 1.4f + TaskObject.GetComponent<PinetreeStage5>().TreeRightSide * 1f, 0f);
                }
            }
        }
        else if(stage == "collectwood")
        {
            GameObject treeObject = FindClosestObject("treepart");
            if(treeObject)
            {
                TaskObject = treeObject;
                selfLogic.Task = "collectwood";
                if (!selfLogic.CurrentToolIsHolstered)
                {
                    selfLogic.ToggleHolsterTool();
                    IEnumerator DelayForNav()
                    {
                        yield return new WaitForSeconds(2f);
                        if (Vector3.Distance(self.transform.position, TaskObject.transform.position) <= 2)
                        {
                            CarryLog();
                        }
                        else
                        {
                            selfNavigation.WalkToDestination(treeObject.transform.position + treeObject.transform.forward * 1f, 2f);
                        }
                        StopCoroutine(DelayForNav());
                    }
                    StartCoroutine(DelayForNav());
                }
                else
                {
                    if (Vector3.Distance(self.transform.position, TaskObject.transform.position) <= 2)
                    {
                        CarryLog();
                    }
                    else
                    {
                        selfNavigation.WalkToDestination(treeObject.transform.position + treeObject.transform.forward * 1f, 2f);
                    }
                }
            }
        }
    }


    // Begin chopping sequence for LumberTask
    private void BeginChopping(string swingside)
    {
        float timerToActivateAxe = 0f;
        if (!isChopping)
        {
            switch (swingside)
            {
                case "horizontal":
                selfAnim.PlayAnimation("hacking_horizontal_start", 1f);
                selfAnim.PlayAnimation("hacking_horizontal", 1.8f);
                isChopping = true;
                timerToActivateAxe = 1.8f;
                break;

                case "vertical":
                selfAnim.PlayAnimation("hacking_vertical_start", 0f);
                selfAnim.PlayAnimation("hacking_vertical", 2.5f);
                isChopping = true;
                timerToActivateAxe = 2.5f;
                break;
            }
            IEnumerator ActivateChopping()
            {
                yield return new WaitForSeconds(timerToActivateAxe);
                selfLogic.CurrentTool.GetComponent<Axe>().shouldChop = true;
                StopCoroutine(ActivateChopping());
            }
            StartCoroutine(ActivateChopping());
        }
    }

    private Vector3 treepartPos = new Vector3(0.25f, 0f, -0.75f);
    private Quaternion treepartQua = new Quaternion(0f, 0f, 0f, 0);
    private void CarryLog()
    {
        if(TaskObject.tag == "treepart")
        {
            selfAnim.PlayAnimation("log_pickup", 0f);
            IEnumerator ParentLog()
            {
                yield return new WaitForSeconds(0.5f);
                Transform tot = TaskObject.transform;
                tot.SetParent(selfLogic.RightHand.transform);
                tot.rotation = selfLogic.RightHand.transform.rotation;
                tot.RotateAround(tot.position, tot.right, treepartQua.x);
                tot.RotateAround(tot.position, tot.up, treepartQua.y);
                tot.RotateAround(tot.position, tot.forward, treepartQua.z);
                tot.position = selfLogic.RightHand.transform.position + tot.right * treepartPos.x + tot.up * treepartPos.y + tot.forward * treepartPos.z;
                StopCoroutine(ParentLog());
            }
            StartCoroutine(ParentLog());
        }
    }


    // New work here etc.



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




    // Check if a GameObject is within the world from a tag, and with custom properties for each GameObject type
    private bool isGameObjectInWorld(string objectTag)
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




    // Update method for all jobs/tasks
    private void FixedUpdate()
    {
        switch(selfLogic.Work)
        {


            // -- Lumberjack job --
            case "lumberjack":


                if (selfLogic.Task == "choptree") // Find and chop a tree
                {
                    if (!hasReachedTaskObject && TaskObject) // If settler hasn't reached the tree
                    {
                        float distance = Vector3.Distance(self.transform.position, TaskObject.transform.position);

                        if (distance <= 3 && !hasReachedTaskObject)
                        {
                            hasReachedTaskObject = true;
                            BeginChopping("horizontal");
                            self.transform.LookAt(TaskObject.transform);
                        }
                    }
                    else if (hasReachedTaskObject && !TaskObject) // If the tree has been chopped down, and needs to go to stage1
                    {
                        hasReachedTaskObject = false;
                        isChopping = false;
                        selfLogic.CurrentTool.GetComponent<Axe>().shouldChop = false;
                        selfAnim.PlayAnimation("hacking_horizontal_end", 1f);
                        IEnumerator SwitchStageTo1()
                        {
                            yield return new WaitForSeconds(5f);
                            LumberTask("choptree_stage1");
                            StopCoroutine(SwitchStageTo1());
                        }
                        StartCoroutine(SwitchStageTo1());
                    }
                }
                else if (selfLogic.Task == "choptree_stage1") // Find and pluck a tree
                {
                    if (!hasReachedTaskObject && TaskObject) // If the settler hasn't reached the tree in stage 1
                    {
                        Vector3 stage1GameObjectPositionToLookAt = TaskObject.transform.position + TaskObject.transform.forward * 4;
                        float distance = Vector3.Distance(self.transform.position, stage1GameObjectPositionToLookAt + self.transform.right * 1);
                        if (distance <= 1.1f) // If we reach the tree in stage1 
                        {
                            hasReachedTaskObject = true;
                            IEnumerator Stage1Pluck() // Begin plucking sequence of the tree
                            {
                                yield return new WaitForSeconds(0.5f);
                                self.transform.LookAt(stage1GameObjectPositionToLookAt);
                                if (!selfLogic.CurrentToolIsHolstered)
                                {
                                    selfLogic.ToggleHolsterTool();
                                }
                                selfAnim.PlayAnimation("plucking_start", 1.5f);
                                selfAnim.PlayAnimation("plucking", 1.5f);
                                TaskObject.GetComponent<PinetreeStage1>().plucktree_start();
                                StopCoroutine(Stage1Pluck());
                            }
                            StartCoroutine(Stage1Pluck());
                        }
                    }
                    else if (hasReachedTaskObject && !TaskObject) // When the tree has been plucked
                    {
                        hasReachedTaskObject = false;
                        selfAnim.PlayAnimation("plucking_end", 1.5f);
                        IEnumerator SwitchStageTo2()
                        {
                            yield return new WaitForSeconds(2.5f);
                            LumberTask("choptree_stage2");
                            StopCoroutine(SwitchStageTo2());
                        }
                        StartCoroutine(SwitchStageTo2());
                    }
                }
                else if (selfLogic.Task == "choptree_stage2") // Find and begin chopping top of the tree
                {
                    if (!hasReachedTaskObject && TaskObject)
                    {
                        Vector3 stage2GameObjectPositionToLookAt = TaskObject.transform.position + TaskObject.transform.forward * 5.4f;
                        float distance = Vector3.Distance(self.transform.position, stage2GameObjectPositionToLookAt + TaskObject.GetComponent<PinetreeStage2>().TreeRightSide * 1f);
                        if (distance <= 1f) // If we reach the tree in stage2 
                        {
                            hasReachedTaskObject = true;
                            IEnumerator Stage2BeginChopping() // Begin vertically chopping the tree in stage 2
                            {
                                yield return new WaitForSeconds(1f);
                                self.transform.LookAt(stage2GameObjectPositionToLookAt);
                                if (selfLogic.CurrentToolIsHolstered)
                                {
                                    selfLogic.ToggleHolsterTool();
                                    IEnumerator BeginChoppingDelay()
                                    {
                                        yield return new WaitForSeconds(1.5f);
                                        BeginChopping("vertical");
                                        StopCoroutine(BeginChoppingDelay());
                                    }
                                    StartCoroutine(BeginChoppingDelay());
                                }
                                else
                                {
                                    BeginChopping("vertical");
                                }
                                StopCoroutine(Stage2BeginChopping());
                            }
                            StartCoroutine(Stage2BeginChopping());
                        }
                    }
                    else if (hasReachedTaskObject && !TaskObject) // When the upper part of the tree has ben cut off.
                    {
                        hasReachedTaskObject = false;
                        isChopping = false;
                        selfLogic.CurrentTool.GetComponent<Axe>().shouldChop = false;
                        selfAnim.PlayAnimation("hacking_vertical_end", 0.5f);
                        IEnumerator SwitchStageTo3()
                        {
                            yield return new WaitForSeconds(1.5f);
                            LumberTask("choptree_stage3");
                            StopCoroutine(SwitchStageTo3());
                        }
                        StartCoroutine(SwitchStageTo3());
                    }
                }
                else if (selfLogic.Task == "choptree_stage3") // Find and begin chopping lower section of the tree, in stage 3
                {
                    if (!hasReachedTaskObject && TaskObject)
                    {
                        Vector3 stage3GameObjectPositionToLookAt = TaskObject.transform.position + TaskObject.transform.forward * 3f;
                        float distance = Vector3.Distance(self.transform.position, stage3GameObjectPositionToLookAt + TaskObject.GetComponent<PinetreeStage3>().TreeRightSide * 1f);
                        if (distance <= 1f) // If we reach the tree in stage3
                        {
                            hasReachedTaskObject = true;
                            IEnumerator Stage3BeginChopping() // Begin vertically chopping the tree in stage 3
                            {
                                yield return new WaitForSeconds(1f);
                                self.transform.LookAt(stage3GameObjectPositionToLookAt);
                                if (selfLogic.CurrentToolIsHolstered)
                                {
                                    selfLogic.ToggleHolsterTool();
                                    IEnumerator BeginChoppingDelay()
                                    {
                                        yield return new WaitForSeconds(2.5f);
                                        BeginChopping("vertical");
                                        StopCoroutine(BeginChoppingDelay());
                                    }
                                    StartCoroutine(BeginChoppingDelay());
                                }
                                else
                                {
                                    BeginChopping("vertical");
                                }
                                StopCoroutine(Stage3BeginChopping());
                            }
                            StartCoroutine(Stage3BeginChopping());
                        }
                    }
                    else if (hasReachedTaskObject && !TaskObject) // When the part of the tree has ben cut off.
                    {
                        hasReachedTaskObject = false;
                        isChopping = false;
                        selfLogic.CurrentTool.GetComponent<Axe>().shouldChop = false;
                        selfAnim.PlayAnimation("hacking_vertical_end", 0.5f);
                        IEnumerator SwitchStageTo4()
                        {
                            yield return new WaitForSeconds(1.5f);
                            LumberTask("choptree_stage4");
                            StopCoroutine(SwitchStageTo4());
                        }
                        StartCoroutine(SwitchStageTo4());
                    }
                }
                else if (selfLogic.Task == "choptree_stage4") // Find and begin chopping lower section of the tree, in stage 4
                {
                    if (!hasReachedTaskObject && TaskObject)
                    {
                        Vector3 stage4GameObjectPositionToLookAt = TaskObject.transform.position + TaskObject.transform.forward * 1.25f;
                        float distance = Vector3.Distance(self.transform.position, stage4GameObjectPositionToLookAt + TaskObject.GetComponent<PinetreeStage4>().TreeRightSide * 1f);
                        if (distance <= 1f) // If we reach the tree in stage4
                        {
                            hasReachedTaskObject = true;
                            IEnumerator Stage3BeginChopping() // Begin vertically chopping the tree in stage 4
                            {
                                yield return new WaitForSeconds(1f);
                                self.transform.LookAt(stage4GameObjectPositionToLookAt);
                                if (selfLogic.CurrentToolIsHolstered)
                                {
                                    selfLogic.ToggleHolsterTool();
                                    IEnumerator BeginChoppingDelay()
                                    {
                                        yield return new WaitForSeconds(2.5f);
                                        BeginChopping("vertical");
                                        StopCoroutine(BeginChoppingDelay());
                                    }
                                    StartCoroutine(BeginChoppingDelay());
                                }
                                else
                                {
                                    BeginChopping("vertical");
                                }
                                StopCoroutine(Stage3BeginChopping());
                            }
                            StartCoroutine(Stage3BeginChopping());
                        }
                    }
                    else if (hasReachedTaskObject && !TaskObject) // When the part of the tree has ben cut off.
                    {
                        hasReachedTaskObject = false;
                        isChopping = false;
                        selfLogic.CurrentTool.GetComponent<Axe>().shouldChop = false;
                        selfAnim.PlayAnimation("hacking_vertical_end", 0.5f);
                        IEnumerator SwitchStageTo4()
                        {
                            yield return new WaitForSeconds(1.5f);
                            LumberTask("choptree_stage5");
                            StopCoroutine(SwitchStageTo4());
                        }
                        StartCoroutine(SwitchStageTo4());
                    }
                }
                else if (selfLogic.Task == "choptree_stage5") // Find and begin chopping lower section of the tree, in stage 5
                {
                    if (!hasReachedTaskObject && TaskObject)
                    {
                        Vector3 stage5GameObjectPositionToLookAt = TaskObject.transform.position + TaskObject.transform.forward * 1.5f;
                        float distance = Vector3.Distance(self.transform.position, stage5GameObjectPositionToLookAt + TaskObject.GetComponent<PinetreeStage5>().TreeRightSide * 1f);
                        if (distance <= 1f) // If we reach the tree in stage5
                        {
                            hasReachedTaskObject = true;
                            IEnumerator Stage3BeginChopping() // Begin vertically chopping the tree in stage 5
                            {
                                yield return new WaitForSeconds(1f);
                                self.transform.LookAt(stage5GameObjectPositionToLookAt);
                                if (selfLogic.CurrentToolIsHolstered)
                                {
                                    selfLogic.ToggleHolsterTool();
                                    IEnumerator BeginChoppingDelay()
                                    {
                                        yield return new WaitForSeconds(2.5f);
                                        BeginChopping("vertical");
                                        StopCoroutine(BeginChoppingDelay());
                                    }
                                    StartCoroutine(BeginChoppingDelay());
                                }
                                else
                                {
                                    BeginChopping("vertical");
                                }
                                StopCoroutine(Stage3BeginChopping());
                            }
                            StartCoroutine(Stage3BeginChopping());
                        }
                    }
                    else if (hasReachedTaskObject && !TaskObject) // When the part of the tree has ben cut off.
                    {
                        hasReachedTaskObject = false;
                        isChopping = false;
                        selfLogic.CurrentTool.GetComponent<Axe>().shouldChop = false;
                        selfAnim.PlayAnimation("hacking_vertical_end", 0.5f);
                        IEnumerator SwitchToCollectingWood()
                        {
                            yield return new WaitForSeconds(1.5f);
                            LumberTask("collectwood");
                            StopCoroutine(SwitchToCollectingWood());
                        }
                        StartCoroutine(SwitchToCollectingWood());
                    }
                }
                else if(selfLogic.Task == "collectwood")
                {
                    if (!hasReachedTaskObject && TaskObject)
                    {
                        Vector3 treepartGameObjectPositionToLookAt = TaskObject.transform.position + TaskObject.transform.forward * 1f;
                        float distance = Vector3.Distance(self.transform.position, treepartGameObjectPositionToLookAt);
                        if (distance <= 1f) // If we reach the treepart
                        {
                            hasReachedTaskObject = true;
                           
                            IEnumerator PickupLog() 
                            {
                                yield return new WaitForSeconds(1f);
                                self.transform.LookAt(treepartGameObjectPositionToLookAt);
                                if (!selfLogic.CurrentToolIsHolstered)
                                {
                                    selfLogic.ToggleHolsterTool();
                                    IEnumerator PickupLog2()
                                    {
                                        yield return new WaitForSeconds(2.5f);
                                        CarryLog();
                                        StopCoroutine(PickupLog2());
                                    }
                                    StartCoroutine(PickupLog2());
                                }
                                else
                                {
                                    CarryLog();
                                }
                                StopCoroutine(PickupLog());
                            }
                            StartCoroutine(PickupLog());
                        }
                    }
                }
                break;


            // -- Next Job here --
            case "nextjobhere":
            break;
                



        }
    }


}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class LumberjackTask : MonoBehaviour
{
    

    private GameObject self;

    private NavMeshAgent selfAgent;

    private Logic selfLogic;

    private Nav selfNavigation;

    private Anim selfAnim;

    private Sound selfSound;


    private bool hasReachedTaskObject;
    private bool transportTaskObject = false;
    private bool breakLoops = false;
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



    // Lumberjack methods:------------------------------------------------------------

    public void CalcLumberTask()
    {
       
        if(selfLogic.isGameObjectInWorld("treepart") || selfLogic.isGameObjectInWorld("treepartcut"))
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
            GameObject treeobject = selfLogic.FindClosestObject("tree");
            if (treeobject)
            {
                selfLogic.Task = "choptree";
                selfLogic.TaskObject = treeobject;
                selfLogic.HasDestination = true;
                hasReachedTaskObject = false;
                treeobject.GetComponent<Tree>().OccupiedOwner = self;
                selfNavigation.WalkToDestination(treeobject.transform.position, 0f);
                
            }
        }
        else if(stage == "choptree_stage1") // Find and work with a tree in stage 1
        {
            GameObject treeObject = selfLogic.FindClosestObject("treestage1");
            if (treeObject)
            {
                hasReachedTaskObject = false;
                selfLogic.Task = "choptree_stage1";
                selfNavigation.WalkToDestination(treeObject.transform.position + treeObject.transform.forward * 4 + self.transform.right * 1, 0f);
                selfLogic.TaskObject = treeObject;
            }
        }
        else if(stage == "choptree_stage2") // Find and work with a tree in stage 2
        {
            GameObject treeObject = selfLogic.FindClosestObject("treestage2");
            if(treeObject)
            {
                selfLogic.Task = "choptree_stage2";
                selfLogic.TaskObject = treeObject;
                if (selfLogic.CurrentToolIsHolstered)
                {
                    selfLogic.ToggleHolsterTool();
                    IEnumerator DelayForNav()
                    {
                        yield return new WaitForSeconds(2f);
                        selfNavigation.WalkToDestination(treeObject.transform.position + treeObject.transform.forward * 5.4f + selfLogic.TaskObject.GetComponent<PinetreeStage2>().TreeRightSide * 1f, 0f);
                        StopCoroutine(DelayForNav());
                    }
                    StartCoroutine(DelayForNav());
                }
                else
                {
                    selfNavigation.WalkToDestination(treeObject.transform.position + treeObject.transform.forward * 5.4f + selfLogic.TaskObject.GetComponent<PinetreeStage2>().TreeRightSide * 1f, 0f);
                }
            }
        }
        else if(stage == "choptree_stage3")
        {
            GameObject treeObject = selfLogic.FindClosestObject("treestage3");
            if(treeObject)
            {
                selfLogic.TaskObject = treeObject;
                selfLogic.Task = "choptree_stage3";
                if (selfLogic.CurrentToolIsHolstered)
                {
                    selfLogic.ToggleHolsterTool();
                    IEnumerator DelayForNav()
                    {
                        yield return new WaitForSeconds(2f);
                        selfNavigation.WalkToDestination(treeObject.transform.position + treeObject.transform.forward * 3f + selfLogic.TaskObject.GetComponent<PinetreeStage3>().TreeRightSide * 1f, 0f);
                        StopCoroutine(DelayForNav());
                    }
                    StartCoroutine(DelayForNav());
                }
                else
                {
                    selfNavigation.WalkToDestination(treeObject.transform.position + treeObject.transform.forward * 3f + selfLogic.TaskObject.GetComponent<PinetreeStage3>().TreeRightSide * 1f, 0f);
                }
            }
        }
        else if (stage == "choptree_stage4")
        {
            GameObject treeObject = selfLogic.FindClosestObject("treestage4");
            if (treeObject)
            {
                selfLogic.TaskObject = treeObject;
                selfLogic.Task = "choptree_stage4";
                if (selfLogic.CurrentToolIsHolstered)
                {
                    selfLogic.ToggleHolsterTool();
                    IEnumerator DelayForNav()
                    {
                        yield return new WaitForSeconds(2f);
                        selfNavigation.WalkToDestination(treeObject.transform.position + treeObject.transform.forward * 1.15f + selfLogic.TaskObject.GetComponent<PinetreeStage4>().TreeRightSide * 1f, 0f);
                        StopCoroutine(DelayForNav());
                    }
                    StartCoroutine(DelayForNav());
                }
                else
                {
                    selfNavigation.WalkToDestination(treeObject.transform.position + treeObject.transform.forward * 1.15f + selfLogic.TaskObject.GetComponent<PinetreeStage4>().TreeRightSide * 1f, 0f);
                }
            }
        }
        else if (stage == "choptree_stage5")
        {
            GameObject treeObject = selfLogic.FindClosestObject("treestage5");
            if (treeObject)
            {
                selfLogic.TaskObject = treeObject;
                selfLogic.Task = "choptree_stage5";
                if (selfLogic.CurrentToolIsHolstered)
                {
                    selfLogic.ToggleHolsterTool();
                    IEnumerator DelayForNav()
                    {
                        yield return new WaitForSeconds(2f);
                        selfNavigation.WalkToDestination(treeObject.transform.position + treeObject.transform.forward * 1.4f + selfLogic.TaskObject.GetComponent<PinetreeStage5>().TreeRightSide * 1f, 0f);
                        StopCoroutine(DelayForNav());
                    }
                    StartCoroutine(DelayForNav());
                }
                else
                {
                    selfNavigation.WalkToDestination(treeObject.transform.position + treeObject.transform.forward * 1.4f + selfLogic.TaskObject.GetComponent<PinetreeStage5>().TreeRightSide * 1f, 0f);
                }
            }
        }
        else if(stage == "collectwood")
        {
            hasReachedTaskObject = false; 
            GameObject workstation = FindClosestLumberWorkstationWithLog();
            if (workstation != null)
            {
                selfLogic.TaskObject = workstation;
                selfLogic.UsingWorkstation = workstation;
                selfLogic.Task = "gotolumberworkstation";
                selfNavigation.WalkToDestination(selfLogic.TaskObject.transform.Find("SawingPos").position, 0f);
            }
            else
            {
                GameObject treeObject = selfLogic.FindClosestObject("treepart");
                if (treeObject)
                {
                    selfLogic.TaskObject = treeObject;
                    selfLogic.Task = "collectwood";
                    if (!selfLogic.CurrentToolIsHolstered)
                    {
                        selfLogic.ToggleHolsterTool();
                        IEnumerator DelayForNav()
                        {
                            yield return new WaitForSeconds(2f);
                            float distance = Vector3.Distance(self.transform.position, selfLogic.TaskObject.transform.position);
                            if (distance > 2.5)
                            {
                                selfNavigation.WalkToDestination(treeObject.transform.position + treeObject.transform.forward * 1f, 4f);
                            }
                            StopCoroutine(DelayForNav());
                        }
                        StartCoroutine(DelayForNav());
                    }
                    else
                    {
                        selfNavigation.WalkToDestination(treeObject.transform.position + treeObject.transform.forward * 1f, 1.5f);
                    }
                }
            }
        }
       
        else if(stage == "sawtreepart")
        {
            hasReachedTaskObject = false;
            Lumberworkstation uwsScript = selfLogic.UsingWorkstation.GetComponent<Lumberworkstation>();
            if (!uwsScript.RightValveIsAdjusted) // Making sure that the right valve is closed
            {
                selfLogic.TaskObject = selfLogic.UsingWorkstation.transform.Find("AdjustStation").gameObject;
                selfNavigation.WalkToDestination(selfLogic.TaskObject.transform.position, 0f);
            }
            else if (!uwsScript.LeftValveIsAdjusted) // Making sure that the left valve is closed
            {
                selfLogic.TaskObject = selfLogic.UsingWorkstation.transform.Find("AdjustStation2").gameObject;
                selfNavigation.WalkToDestination(selfLogic.TaskObject.transform.position, 0f);
            }
            else if (!uwsScript.SawIsTaken) // If both valves are closed - Then we can begin to get the saw
            {
                selfLogic.TaskObject = selfLogic.UsingWorkstation.transform.Find("GetSawPos").gameObject;
                selfNavigation.WalkToDestination(selfLogic.TaskObject.transform.position, 0f);
            }
            else // If all the above conditions are met, we begin to saw the treepart
            {
                selfLogic.TaskObject = selfLogic.UsingWorkstation.transform.Find("SawingPos").gameObject;
                selfNavigation.WalkToDestination(selfLogic.TaskObject.transform.position, 0f);
            }
            selfLogic.Task = "sawtreepart";
        }
        else if(stage == "storetreeparts")
        {
            hasReachedTaskObject = false;
            selfLogic.TaskObject = selfLogic.UsingWorkstation.transform.Find("GetSawPos").gameObject;
            selfNavigation.WalkToDestination(selfLogic.TaskObject.transform.position, 0f);
            selfLogic.Task = "storetreeparts";
        }
        else if(stage == "gotolumberworkstation")
        {
            hasReachedTaskObject = false;
            selfLogic.TaskObject = selfLogic.UsingWorkstation;
            selfNavigation.WalkToDestination(selfLogic.UsingWorkstation.transform.Find("SawingPos").position, 0f); 
            selfLogic.Task = "gotolumberworkstation";
        }
        else if (stage == "gotostorage")
        {
            hasReachedTaskObject = false;
            selfLogic.TaskObject = selfLogic.FindClosestObject("storagetent");
            if(selfLogic.TaskObject != null)
            {
                selfLogic.Task = "gotostorage";
                selfNavigation.WalkToDestination(selfLogic.TaskObject.transform.Find("UsePosition").transform.position, 0f);
            }
        }

    }


    
    // Begin chopping sequence for LumberTask
    private void BeginChopping(string swingside)
    {
        if (selfLogic.CurrentToolIsHolstered)
        {
            selfLogic.ToggleHolsterTool();

            IEnumerator ReRunMethod()
            {
                yield return new WaitForSeconds(1.5f);
                BeginChopping(swingside);
                StopCoroutine(ReRunMethod());
            }
            StartCoroutine(ReRunMethod());
        }
        else
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
    }



    // Carry a treepart
    private Vector3 treepartPos = new Vector3(0.25f, 0.1f, -0.75f);
    private Quaternion treepartQua = new Quaternion(0f, 0f, 0f, 0);
    private void CarryLog()
    {
        if(selfLogic.TaskObject.tag == "treepart")
        {
            selfAnim.PlayAnimation("log_pickup", 0f);
            IEnumerator ParentLog()
            {
                yield return new WaitForSeconds(0.7f);
                Transform tot = selfLogic.TaskObject.transform;
                tot.SetParent(selfLogic.RightHand.transform);
                tot.rotation = selfLogic.RightHand.transform.rotation;
                tot.RotateAround(tot.position, tot.right, treepartQua.x);
                tot.RotateAround(tot.position, tot.up, treepartQua.y);
                tot.RotateAround(tot.position, tot.forward, treepartQua.z);
                tot.position = selfLogic.RightHand.transform.position + tot.right * treepartPos.x + tot.up * treepartPos.y + tot.forward * treepartPos.z;
                selfLogic.CurrentCarrying = selfLogic.TaskObject;
                selfLogic.IsCarryingResource = true;
                StopCoroutine(ParentLog());
            }
            StartCoroutine(ParentLog());

            IEnumerator ContinueWork()
            {
                yield return new WaitForSeconds(3.5f);
                if (selfLogic.CurrentCarrying.name.Contains("pinetree_level3_top"))
                {
                    LumberTask("gotostorage");
                }
                else
                {
                    selfLogic.TaskObject = selfLogic.FindClosestObject("lumberworkstation");
                    if (selfLogic.TaskObject)
                    {
                        selfNavigation.WalkToDestination(selfLogic.TaskObject.transform.Find("SawingPos").position, 0f);
                        hasReachedTaskObject = false;
                    }
                    else
                    {
                        IEnumerator WaitToContinue()
                        {
                            while (true)
                            {
                                yield return new WaitForSeconds(2f);
                                selfLogic.TaskObject = selfLogic.FindClosestObject("lumberworkstation");
                                if (selfLogic.TaskObject)
                                {
                                    selfNavigation.WalkToDestination(selfLogic.TaskObject.transform.Find("SawingPos").position, 0f);
                                    hasReachedTaskObject = false;
                                    break;
                                }

                            }
                            StopCoroutine(WaitToContinue());
                        }
                        StartCoroutine(WaitToContinue());
                    }
                }
                StopCoroutine(ContinueWork());
            }
            StartCoroutine(ContinueWork());
        }
    }


    // Play saw sounds
    private void PlaySawSound()
    {
        string[] sounds = {"handsaw1", "handsaw2", "handsaw3"};
        selfSound.PlaySound(sounds[Random.Range(0, 2)], 0.5f, 1f, 0f);
    }



    private void SplitTreepart() // Split the treepart in two when the sawing process is finished
    {
        Lumberworkstation station = selfLogic.UsingWorkstation.GetComponent<Lumberworkstation>();
        if(station.CurrentHoldingRS != null)
        {
            if(station.CurrentHoldingRS.tag == "treepart")
            {
                GameObject log = station.CurrentHoldingRS;

                GameObject RightCut = Instantiate(log.GetComponent<TreePart>().RightPart);
                RightCut.transform.parent = selfLogic.UsingWorkstation.transform;
                RightCut.transform.rotation = log.transform.rotation;
                RightCut.transform.position = log.transform.position + log.transform.right * -0.01f + log.transform.up * 0f + log.transform.forward * 0f;
                station.CurrentHoldingRS = RightCut;

                GameObject LeftCut = Instantiate(log.GetComponent<TreePart>().LeftPart);
                LeftCut.transform.parent = selfLogic.UsingWorkstation.transform;
                LeftCut.transform.rotation = log.transform.rotation;
                LeftCut.transform.position = log.transform.position + log.transform.right * 0.01f + log.transform.up * 0f + log.transform.forward * 0f;
                station.CurrentHoldingRS2 = LeftCut;

                Destroy(log);
            }
        }
    }



    public GameObject FindClosestLumberWorkstationWithLog() // Find the closest lumber workstation with a log on it
    {
        GameObject[] objects = GameObject.FindGameObjectsWithTag("lumberworkstation");
        if (objects.Length != 0)
        {
            float[] objectsDistances = new float[objects.Length];
            int index = 0;
            foreach (GameObject i in objects)
            {
                objectsDistances[index] = Vector3.Distance(self.transform.position, i.transform.position);
                index++;
            }

            bool objectHasntBeenFound = true;
            int indexToUse = -1;
            int indexFound = 0;
            foreach (float distance in objectsDistances)
            {
                if (distance == Mathf.Min(objectsDistances))
                {
                    Lumberworkstation script = objects[indexFound].GetComponent<Lumberworkstation>();
                    if (!script.IsOccupied && script.CurrentHoldingRS != null)
                    {
                        objectHasntBeenFound = false;
                        objects[indexFound].GetComponent<Lumberworkstation>().IsOccupied = true;
                        indexToUse = indexFound;
                        break;
                    }
                    else
                    {
                        objectsDistances[indexFound] = 99999999;
                        objects[indexFound] = null;
                    }
                }
                indexFound++;
            }

            if (!objectHasntBeenFound)
            {
                return objects[indexToUse];
            }
            else
            {
                return null;
            }

        }
        else
        {
            return null;
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
                    if (!hasReachedTaskObject && selfLogic.TaskObject) // If settler hasn't reached the tree
                    {
                        float distance = Vector3.Distance(self.transform.position, selfLogic.TaskObject.transform.position);

                        if (distance <= 1.5f && !hasReachedTaskObject)
                        {
                            hasReachedTaskObject = true;
                            selfNavigation.Stop();
                            self.transform.LookAt(selfLogic.TaskObject.transform);
                            BeginChopping("horizontal");
                        }
                    }
                    else if (hasReachedTaskObject && !selfLogic.TaskObject) // If the tree has been chopped down, and needs to go to stage1
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
                    if (!hasReachedTaskObject && selfLogic.TaskObject) // If the settler hasn't reached the tree in stage 1
                    {
                        Vector3 stage1GameObjectPositionToLookAt = selfLogic.TaskObject.transform.position + selfLogic.TaskObject.transform.forward * 4;
                        float distance = Vector3.Distance(self.transform.position, stage1GameObjectPositionToLookAt + self.transform.right * 1);
                        if (distance <= 2f) // If we reach the tree in stage1 
                        {
                            hasReachedTaskObject = true;
                            selfNavigation.Stop();
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
                                selfLogic.TaskObject.GetComponent<PinetreeStage1>().plucktree_start();
                                StopCoroutine(Stage1Pluck());
                            }
                            StartCoroutine(Stage1Pluck());
                        }
                    }
                    else if (hasReachedTaskObject && !selfLogic.TaskObject) // When the tree has been plucked
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
                    if (!hasReachedTaskObject && selfLogic.TaskObject)
                    {
                        Vector3 stage2GameObjectPositionToLookAt = selfLogic.TaskObject.transform.position + selfLogic.TaskObject.transform.forward * 5.4f;
                        float distance = Vector3.Distance(self.transform.position, stage2GameObjectPositionToLookAt + selfLogic.TaskObject.GetComponent<PinetreeStage2>().TreeRightSide * 1f);
                        if (distance <= 0.4f) // If we reach the tree in stage2 
                        {
                            hasReachedTaskObject = true;
                            selfNavigation.Stop();
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
                    else if (hasReachedTaskObject && !selfLogic.TaskObject) // When the upper part of the tree has ben cut off.
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
                    if (!hasReachedTaskObject && selfLogic.TaskObject)
                    {
                        Vector3 stage3GameObjectPositionToLookAt = selfLogic.TaskObject.transform.position + selfLogic.TaskObject.transform.forward * 3f;
                        float distance = Vector3.Distance(self.transform.position, stage3GameObjectPositionToLookAt + selfLogic.TaskObject.GetComponent<PinetreeStage3>().TreeRightSide * 1f);
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
                    else if (hasReachedTaskObject && !selfLogic.TaskObject) // When the part of the tree has ben cut off.
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
                    if (!hasReachedTaskObject && selfLogic.TaskObject)
                    {
                        Vector3 stage4GameObjectPositionToLookAt = selfLogic.TaskObject.transform.position + selfLogic.TaskObject.transform.forward * 1.25f;
                        float distance = Vector3.Distance(self.transform.position, stage4GameObjectPositionToLookAt + selfLogic.TaskObject.GetComponent<PinetreeStage4>().TreeRightSide * 1f);
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
                    else if (hasReachedTaskObject && !selfLogic.TaskObject) // When the part of the tree has ben cut off.
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
                    if (!hasReachedTaskObject && selfLogic.TaskObject)
                    {
                        Vector3 stage5GameObjectPositionToLookAt = selfLogic.TaskObject.transform.position + selfLogic.TaskObject.transform.forward * 1.5f;
                        float distance = Vector3.Distance(self.transform.position, stage5GameObjectPositionToLookAt + selfLogic.TaskObject.GetComponent<PinetreeStage5>().TreeRightSide * 1f);
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
                    else if (hasReachedTaskObject && !selfLogic.TaskObject) // When the part of the tree has ben cut off.
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
                else if (selfLogic.Task == "collectwood")
                {
                    if (!hasReachedTaskObject && selfLogic.TaskObject) // When we are making our way to a treepart
                    {
                        if (selfLogic.TaskObject.tag == "treepart")
                        {
                            Vector3 treepartGameObjectPositionToLookAt = selfLogic.TaskObject.transform.position + selfLogic.TaskObject.transform.forward * 1f;
                            float distance = Vector3.Distance(self.transform.position, treepartGameObjectPositionToLookAt);
                            print(distance);
                            if (distance <= 2f) // If we reach the treepart
                            {
                                hasReachedTaskObject = true;
                                selfNavigation.Stop();
                                IEnumerator PickupLog()
                                {
                                    yield return new WaitForSeconds(1f);
                                    selfNavigation.Stop();
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
                        else if (selfLogic.TaskObject.tag == "lumberworkstation") // When we are making our way to the workstation, and reach it.
                        {
                            Vector3 gameObjectPositionToLookAt = selfLogic.TaskObject.transform.Find("SawingPos").position;
                            float distance = Vector3.Distance(self.transform.position, gameObjectPositionToLookAt);
                            print(distance);
                            if (distance <= 0.1f)
                            {
                                hasReachedTaskObject = true;
                                selfLogic.UsingWorkstation = selfLogic.TaskObject;
                                Transform treepartPos = selfLogic.TaskObject.transform.Find("TreepartPos");
                                self.transform.LookAt(treepartPos);
                                selfAnim.PlayAnimation("log_idle", 0f);
                                selfAnim.PlayAnimation("log_drop", 1.25f);
                                IEnumerator DropLogToStation()
                                {
                                    yield return new WaitForSeconds(2f);
                                    self.transform.LookAt(treepartPos);
                                    Transform workstationTreepartPos = selfLogic.TaskObject.transform.Find("TreepartPos");
                                    GameObject treepart = selfLogic.CurrentCarrying;
                                    treepart.transform.SetParent(workstationTreepartPos);
                                    treepart.transform.rotation = workstationTreepartPos.rotation;
                                    treepart.transform.RotateAround(treepart.transform.position, treepart.transform.up, -90);
                                    treepart.transform.position = workstationTreepartPos.position + treepart.transform.right * 0f + treepart.transform.up * 0.05f + treepart.transform.forward * -0.75f;
                                    selfLogic.IsCarryingResource = false;
                                    selfLogic.UsingWorkstation.GetComponent<Lumberworkstation>().CurrentHoldingRS = treepart;
                                    
                                    if(treepart.name.Contains("pinetree_level5_part2")) // Adjust treepart's position/rotations each, if the a specific model does not fit as everyone else.
                                    {
                                        treepart.transform.position = workstationTreepartPos.position + treepart.transform.right * 0f + treepart.transform.up * 0.05f + treepart.transform.forward * -0.52f;
                                    }

                                    selfLogic.CurrentCarrying = null;

                                    IEnumerator BeginAdjustWorkstation()
                                    {
                                        yield return new WaitForSeconds(2f);
                                        LumberTask("sawtreepart");
                                        StopCoroutine(BeginAdjustWorkstation());
                                    }
                                    StartCoroutine(BeginAdjustWorkstation());


                                    StopCoroutine(DropLogToStation());
                                }
                                StartCoroutine(DropLogToStation());
                            }
                        }
                    }
                    
                }
                else if (selfLogic.Task == "sawtreepart") // When we are going to saw the treepart
                {
                    if (!hasReachedTaskObject && selfLogic.TaskObject)
                    {
                        if (selfLogic.TaskObject.name == "GetSawPos") // Get the saw from the table
                        {
                            float distance = Vector3.Distance(selfLogic.TaskObject.transform.position, self.transform.position);
                            if (distance <= 0.1f)
                            {
                                hasReachedTaskObject = true;
                                selfNavigation.Stop();
                                selfAnim.PlayAnimation("item_pickup_ground", 1f);

                                GameObject saw = selfLogic.UsingWorkstation.transform.Find("carpenter_saw").gameObject;
                                GameObject rh = selfLogic.RightHand;
                                Transform sawT = saw.transform;
                                self.transform.LookAt(saw.transform);
                                IEnumerator GrabSaw()
                                {
                                    yield return new WaitForSeconds(2f);
                                    float extratime = 0f;
                                    if (!selfLogic.CurrentToolIsHolstered) // If we don't have our tool holstered
                                    {
                                        extratime = 2f;
                                        selfLogic.ToggleHolsterTool();
                                        IEnumerator GrabSaw2()
                                        {
                                            yield return new WaitForSeconds(2f);
                                            sawT.SetParent(rh.transform);
                                            sawT.rotation = rh.transform.rotation;
                                            sawT.RotateAround(sawT.position, sawT.right, 180f);
                                            sawT.RotateAround(sawT.position, sawT.up, 180f);
                                            sawT.RotateAround(sawT.position, sawT.forward, 10f);
                                            sawT.position = rh.transform.position + sawT.transform.right * -0.05f + sawT.transform.up * -0.35f + sawT.transform.forward * -0.035f;
                                            StopCoroutine(GrabSaw2());
                                        }
                                        StartCoroutine(GrabSaw2());
                                    }
                                    else
                                    {
                                        sawT.SetParent(rh.transform);
                                        sawT.rotation = rh.transform.rotation;
                                        sawT.RotateAround(sawT.position, sawT.right, 180f);
                                        sawT.RotateAround(sawT.position, sawT.up, 180f);
                                        sawT.RotateAround(sawT.position, sawT.forward, 10f);
                                        sawT.position = rh.transform.position + sawT.transform.right * -0.05f + sawT.transform.up * -0.35f + sawT.transform.forward * -0.035f;
                                    }
                                    StopCoroutine(GrabSaw());
                                    selfLogic.UsingWorkstation.GetComponent<Lumberworkstation>().SawIsTaken = true;
                                    IEnumerator GrabSaw3()
                                    {
                                        yield return new WaitForSeconds(2f + extratime);
                                        LumberTask("sawtreepart");
                                        StopCoroutine(GrabSaw3());
                                    }
                                    StartCoroutine(GrabSaw3());
                                }
                                StartCoroutine(GrabSaw());
                            }
                        }
                        else if (selfLogic.TaskObject.name == "AdjustStation") // Adjust the workbench with the right valve
                        {
                            float distance = Vector3.Distance(selfLogic.TaskObject.transform.position, self.transform.position);
                            if (distance <= 0.05f)
                            {
                                selfNavigation.Stop();
                                hasReachedTaskObject = true;
                                self.transform.LookAt(selfLogic.UsingWorkstation.transform);
                                selfAnim.PlayAnimation("workstation_lumber_use_valve1", 1f);
                                selfLogic.UsingWorkstation.GetComponent<Anim>().PlayAnimation("RValve_close", 2f);
                                IEnumerator AdjustStation()
                                {
                                    yield return new WaitForSeconds(6f);
                                    selfLogic.UsingWorkstation.GetComponent<Lumberworkstation>().RightValveIsAdjusted = true;
                                    LumberTask("sawtreepart");
                                    StopCoroutine(AdjustStation());
                                }
                                StartCoroutine(AdjustStation());
                            }
                        }
                        else if (selfLogic.TaskObject.name == "AdjustStation2") // Adjust the workbench on the left valve
                        {
                            float distance = Vector3.Distance(selfLogic.TaskObject.transform.position, self.transform.position);
                            if (distance <= 0.05f)
                            {
                                selfNavigation.Stop();
                                hasReachedTaskObject = true;
                                self.transform.LookAt(selfLogic.UsingWorkstation.transform.Find("AdjustStation2Look"));
                                selfAnim.PlayAnimation("workstation_lumber_use_valve2", 1f);
                                selfLogic.UsingWorkstation.GetComponent<Anim>().PlayAnimation("LValve_close", 4f);
                                IEnumerator AdjustStation()
                                {
                                    yield return new WaitForSeconds(8f);
                                    selfLogic.UsingWorkstation.GetComponent<Lumberworkstation>().LeftValveIsAdjusted = true;
                                    LumberTask("sawtreepart");
                                    StopCoroutine(AdjustStation());
                                }
                                StartCoroutine(AdjustStation());
                            }
                        }
                        else if (selfLogic.TaskObject.name == "SawingPos") // Go to the sawing position, and begin sawing 
                        {
                            float distance = Vector3.Distance(selfLogic.TaskObject.transform.position, self.transform.position);
                            if (distance <= 0.05f)
                            {
                                selfNavigation.Stop();
                                hasReachedTaskObject = true;
                                self.transform.LookAt(selfLogic.UsingWorkstation.GetComponent<Lumberworkstation>().CurrentHoldingRS.transform);
                                selfAnim.PlayAnimation("tools_saw_lumberworkstation_start", 0f);
                                GameObject treepart = selfLogic.UsingWorkstation.GetComponent<Lumberworkstation>().CurrentHoldingRS;

                                IEnumerator Sawing1()
                                {
                                    int health = treepart.GetComponent<TreePart>().SawingHealth;
                                    while (true)
                                    {
                                        if (breakLoops)
                                        {
                                            treepart.GetComponent<TreePart>().SawingHealth = health;
                                            break;
                                        }

                                        if (health > 0)
                                        {
                                            yield return new WaitForSeconds(1f);
                                            health -= 1;
                                            PlaySawSound();
                                            print(health);
                                        }
                                        else if (health <= 0)
                                        {
                                            StopCoroutine(Sawing1());
                                            selfAnim.PlayAnimation("tools_saw_lumberworkstation_end", 0f);
                                            SplitTreepart();
                                            IEnumerator SwitchToStorageTreeparts()
                                            {
                                                yield return new WaitForSeconds(1f);
                                                LumberTask("storetreeparts");
                                                StopCoroutine(SwitchToStorageTreeparts());
                                            }
                                            StartCoroutine(SwitchToStorageTreeparts());
                                            break;
                                        }
                                    }
                                }

                                IEnumerator Sawing()
                                {
                                    yield return new WaitForSeconds(2f);
                                    selfAnim.PlayAnimation("tools_saw_lumberworkstation_sawing", 0f);
                                    StartCoroutine(Sawing1());
                                    StopCoroutine(Sawing());
                                }
                                StartCoroutine(Sawing());
                            }
                        }
                    }
                }
                else if (selfLogic.Task == "gotolumberworkstation") // Go to a lumberworkstation, and work with the treepart
                {
                    if (!hasReachedTaskObject && selfLogic.TaskObject)
                    {
                        Transform gameObject = selfLogic.TaskObject.transform.Find("SawingPos");
                        float distance = Vector3.Distance(self.transform.position, gameObject.position);
                        if (distance <= 0.1f)
                        {
                            hasReachedTaskObject = true;
                            selfNavigation.Stop();
                            self.transform.LookAt(gameObject.position + gameObject.right * 1.1f);
                            int extratime = 0;
                            if (!selfLogic.CurrentToolIsHolstered)
                            {
                                selfLogic.ToggleHolsterTool();
                                extratime = 1;
                            }
                            GameObject resource = selfLogic.TaskObject.GetComponent<Lumberworkstation>().CurrentHoldingRS;
                            if (resource.tag == "treepart") // Treepart is not sawed completely 
                            {
                                int logHealth = resource.GetComponent<TreePart>().SawingHealth;
                                IEnumerator Delay()
                                {
                                    yield return new WaitForSeconds(1f + extratime);
                                    if (logHealth >= 1)
                                    {
                                        LumberTask("sawtreepart");
                                    }
                                    else
                                    {
                                        
                                    }
                                    StopCoroutine(Delay());
                                }
                                StartCoroutine(Delay());
                            }
                            else if(resource.tag == "treepartcut") // Treepart has been sawed
                            {
                                selfAnim.PlayAnimation("log_pickup", 1f);
                                IEnumerator Delay()
                                {
                                    yield return new WaitForSeconds(2f + extratime);
                                    GameObject log = selfLogic.UsingWorkstation.GetComponent<Lumberworkstation>().GetResource();
                                    if(log != null)
                                    {
                                        Transform logT = log.transform;
                                        GameObject selfRH = selfLogic.RightHand;
                                        logT.SetParent(selfRH.transform);
                                        logT.rotation = selfRH.transform.rotation;
                                        logT.RotateAround(logT.position, logT.forward, 120f);
                                        logT.position = selfRH.transform.position + logT.right * -0.16f + logT.up * -0.17f + logT.forward * -0.75f;
                                        selfLogic.IsCarryingResource = true;
                                        selfLogic.CurrentCarrying = log;
                                        Lumberworkstation workstation = selfLogic.UsingWorkstation.GetComponent<Lumberworkstation>();
                                        if(!workstation.IsLastResource())
                                        {
                                            workstation.RemoveResource(2);
                                        }
                                        else if(workstation.IsLastResource())
                                        {
                                            workstation.RemoveResource(1);
                                            workstation.IsOccupied = false;
                                            workstation.RightValveIsAdjusted = false;
                                            workstation.LeftValveIsAdjusted = false;
                                            Anim workstationAnim = selfLogic.UsingWorkstation.GetComponent<Anim>();
                                            workstationAnim.PlayAnimation("RValve_open", 0f);
                                            workstationAnim.PlayAnimation("LValve_open", 0f);
                                            selfLogic.UsingWorkstation = null;
                                        }
                                        
                                        IEnumerator Delay2()
                                        {
                                            yield return new WaitForSeconds(2f);
                                            LumberTask("gotostorage");
                                            StopCoroutine(Delay2());
                                        }
                                        StartCoroutine(Delay2());
                                    }
                                    StopCoroutine(Delay());
                                }
                                StartCoroutine(Delay());
                            }
                        }
                    }
                }
                else if (selfLogic.Task == "storetreeparts") // When the wood has been sawed, and we must begin to store it
                {
                    if (!hasReachedTaskObject && selfLogic.TaskObject)
                    {

                        float distance = Vector3.Distance(selfLogic.TaskObject.transform.position, self.transform.position);
                        if (distance <= 0.1f)
                        {
                            hasReachedTaskObject = true;
                            Transform HolderPos = selfLogic.UsingWorkstation.transform.Find("SawHolderPos");
                            self.transform.LookAt(HolderPos);
                            selfNavigation.Stop();
                            selfAnim.PlayAnimation("aaa", 0f);
                            selfAnim.PlayAnimation("item_pickup_ground", 1f);
                            IEnumerator PlaceSaw() // We place the handsaw we just used to saw the wood
                            {
                                yield return new WaitForSeconds(2f);
                                GameObject saw = selfLogic.RightHand.transform.Find("carpenter_saw").gameObject;
                                saw.transform.SetParent(selfLogic.UsingWorkstation.transform);
                                saw.transform.position = HolderPos.transform.position;
                                saw.transform.rotation = HolderPos.transform.rotation;
                                selfLogic.UsingWorkstation.GetComponent<Lumberworkstation>().SawIsTaken = false;
                                IEnumerator PlaceSaw1()
                                {
                                    yield return new WaitForSeconds(2);
                                    LumberTask("gotolumberworkstation");
                                    StopCoroutine(PlaceSaw1());
                                }
                                StartCoroutine(PlaceSaw1());
                                StopCoroutine(PlaceSaw());
                            }
                            StartCoroutine(PlaceSaw());
                        }
                    }
                }
                else if(selfLogic.Task == "gotostorage") // Make our way to a storage building, with the sawed wood
                {
                    if (!hasReachedTaskObject && selfLogic.TaskObject)
                    {
                        float distance = Vector3.Distance(self.transform.position, selfLogic.TaskObject.transform.Find("UsePosition").transform.position);
                        if (distance <= 1f)
                        {
                            hasReachedTaskObject = true;
                            selfNavigation.Stop();
                            selfAnim.PlayAnimation("log_drop", 1f);
                            IEnumerator Delay()
                            {
                                yield return new WaitForSeconds(2f);
                                GameObject log = selfLogic.CurrentCarrying;
                                Destroy(log);
                                selfLogic.IsCarryingResource = false;
                                selfLogic.TaskObject.GetComponent<storagetent>().AddResource("wood");
                                IEnumerator Delay2()
                                {
                                    yield return new WaitForSeconds(1f);
                                    if (selfLogic.UsingWorkstation == null)
                                    {
                                        CalcLumberTask();
                                    }
                                    else
                                    {
                                        LumberTask("gotolumberworkstation");
                                    }
                                    StopCoroutine(Delay2());
                                }
                                StartCoroutine(Delay2());
                                StopCoroutine(Delay());
                            }
                            StartCoroutine(Delay());
                        }
                    }
                }
                break;

                



        }
    }


}

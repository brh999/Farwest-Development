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
                        selfNavigation.WalkToDestination(treeObject.transform.position + treeObject.transform.forward * 5 + self.transform.right * 1.35f, 0f);
                        StopCoroutine(DelayForNav());
                    }
                    StartCoroutine(DelayForNav());
                }
                else
                {
                    selfNavigation.WalkToDestination(treeObject.transform.position + treeObject.transform.forward * 5 + self.transform.right * 1.35f, 0f);
                }
            }
        }
    }


    // Begin chopping sequence for LumberTask
    private void BeginChopping(string swingside)
    {
        if (!isChopping)
        {
            switch (swingside)
            {
                case "horizontal":
                selfAnim.PlayAnimation("hacking_horizontal_start", 1f);
                selfAnim.PlayAnimation("hacking_horizontal", 1.8f);
                isChopping = true;
                break;

                case "vertical":
                selfAnim.PlayAnimation("hacking_vertical_start", 0f);
                selfAnim.PlayAnimation("hacking_vertical", 2.5f);
                isChopping = true;
                break;
            }
            
        }
    }




    // New work here etc.



    // Find an object by tag, and return the closest one to a settler/player
    private GameObject FindClosestObject(string tag)
    {

        GameObject[] objects = GameObject.FindGameObjectsWithTag(tag);
        if (objects != null)
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
            return null;
        }
    }

    // Update method for all jobs/tasks
    private void Update()
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
                else if(selfLogic.Task == "choptree_stage1") // Find and pluck a tree
                {
                    if(!hasReachedTaskObject && TaskObject) // If the settler hasn't reached the tree in stage 1
                    {
                        Vector3 stage1GameObjectPositionToLookAt = TaskObject.transform.position + TaskObject.transform.forward * 4;
                        float distance = Vector3.Distance(self.transform.position, stage1GameObjectPositionToLookAt + self.transform.right * 1);
                        if(distance <= 1f) // If we reach the tree in stage1 
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
                    else if(hasReachedTaskObject && !TaskObject) // When the tree has been plucked
                    {
                        hasReachedTaskObject = false;
                        IEnumerator SwitchStageTo2()
                        {
                            yield return new WaitForSeconds(2.5f);
                            LumberTask("choptree_stage2");
                            StopCoroutine(SwitchStageTo2());
                        }
                        StartCoroutine(SwitchStageTo2());
                    }
                }
                else if(selfLogic.Task == "choptree_stage2") // Find and begin chopping top of the tree
                {
                    if(!hasReachedTaskObject && TaskObject)
                    {
                        Vector3 stage2GameObjectPositionToLookAt = TaskObject.transform.position + TaskObject.transform.forward * 5;
                        float distance = Vector3.Distance(self.transform.position, stage2GameObjectPositionToLookAt + self.transform.right * 1.35f);
                        if (distance <= 1f) // If we reach the tree in stage2 
                        {
                            hasReachedTaskObject = true;
                            IEnumerator Stage2BeginChopping() // Begin vertically chopping the tree in stage 2
                            {
                                yield return new WaitForSeconds(1.5f);
                                self.transform.LookAt(stage2GameObjectPositionToLookAt + TaskObject.transform.forward * 1);
                                if (selfLogic.CurrentToolIsHolstered)
                                {
                                    selfLogic.ToggleHolsterTool();
                                    IEnumerator BeginChoppingDelay()
                                        {
                                        yield return new WaitForSeconds(1.5f);
                                        BeginChopping("vertical");
                                        StopCoroutine("BeginChoppingDelay");
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
                }
                break;


            // -- Next Job here --
            case "nextjobhere":
            break;
                



        }
    }


}

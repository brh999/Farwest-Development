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
                treeobject.GetComponent<Tree>().OccupiedOwner = gameObject;
                selfNavigation.WalkToDestination(treeobject.transform.position, 3f);
                selfAnim.PlayAnimation("walk_m", 0);
                TaskObject = treeobject;
                selfLogic.HasDestination = true;
                hasReachedTaskObject = false;
                selfLogic.Task = "choptree";
            }
        }
        else if(stage == "choptree_stage1") // Find and work with a tree in stage 1
        {
            GameObject treeObject = FindClosestObject("treestage1");
            if (treeObject)
            {
                selfNavigation.WalkToDestination(treeObject.transform.position + treeObject.transform.forward * 4 + self.transform.right * 1, 0f);
                selfLogic.Task = "choptree_stage1";
                TaskObject = treeObject;
            }
        }
        else if(stage == "choptree_stage2") // Find and work with a tree in stage 2
        {
            GameObject treeObject = FindClosestObject("treestage1plucked");
            if(treeObject)
            {
                if (selfLogic.CurrentToolIsHolstered)
                {
                    selfLogic.ToggleHolsterTool();
                    IEnumerator DelayForNav()
                    {
                        yield return new WaitForSeconds(1.5f);
                        selfNavigation.WalkToDestination(treeObject.transform.position + treeObject.transform.forward * 7 + self.transform.right * 1, 0f);
                        StopCoroutine(DelayForNav());
                    }
                    StartCoroutine(DelayForNav());
                }
                else
                {
                    selfNavigation.WalkToDestination(treeObject.transform.position + treeObject.transform.forward * 7 + self.transform.right * 1, 0f);
                }
                selfLogic.Task = "choptree_stage2";
                TaskObject = treeObject;
            }
        }
    }


    // Begin chopping sequence for LumberTask
    private void BeginChopping()
    {
        if (!isChopping)
        {
            self.transform.LookAt(TaskObject.transform);
            selfAnim.PlayAnimation("hacking_horizontal_start", 1f);
            selfAnim.PlayAnimation("hacking_horizontal", 1.8f);
            isChopping = true;
            
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
                
               
                if (selfLogic.Task == "choptree")
                {
                    if (!hasReachedTaskObject && TaskObject) // If settler hasn't reached the tree
                    {
                        float distance = Vector3.Distance(self.transform.position, TaskObject.transform.position);
                    
                        if (distance <= 3 && !hasReachedTaskObject)
                        {
                            hasReachedTaskObject = true;
                            BeginChopping();
                        }
                    }
                    else if (hasReachedTaskObject && !TaskObject) // If the tree has been chopped down, and needs to go to stage1
                    {
                        hasReachedTaskObject = false;
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
                else if(selfLogic.Task == "choptree_stage1")
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
                                selfLogic.ToggleHolsterTool();
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
                        IEnumerator SwitchStageTo2()
                        {
                            yield return new WaitForSeconds(2.5f);
                            LumberTask("choptree_stage2");
                            StopCoroutine(SwitchStageTo2());
                        }
                        StartCoroutine(SwitchStageTo2());
                    }
                }
                break;


            // -- Next Job here --
            case "nextjobhere":
            break;
                



        }
    }


}

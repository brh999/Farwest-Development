using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class LogicTasks : MonoBehaviour
{
    

    private GameObject self;
    public GameObject TaskObject;

    private NavMeshAgent selfAgent;

    private Logic logic;

    private Nav navigation;

    private Anim anim;

    private Sound sound;


    private bool hasReachedTaskObject;
    public bool isChopping = false;



    void Awake()
    {

        self = gameObject;

        selfAgent = GetComponent<NavMeshAgent>();
        logic = GetComponent<Logic>();
        anim = GetComponent<Anim>();
        navigation = GetComponent<Nav>();
        sound = GetComponent<Sound>();
      
    }





    // Lumberjack methods:

    public void LumberTask(string stage)
    {
        if(stage == "choptree")
        {
            GameObject treeobject = FindClosestObject("tree");
            if(treeobject != null)
            {
                treeobject.GetComponent<Tree>().OccupiedOwner = gameObject;
                navigation.WalkToDestination(treeobject.transform.position, 3f);
                anim.PlayAnimation("walk_m", 0);
                TaskObject = treeobject;
                logic.HasDestination = true;
                hasReachedTaskObject = false;
                logic.Task = "choptree";
            }
        }
        else if(stage == "choptree_stage1")
        {
            GameObject treeObject = FindClosestObject("treestage1");
            if (treeObject)
            {
                navigation.WalkToDestination(treeObject.transform.position + treeObject.transform.forward * 4 + self.transform.right * 1, 0f);
                logic.Task = "choptree_stage1";
                TaskObject = treeObject;
            }
        }
    }


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
                        if (tag == "tree")
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
                        else if(tag == "treestage1")
                        {
                            if (!objects[indexFound].GetComponent<PinetreeStage1>().IsOccupied)
                            {
                                objects[indexFound].GetComponent<PinetreeStage1>().IsOccupied = true;
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


    private void BeginChopping()
    {
        if (!isChopping)
        {
            self.transform.LookAt(TaskObject.transform);
            anim.PlayAnimation("hacking_horizontal_start", 1f);
            anim.PlayAnimation("hacking_horizontal", 1.8f);
            isChopping = true;
            
        }
    }




    // New work here etc.



    // Update method for all jobs/tasks
    private void Update()
    {
        switch(logic.Work)
        {


            // -- Lumberjack job --
            case "lumberjack":
                
               
                if (logic.Task == "choptree")
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
                        anim.PlayAnimation("hacking_horizontal_end", 1f);
                        IEnumerator SwitchStageTo1()
                        {
                            while (true)
                            {
                                yield return new WaitForSeconds(5f);
                                LumberTask("choptree_stage1");
                                StopCoroutine(SwitchStageTo1());
                            }
                        }
                        StartCoroutine(SwitchStageTo1());
                    }
                }
                else if(logic.Task == "choptree_stage1")
                {
                    if(!hasReachedTaskObject && TaskObject)
                    {

                    }
                    
                }
                break;


            // -- Next Job here --
            case "nextjobhere":
            break;
                



        }
    }


}

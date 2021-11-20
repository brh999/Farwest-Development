using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class LogicTasks : MonoBehaviour
{

    private GameObject self;
    public GameObject taskObject;

    private NavMeshAgent selfAgent;

    private Logic logic;

    private Nav navigation;

    private Anim anim;

    private string logicWork;
    private string logicTask;

    private bool hasReachedTaskObject;


    void Awake()
    {
        self = gameObject;

        selfAgent = GetComponent<NavMeshAgent>();
        logic = GetComponent<Logic>();
        anim = GetComponent<Anim>();
        navigation = GetComponent<Nav>();

       

        logicWork = logic.Work;
        logicTask = logic.Task;
    }


    // Lumberjack methods:

    public void LumberTask(string stage)
    {
        if(stage == "collect")
        {
            GameObject treeobject = FindTree();
            if(treeobject != null)
            {
                navigation.WalkToDestination(treeobject.transform.position);
                anim.PlayAnimation("walk_m", 0);
                taskObject = treeobject;
                logic.HasDestination = true;
                hasReachedTaskObject = false;
                logic.Task = "collect";
            }
        }
    }

    GameObject FindTree()
    {
      
        GameObject[] pinetrees = GameObject.FindGameObjectsWithTag("pinetree");
        if (pinetrees != null)
        {
            float[] pinetreesDistances = new float[pinetrees.Length];
            int index = 0;
            foreach (GameObject tree in pinetrees)
            {
                pinetreesDistances[index] = Vector3.Distance(self.transform.position, tree.transform.position);
                index++;
            }

            int indexFound = 0;
            foreach (float distance in pinetreesDistances)
            {
                if (distance == Mathf.Min(pinetreesDistances))
                {
                    if (pinetrees[indexFound].GetComponent<PineTree>().IsOccupied)
                    {
                        break;
                    }
                }
                indexFound++;
            }
          
            return pinetrees[indexFound];

        }
        else
        {
            return null;
        }
    }


    // New work here:



    // Update method for all jobs/tasks
    private void Update()
    {
        switch(logicWork)
        {
            // -- Lumberjack job --
            case "lumberjack":
                // Collecting wood
                if (logic.Task == "collect")
                {
                    float distance = Vector3.Distance(self.transform.position, taskObject.transform.position);
                    if (distance <= 6 && distance >= 4.5)
                    {
                        navigation.WalkToDestination(Vector3.Lerp(self.transform.position, taskObject.transform.position, 0.5f));
                        
                    }
                    if(distance <= 4.5 && !hasReachedTaskObject)
                    {
                        self.transform.LookAt(taskObject.transform);
                        anim.PlayAnimation("hacking_horizontal_start", 1f);
                        anim.PlayAnimation("hacking_horizontal", 1.8f);
                        hasReachedTaskObject = true;
                    }
                }
            break;
                



        }
    }


}

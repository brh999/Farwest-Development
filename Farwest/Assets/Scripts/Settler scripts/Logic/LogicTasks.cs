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


    private bool hasReachedTaskObject;
    private bool isChopping;



    void Awake()
    {
        self = gameObject;

        selfAgent = GetComponent<NavMeshAgent>();
        logic = GetComponent<Logic>();
        anim = GetComponent<Anim>();
        navigation = GetComponent<Nav>();
        isChopping = false;
      
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
      
        GameObject[] trees = GameObject.FindGameObjectsWithTag("tree");
        if (trees != null)
        {
            float[] treesDistances = new float[trees.Length];
            int index = 0;
            foreach (GameObject tree in trees)
            {
                treesDistances[index] = Vector3.Distance(self.transform.position, tree.transform.position);
                index++;
            }

            bool treeHasntBeenFound = true;
            int indexToUse = -1;
            while (treeHasntBeenFound)
            {
                int indexFound = 0;
                foreach (float distance in treesDistances)
                {
                    if (distance == Mathf.Min(treesDistances))
                    {
                        if (!trees[indexFound].GetComponent<Tree>().IsOccupied)
                        {
                            trees[indexFound].GetComponent<Tree>().IsOccupied = true;
                            indexToUse = indexFound;
                            treeHasntBeenFound = false;
                            break;
                        }
                        else
                        {
                            trees[indexFound] = null;
                            treesDistances[indexFound] = 99999999;
                            break;
                        }
                    }

                    indexFound++;
                }
            }
            
                return trees[indexToUse];
           
        }
        else
        {
            return null;
        }
    }


    void BeginChopping()
    {
        if (!isChopping)
        {
            self.transform.LookAt(taskObject.transform);
            anim.PlayAnimation("hacking_horizontal_start", 1f);
            anim.PlayAnimation("hacking_horizontal", 1.8f);
            float timerForHack = 0;
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
                // Collecting wood
               
                if (logic.Task == "collect")
                {
                    if (!hasReachedTaskObject) // If settler hasn't reached the tree
                    {
                        float distance = Vector3.Distance(self.transform.position, taskObject.transform.position);
                        if (distance <= 6 && distance >= 4.5)
                        {
                            navigation.WalkToDestination(Vector3.Lerp(self.transform.position, taskObject.transform.position, 0.5f));
                        }
                        if (distance <= 4.5 && !hasReachedTaskObject)
                        {
                            hasReachedTaskObject = true;
                            BeginChopping();
                        }
                    }
                    else // When the settler has reached a tree
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

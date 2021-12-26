using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Nav : MonoBehaviour
{

    private GameObject self;

    private Anim selfAnim;

    private NavMeshAgent selfNav;

    private LogicTasks selfLogicTasks;

    public bool isRunning = false;
    public bool isWalking = false;
    public bool isUsingSlerp = false;

    private Vector3 destination;

    private float distanceToKeep;
    private float slerpToUse;

    void Awake()
    {
        self = gameObject;
        selfNav = GetComponent<NavMeshAgent>();
        selfAnim = self.GetComponent<Anim>();
        selfLogicTasks = self.GetComponent<LogicTasks>();
    }

    // Walk to a destination, and set how far a settler should their distance to their destination:
    public void WalkToDestination(Vector3 dest, float distance)
    {
        selfNav.destination = dest;
        isWalking = true;
        destination = dest;
        distanceToKeep = distance;
        if(selfAnim.currentAnim != "walk_m")
        {
            selfAnim.PlayAnimation("walk_m", 0f);
        }
    }

    public void WalkToDestinationSlerp(Vector3 dest, float distance, float slerpAmount)
    {
        selfNav.destination = Vector3.Slerp(self.gameObject.transform.position, dest, slerpAmount);
        isWalking = true;
        isUsingSlerp = true;
        destination = dest;
        distanceToKeep = distance;
        slerpToUse = slerpAmount;
        if (selfAnim.currentAnim != "walk_m")
        {
            selfAnim.PlayAnimation("walk_m", 0f);
        }
    }


    private void FixedUpdate()
    {
        if (isWalking) 
        {
            float distance = Vector3.Distance(self.transform.position, destination);
            if (distance <= distanceToKeep + 1.5f && distance >= distanceToKeep)
            {
                
                    selfNav.destination = Vector3.Lerp(self.transform.position, destination, 0.5f);
               
            }
            else if(distance <= distanceToKeep)
            {
                isWalking = false;
                if(selfAnim.currentAnim == "walk_m" && !selfLogicTasks.TaskObject)
                {
                    selfAnim.PlayAnimation("aaa", 1f);
                }
            }
        }
    }

}

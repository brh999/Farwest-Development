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

    private Logic selfLogic;

    public bool isRunning = false;
    public bool isWalking = false;
    public bool isUsingSlerp = false;
    private bool distanceLerpAmountSpeed_Modified = false;

    private Vector3 destination;

    private float distanceToKeep;
    private float slerpToUse;
    private float distanceLerpAmount = 1f; 
    private float distanceLerpAmountSpeedValue = 0.5f; // How fast we want to smooth out the movement to our end destination
    private float distanceLerpAmountSpeed;

    void Awake()
    {
        self = gameObject;
        selfNav = GetComponent<NavMeshAgent>();
        selfAnim = self.GetComponent<Anim>();
        selfLogicTasks = self.GetComponent<LogicTasks>();
        selfLogic = self.GetComponent<Logic>();
        distanceLerpAmountSpeed = distanceLerpAmountSpeedValue;
    }

    // Walk to a destination, and set how far a settler should their distance to their destination:
    public void WalkToDestination(Vector3 dest, float distance)
    {
        selfNav.destination = dest;
        isWalking = true;
        destination = dest;
        distanceToKeep = distance;
       
        if(selfLogic.Task == "collectwood" && selfLogicTasks.IsCarryingResource)
        {
            selfAnim.PlayAnimation("log_carry_walk", 0f);
        }
        else if(selfAnim.currentAnim != "walk_m")
        {
            selfAnim.PlayAnimation("walk_m", 0f);
        }

        switch(selfLogic.Task) // If we want to adjust the speed of the lerp in specific situations:
        {
            case "choptree": 
            distanceLerpAmountSpeed = 0.7f;
            distanceLerpAmountSpeed_Modified = true;
            break;

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

    public void Stop()
    {
        selfAnim.PlayAnimation("aaa", 0f);
        isWalking = false;
        isRunning = false;
        selfNav.destination = self.transform.position;
    }

    private void FixedUpdate()
    {
        if (isWalking) 
        {
            float distance = Vector3.Distance(self.transform.position, destination);
            if (distance <= distanceToKeep + 1.5f && distanceLerpAmount > 0f)
            {
                    selfNav.destination = Vector3.Lerp(self.transform.position, destination, distanceLerpAmount);
                    distanceLerpAmount -= distanceLerpAmountSpeed * Time.deltaTime;
            }
            else if(distance <= distanceToKeep)
            {
                isWalking = false;
                distanceLerpAmount = 1f;
                if(selfAnim.currentAnim == "walk_m" && !selfLogicTasks.TaskObject)
                {
                    selfAnim.PlayAnimation("aaa", 1f);
                }
                if(distanceLerpAmountSpeed_Modified)
                {
                    distanceLerpAmountSpeed_Modified = false;
                    distanceLerpAmountSpeed = distanceLerpAmountSpeedValue;
                }
            }
        }
    }

}

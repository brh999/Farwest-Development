using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class IdleTask : MonoBehaviour
{
    private GameObject self;
    public GameObject TaskObject;

    private Logic selfLogic;

    private Nav selfNav;

    private Anim selfAnim;

    private bool hasReachedTaskObject;

    public GameObject test;

    private float[] idlecliplengths = new float[6];

    private int idleReps = 2; // How many times does the AI need to perform "idle animations", before moving back to a new idle activity
    private int idleRepsDone = 0;

    void Awake()
    {
        self = gameObject;
        selfLogic = self.GetComponent<Logic>();
        selfNav = self.GetComponent<Nav>();
        selfAnim = self.GetComponent<Anim>();
    }

     void Start()
    {
        AnimationClip[] clips = self.GetComponent<Animator>().runtimeAnimatorController.animationClips;
        foreach(AnimationClip clip in clips)
        {
            switch(clip.name)
            {
                case "idle1":
                    idlecliplengths[0] = clip.length;
                    break;
                case "idle2":
                    idlecliplengths[1] = clip.length;
                    break;
                case "idle3":
                    idlecliplengths[2] = clip.length;
                    break;
                case "idle4":
                    idlecliplengths[3] = clip.length;
                    break;
                case "idle5":
                    idlecliplengths[4] = clip.length;
                    break;
                case "idle6":
                    idlecliplengths[5] = clip.length;
                    break;
            }
        }
    }

    public void CalcIdleTask()
    {
        float q = Random.Range(0, 1);
        /*if(q >= 0.5)
        {
            // Walks to a random destination and stands doing idle animations
            IdlingTask("walktodestination");
        }
        else if(q < 0.5 && q >= 0.25)
        {
            // Find the nearest chair and sit
        }
        else if(q < 0.25)
        {
            // Find the nearest settler who is also in idle task and socialize 
        } */
        IdlingTask("walktodestination");
    }
    
    
    public void IdlingTask(string stage)
    {
        switch (stage)
        {
            case "walktodestination":
                RaycastHit hitinfo;
                bool haveNotFoundDest = true;
                float maxSearchValue = 10;
                float minSearchValue = -maxSearchValue;
                Transform selfT = self.transform;
                while (haveNotFoundDest)
                {
                    Vector3 origin = selfT.position + selfT.right * Random.Range(maxSearchValue, minSearchValue) + selfT.up * 3 + selfT.forward * Random.Range(maxSearchValue, minSearchValue);
                    if (Physics.Raycast(origin, selfT.TransformDirection(Vector3.down), out hitinfo))
                    {
                        if (hitinfo.transform.gameObject.name == "WorldGround")
                        {
                            haveNotFoundDest = false;
                            Debug.DrawLine(origin, selfT.TransformDirection(Vector3.down), Color.blue, 10f);
                            selfNav.WalkToDestination(origin - selfT.up * 3, 3f);
                            selfLogic.Task = "walktodestination";
                        }
                    }
                }
                break;

            case "idling":
                PlayRandomIdleAnim();
                break;
        }
    }

    private void PlayRandomIdleAnim()
    {
        float time = 0;
        int q = Random.Range(1, 6);
        switch (q)
        { 
            case 1:
                time = idlecliplengths[0];
                selfAnim.PlayAnimation("idle1", 0.5f);
            break;
            case 2:
                time = idlecliplengths[1];
                selfAnim.PlayAnimation("idle2", 0.5f);
                break;
            case 3:
                time = idlecliplengths[2];
                selfAnim.PlayAnimation("idle3", 0.5f);
                break;
            case 4:
                time = idlecliplengths[3];
                selfAnim.PlayAnimation("idle4", 0.5f);
                break;
            case 5:
                time = idlecliplengths[4];
                selfAnim.PlayAnimation("idle5", 0.5f);
                break;
            case 6:
                time = idlecliplengths[5];
                selfAnim.PlayAnimation("idle6", 0.5f);
                break;
        }

        idleRepsDone++;
        IEnumerator RedoStage()
        {
            yield return new WaitForSeconds(time);
            if (idleRepsDone < idleReps)
            {
                IdlingTask("idling");
            }
            else
            {
                CalcIdleTask();
                idleRepsDone = 0;
            }
                StopCoroutine(RedoStage());
        }
        StartCoroutine(RedoStage());
    }

    void FixedUpdate()
    {
        if(selfLogic.Task == "walktodestination" && !selfNav.IsWalking)
        {
            selfLogic.Task = "idling";
            IdlingTask("idling");
        }
    }
}

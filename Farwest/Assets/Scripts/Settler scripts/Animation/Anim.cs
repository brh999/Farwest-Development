using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Anim : MonoBehaviour
{

    private GameObject self;

    private Animator animator;

    private float[] upcomingAnimsTime = new float[10];

    private string[] upcomingAnims = new string[10];

    private bool playingDelayedAnim = false;

    public string currentAnim;

    void Awake()
    {
        self = gameObject;
        animator = GetComponent<Animator>();
        currentAnim = "null";
    }

    

    // Play animations, where delay can be added in seconds.
    public void PlayAnimation(string animation, float seconds)
    {
        
        if (seconds > 0)
        {
            DelayedAnimAdd(animation, seconds);
        }
        else
        {
            animator.Play(animation);
            currentAnim = animation;
        }
    }

    // Add a delayed animation to be played
    private void DelayedAnimAdd(string anim, float sec)
    {
        int animArrayKey = 0;
        int secArrayKey = 0;
        foreach(string i in upcomingAnims)
        {
            if(i == null)
            {
                upcomingAnims[animArrayKey] = anim;
                upcomingAnimsTime[secArrayKey] = sec;
                break;
            }
            else 
            {
                animArrayKey += 1;
                secArrayKey += 1;
            }
        }
        if(!playingDelayedAnim)
        {
            playingDelayedAnim = true;
        }
    }



    void Update()
    {
        // Delayed animation 
        if (playingDelayedAnim)
        {    if (upcomingAnims[0] != null && upcomingAnims[0] != "")
            {
                upcomingAnimsTime[0] -= Time.deltaTime;
                if (upcomingAnimsTime[0] <= 0)
                {
                    // Switch for "correcting" any position/rotations of any object with a specific animation
                    switch (upcomingAnims[0])
                    {
                        case "hacking_horizontal_start":
                            transform.Rotate(0, 15, 0, Space.Self);
                            break;
                    }
                    PlayAnimation(upcomingAnims[0], 0);
                    currentAnim = upcomingAnims[0];
                    playingDelayedAnim = false;
                    upcomingAnims[0] = null;
                    upcomingAnimsTime[0] = 0;

                    if (upcomingAnims[1] != null)
                    {
                        int animIndex = 0;
                        foreach (string i in upcomingAnims)
                        {
                            if(i != null)
                            {
                                upcomingAnims[animIndex - 1] = upcomingAnims[animIndex];
                                upcomingAnimsTime[animIndex - 1] = upcomingAnimsTime[animIndex];

                                upcomingAnims[animIndex] = null;
                                upcomingAnimsTime[animIndex] = 0;
                            }
                            animIndex += 1;
                        }
                    }

                    if(upcomingAnims[0] != null)
                    {
                        playingDelayedAnim = true;
                    }
                }
            }
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class Sound : MonoBehaviour
{
    public AudioClip[] sounds = new AudioClip[25];

    private AudioSource audioSource;



    private void Awake()
    {
        audioSource = gameObject.GetComponent<AudioSource>();
    }


    // Play AudioClip with the specified 4 arguments
    public void PlaySound(string name, float volume, float pitch, float delay)
    {
        if (name != "" && volume <= 1 && volume >= 0 && pitch <= 3 && pitch >= -3)
        {
            AudioClip clip = FindAudioClip(name);
            if (clip != null)
            {
                audioSource.clip = clip;
                audioSource.volume = volume;
                audioSource.pitch = pitch;
                if (delay <= 0)
                {
                    audioSource.Play();
                }
                else
                {
                    audioSource.PlayDelayed(delay);
                }
                
            }
        }
        else
        {
            Debug.Log("Could not play AudioClip for " + gameObject.name + " - With audio name: " + name);
        }
    }


    // Find a AudioClip with specified name
    private AudioClip FindAudioClip(string name)
    {
        foreach(AudioClip clip in sounds)
        {
            if(clip.name == name)
            {
                return clip;
            }
        }
        Debug.Log("Could not find AudioClip with the name: " + name);
        return null;
    }

    public void AddAudioClip(AudioClip clip)
    {
        int index = 0;
        foreach(AudioClip i in sounds)
        {
            if(i == null)
            {
                sounds[index] = clip;
                break;
            }
            index += 1;
        }
    }


    //Tree audio for "cracking" the tree up:
    public void Tree_CutDownAudio()
    {
        string pickedsound = PickRandomWoodBreakSound();
        if(pickedsound != null)
        {
            PlaySound(pickedsound, 0.1f, 1, 0);
        }
    }

    // Pick a random wood break sound for the tree:
    private string PickRandomWoodBreakSound()
    {
        int choosenint = Random.Range(0, 4);
        string[] sounds = { "wood_break1", "wood_break2", "wood_break3", "wood_break4", "wood_break5" };
        return sounds[choosenint];
    }



    private void Update()
    {
        
    }

}

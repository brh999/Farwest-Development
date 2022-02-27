using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lumberworkstation : MonoBehaviour
{
    public bool IsOccupied = false;

    public GameObject CurrentHoldingRS; // The current treepart the workstation is holding
    public Transform SawTransform;

    private Animator selfAnim;

    void Awake()
    {
        selfAnim = GetComponent<Animator>();
        SawTransform = gameObject.transform.Find("carpenter_saw");
    }

    public void PlayStartAnimations()
    {

    }

    public void PlayEndAnimations()
    {

    }
    
    void Update()
    {
        
    }
}

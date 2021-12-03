using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Logic : MonoBehaviour
{
    private GameObject self;

    private LogicTasks logictasks;

    public string Work = "none";
    public string Task = "none";

    public bool HasDestination = false;

    public GameObject CurrentTool;

    private void Awake()
    {
        self = gameObject;
        logictasks = GetComponent<LogicTasks>();
        Work = "lumberjack";
    }

    private void Start()
    {
        logictasks.LumberTask("collect");
    }

    public void LogicThink()
    {

    }


}

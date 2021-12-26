using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NavSurface : MonoBehaviour
{
    

    private void Awake()
    {
        
    }

    public void ReGenerate()
    {
        GameObject surface = GameObject.Find("NavMeshSurface");
        if(surface)
        {
            surface.GetComponent<NavMeshSurface>().BuildNavMesh();
        }
        else
        {
            print("No NavMeshSurface gameobject was found..");
        }
    }
  

}

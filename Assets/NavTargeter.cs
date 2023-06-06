using System.Collections;
using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.AI;

public class NavTargeter : MonoBehaviour
{
    
    // Update is called once per frame

    public NavMeshAgent agent;
    public Camera camera;

    public NavMeshSurface surface;
    
    
    void Update()
    {
        if (Input.GetMouseButton(1))
        {
            Physics.Raycast(camera.ScreenPointToRay(Input.mousePosition), out var hitInfo);
            //if (!agent.isOnNavMesh)
            agent.SetDestination(hitInfo.point);
            //agent.Warp(hitInfo.point);
        }

        if (Input.GetMouseButton(0))
        {
            surface.BuildNavMesh();
        }
        
    }
}

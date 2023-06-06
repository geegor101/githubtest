using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;

public class TileSpawner : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject tile;
    
    void Start()
    {
        int width = 4, height = 5;
        
        List<NavMeshSurface> surfaces = new List<NavMeshSurface>(width * height);

        GameObject o;
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                 o = Instantiate(tile, new Vector3(10 * i, 0, 10 * j), new Quaternion());
                 o.name = "Tile " + i + " " + j;
                 surfaces.Add(tile.GetComponentInChildren<NavMeshSurface>());
            }
        }
        
        
        surfaces.ForEach(surface =>
        {
            
            //surface.BuildNavMesh();
            //surface.UpdateNavMesh(surface.navMeshData);
        });
        
        //Physics.ra
        
        
    }

    // Update is called once per frame
    void Update()
    {
        /*
        BoxCollider collider;
        List<RaycastHit> casts = Physics.RaycastAll().ToList<RaycastHit>();
        
        casts.Sort((hit, raycastHit) => hit.distance < raycastHit.distance ? 0 : 1);
        //Array.Sort(Physics.RaycastAll(), (hit, raycastHit) => hit.distance < raycastHit.distance ? 0 : 1);
        */
    }
}

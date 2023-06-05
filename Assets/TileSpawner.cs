using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

public class TileSpawner : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject tile;
    
    void Start()
    {
        GameObject o;
        for (int i = 0; i < 8; i++)
        {
            for (int j = 0; j < 9; j++)
            {
                 o = Instantiate(tile, new Vector3(10 * i, 0, 10 * j), new Quaternion());
                 o.name = "Tile " + i + " " + j;
            }
        }
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

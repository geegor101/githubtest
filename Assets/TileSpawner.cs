using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

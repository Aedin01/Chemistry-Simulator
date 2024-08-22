//
//
//
// UNUSED TEST FILE
//
//
//
//


using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;


public class Voxel : MonoBehaviour
{
    System.Random rand = new System.Random();
    void Start()
    {
        bool[,,] voxel = new bool[10,10,10];
        for (int x = 0; x < 10; x++)
        {
            for (int y = 0; y < 10; y++)
            {
                for (int z = 0; z < 10; z++)
                {
                    if(rand.Next(0,2) % 2 == 0)
                    {
                        voxel[x,y,z] = true;
                        GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
                        cube.transform.position = new UnityEngine.Vector3(x,y,z);
                        cube.transform.localScale = new UnityEngine.Vector3(0.1f,0.1f,0.1f);
                    }
                    else
                    {
                        voxel[x,y,z] = false;
                    }
                }
            }
        }
    }

    void Update()
    {
        
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


struct Cube
{
    public Vector3 position;
    public Color colour;
};

class FluidSim : MonoBehaviour
{
    GameObject[] cubeobjects = new GameObject[10000];
    public ComputeShader shader;
    Cube[] cubes = new Cube[10000];
    void Start()
    {
        System.Random rand = new System.Random();
        int y = 0;
        for (int i = 0; i < 10000; i++)
        {
            if(i % 100 == 0)
            {
                y++;
            }
            cubeobjects[i] = GameObject.CreatePrimitive(PrimitiveType.Cube);
            cubes[i] = new Cube() {position = new Vector3(i % 100,y,rand.Next(0,100))};
        }

        ComputeBuffer buffer = new ComputeBuffer(cubes.Length, sizeof(float) * 7);
        buffer.SetData(cubes);
        shader.SetBuffer(0, "cubes", buffer);
        shader.SetFloat("resolution", cubes.Length);
        shader.Dispatch(0, cubes.Length/100, 8,1);

        buffer.GetData(cubes);

        for (int i = 0; i < 10000; i++)
        {
            cubeobjects[i].transform.position = cubes[i].position;
            cubeobjects[i].GetComponent<Renderer>().material.color = cubes[i].colour;
        }
        buffer.Dispose();
    }
}
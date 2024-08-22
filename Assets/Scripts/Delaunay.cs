using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MIConvexHull;
public class ParticleWrapper : IVertex
{
    public Particle Particle { get; private set; }
    public double[] Position { get; private set; }

    public ParticleWrapper(Particle particle)
    {
        Particle = particle;
        Position = new double[] { particle.position.x, particle.position.y, particle.position.z };
    }
}
public class Delaunay : MonoBehaviour
{
    List<Particle> particleList= new List<Particle>();
    void Start()
    {
        for (int i = 0; i < 4; i++)
        {
            particleList.Add(new Particle() { position = 10*Vector3.back, acceleration = Vector3.zero, velocity = Vector3.zero});
            particleList.Add(new Particle() { position = Vector3.up, acceleration = Vector3.zero, velocity = Vector3.zero});
            particleList.Add(new Particle() { position = Vector3.down, acceleration = Vector3.zero, velocity = Vector3.zero});
            particleList.Add(new Particle() { position = Vector3.left, acceleration = Vector3.zero, velocity = Vector3.zero});
        }
        Mesh mesh = Delaunay.GenerateMesh(particleList);
        GetComponent<MeshFilter>().mesh = mesh;
    }
    public static Mesh GenerateMesh(List<Particle> particles)
    {
        try
        {
        var vertices = new List<ParticleWrapper>();
        foreach (var particle in particles)
        {
            vertices.Add(new ParticleWrapper(particle));
        }

        var tetrahedra = ConvexHull.Create(vertices).Result;

        var mesh = new Mesh();
        var meshVertices = new List<Vector3>();
        var meshTriangles = new List<int>();

        foreach (var face in tetrahedra.Faces)
        {
            foreach (var vertex in face.Vertices)
            {
                Vector3 unityVertex = new Vector3((float)vertex.Position[0], (float)vertex.Position[1], (float)vertex.Position[2]);
                if (!meshVertices.Contains(unityVertex))
                {
                    meshVertices.Add(unityVertex);
                }
            }

            var baseIndex = meshVertices.IndexOf(new Vector3((float)face.Vertices[0].Position[0], (float)face.Vertices[0].Position[1], (float)face.Vertices[0].Position[2]));
            for (int i = 1; i < face.Vertices.Length - 1; i++)
            {
                meshTriangles.Add(baseIndex);
                meshTriangles.Add(meshVertices.IndexOf(new Vector3((float)face.Vertices[i].Position[0], (float)face.Vertices[i].Position[1], (float)face.Vertices[i].Position[2])));
                meshTriangles.Add(meshVertices.IndexOf(new Vector3((float)face.Vertices[i + 1].Position[0], (float)face.Vertices[i + 1].Position[1], (float)face.Vertices[i + 1].Position[2])));
            }
        }

        mesh.vertices = meshVertices.ToArray();
        mesh.triangles = meshTriangles.ToArray();
        mesh.RecalculateNormals();

        return mesh;
        }
        catch
        {
            Debug.Log("delaunay error");
            return new Mesh();
        }
    }
}

//ParticleSimulation.cs
//Aiden Furey, 15/7/2024
//CPU logic system for the simulation of the system's particles

using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

//Structure that represents a single particle, data structure parsed to "ParticleRender.compute"
public struct Particle
{
    public Vector3 position;
    public Vector3 velocity;
    public Vector3 acceleration;
    public float mass;
    //An enumeration that acts as a reference to the particle's chemical type
    public ChemicalInformation.ChemicalType type;
}
public class ParticleSimulation : MonoBehaviour
{
    public float temperature;
    public float maxvelocity;
    public Vector2 cylinderCenter;
    public Vector2 previousCylinderCenter = new Vector2 (0,0);
    public GameObject beaker;
    static int amount = 1024 ;
    public ComputeShader shader;
    public ComputeBuffer buffer;
    Particle[] particles = new Particle[amount];
    public List<Particle> particleList =  new List<Particle>();
    GameObject[] cubes = new GameObject[amount];
    System.Random rand = new System.Random();

    public float collisionThreshold = 0.1f;
    public float deltaTime = 0.02f;

    public float clusterThreshold = 10;
    public List<List<Particle>> clusters = new List<List<Particle>>();

    private List<GameObject> clusterObjects;
    public Material particleMaterial;

    private ComputeBuffer colliderBuffer;
    private ColliderData[] colliders;

    public struct ColliderData
    {
    public Vector3 center;
    public Vector3 size; // x component for radius if it's a sphere, otherwise the size of the box
    public Quaternion rotation; // Quaternion for rotation
    public int type; // 0 = Sphere, 1 = Box
    }
    void Start()
    {
        string dbPath = Application.streamingAssetsPath + "/chemicals.db";
        DatabaseManager.Initialize(dbPath);

        Debug.Log("Melting point of Hydrogen:" + DatabaseManager.GetMeltingPoint(ChemicalInformation.ChemicalType.Hydrogen));

        clusterObjects =  new List<GameObject>();
        for (int i = 0; i < amount; i++)
        {
            particles[i] = new Particle()
            {
            mass = 1,
            position = Random.insideUnitSphere * 10f,
            acceleration = Vector3.down * 2,
            velocity = Vector3.zero//new Vector3(rand.Next(0,100)/100,rand.Next(0,100)/100,rand.Next(0,100)/100)
            };
            cubes[i] = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            cubes[i].transform.localScale = Vector3.one *25;

        // Set the cube's position
            cubes[i].transform.position = particles[i].position;
        }
        buffer = new ComputeBuffer(particles.Length, sizeof(float) * 10 + sizeof(int));
        buffer.SetData(particles);
        shader.SetBuffer(0, "particles", buffer);
    }

     void InitializeColliders()
    {
        Collider[] sceneColliders = FindObjectsOfType<Collider>();
        colliders = new ColliderData[sceneColliders.Length];

        for (int i = 0; i < sceneColliders.Length; i++)
        {
            Collider collider = sceneColliders[i];
            ColliderData data = new ColliderData
            {
                center = collider.transform.position,
                rotation = collider.transform.rotation
            };

            if (collider is SphereCollider sphereCollider)
            {
                data.size = new Vector3(sphereCollider.radius, 0, 0);
                data.type = 0;
            }
            else if (collider is BoxCollider boxCollider)
            {
                data.size = boxCollider.size;
                data.type = 1;
            }

            colliders[i] = data;
        }

        colliderBuffer = new ComputeBuffer(colliders.Length, sizeof(float) * 3 + sizeof(float) * 3 + sizeof(float) * 4 + sizeof(int));
        colliderBuffer.SetData(colliders);
        shader.SetBuffer(0, "colliders", colliderBuffer);
    }

    // Update is called once per frame
    void Update()
    {
        cylinderCenter.x = beaker.transform.position.x;
        cylinderCenter.y = beaker.transform.position.z;
        shader.SetFloat("temperature", temperature);
        shader.SetFloat("maxvelocity", maxvelocity);
        shader.SetFloats("cylinderCenter", cylinderCenter.x, cylinderCenter.y);
        shader.SetFloats("previousCylinderCenter", previousCylinderCenter.x, previousCylinderCenter.y);
        shader.Dispatch(0, amount/16, 1,1);
        buffer.GetData(particles);
        particleList = particles.ToList<Particle>();
        //clusters = ClusterParticles(particleList);
        //Mesh mesh = Delaunay.GenerateMesh(particleList);
        //MeshFilter meshFilter = GetComponent<MeshFilter>();
        //meshFilter.mesh = mesh;
        //GenerateClusterMeshes(clusters);

        previousCylinderCenter = cylinderCenter;
        DrawCubes();
    }

    void GenerateClusterMeshes(List<List<Particle>> clusters)
    {
        // Clean up old cluster objects
        foreach (var obj in clusterObjects)
        {
            Destroy(obj);
        }
        clusterObjects.Clear();

        // Generate new cluster objects
        foreach (var cluster in clusters)
        {
            
            GameObject clusterObject = new GameObject("Cluster");
            Mesh mesh = Delaunay.GenerateMesh(cluster);
            MeshFilter meshFilter = clusterObject.AddComponent<MeshFilter>();
            MeshRenderer meshRenderer = clusterObject.AddComponent<MeshRenderer>();
            meshFilter.mesh = mesh;
            meshRenderer.material = particleMaterial;
            clusterObjects.Add(clusterObject);
        }
    }

    public List<List<Particle>> ClusterParticles(List<Particle> particles)
    {
        clusters.Clear();
        int[] parent = new int[particles.Count];

        // Initialize the parent array
        for (int i = 0; i < particles.Count; i++)
        {
            parent[i] = i;
        }

        // Union-Find functions
        int Find(int x)
        {
            if (parent[x] != x)
            {
                parent[x] = Find(parent[x]);
            }
            return parent[x];
        }

        void Union(int x, int y)
        {
            int rootX = Find(x);
            int rootY = Find(y);
            if (rootX != rootY)
            {
                parent[rootY] = rootX;
            }
        }

        // Cluster particles using union-find
        for (int i = 0; i < particles.Count; i++)
        {
            for (int j = i + 1; j < particles.Count; j++)
            {
                if (Vector3.Distance(particles[i].position, particles[j].position) < clusterThreshold)
                {
                    Union(i, j);
                }
            }
        }

        // Group particles by their root parent
        Dictionary<int, List<Particle>> clusterMap = new Dictionary<int, List<Particle>>();
        for (int i = 0; i < particles.Count; i++)
        {
            int root = Find(i);
            if (!clusterMap.ContainsKey(root))
            {
                clusterMap[root] = new List<Particle>();
            }
            clusterMap[root].Add(particles[i]);
        }

        // Convert the cluster map to a list of clusters
        foreach (var kvp in clusterMap)
        {
            clusters.Add(kvp.Value);
        }

        return clusters;
    }

    void DrawCubes()
    {
        for (int i = 0; i < amount; i++)
        {
            cubes[i].transform.position = particles[i].position;
        }
    }
    Mesh Subdivide(Mesh mesh, int level)
    {
        for (int i = 0; i < level; i++)
        {
            mesh = SubdivideOnce(mesh);
        }
        return mesh;
    }

    Mesh SubdivideOnce(Mesh mesh)
    {
        Dictionary<Vector3, int> midPointCache = new Dictionary<Vector3, int>();
        List<Vector3> vertices = new List<Vector3>(mesh.vertices);
        List<int> triangles = new List<int>();

        for (int i = 0; i < mesh.triangles.Length; i += 3)
        {
            int v0 = mesh.triangles[i];
            int v1 = mesh.triangles[i + 1];
            int v2 = mesh.triangles[i + 2];

            int a = GetMidPointIndex(midPointCache, vertices, v0, v1);
            int b = GetMidPointIndex(midPointCache, vertices, v1, v2);
            int c = GetMidPointIndex(midPointCache, vertices, v2, v0);

            triangles.Add(v0);
            triangles.Add(a);
            triangles.Add(c);

            triangles.Add(v1);
            triangles.Add(b);
            triangles.Add(a);

            triangles.Add(v2);
            triangles.Add(c);
            triangles.Add(b);

            triangles.Add(a);
            triangles.Add(b);
            triangles.Add(c);
        }

        Mesh subdividedMesh = new Mesh();
        subdividedMesh.vertices = vertices.ToArray();
        subdividedMesh.triangles = triangles.ToArray();
        subdividedMesh.RecalculateNormals();

        return subdividedMesh;
    }

    int GetMidPointIndex(Dictionary<Vector3, int> midPointCache, List<Vector3> vertices, int v0, int v1)
    {
        Vector3 midPoint = (vertices[v0] + vertices[v1]) * 0.5f;
        if (midPointCache.TryGetValue(midPoint, out int index))
        {
            return index;
        }

        index = vertices.Count;
        vertices.Add(midPoint);
        midPointCache[midPoint] = index;

        return index;
    }
}
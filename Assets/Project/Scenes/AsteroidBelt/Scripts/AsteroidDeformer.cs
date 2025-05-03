using UnityEngine;

public class AsteroidDeformer : MonoBehaviour
{
    public float deformationAmount = 0.5f;
    public float noiseScale = 0.1f;
    private int seed;

    void Start()
    {
        seed = (int)(Time.time * 1000);
        DeformAsteroid();
        RecenterMesh();
    }

    void DeformAsteroid()
    {
        Mesh mesh = GetComponent<MeshFilter>().mesh;
        Vector3[] vertices = mesh.vertices;

        for (int i = 0; i < vertices.Length; i++)
        {
            Vector3 vertex = vertices[i];
            float noiseX = Mathf.PerlinNoise((vertex.x + seed) * noiseScale, (vertex.y + seed) * noiseScale);
            float noiseY = Mathf.PerlinNoise((vertex.y + seed) * noiseScale, (vertex.z + seed) * noiseScale);
            float noiseZ = Mathf.PerlinNoise((vertex.z + seed) * noiseScale, (vertex.x + seed) * noiseScale);
            vertex += new Vector3(noiseX, noiseY, noiseZ) * deformationAmount;
            vertices[i] = vertex;
        }

        mesh.vertices = vertices;
        mesh.RecalculateNormals();
        mesh.RecalculateBounds();
    }

    // This critical step re-centers the mesh's pivot point!
    void RecenterMesh()
    {
        Mesh mesh = GetComponent<MeshFilter>().mesh;
        Vector3 offset = mesh.bounds.center;

        Vector3[] vertices = mesh.vertices;
        for (int i = 0; i < vertices.Length; i++)
        {
            vertices[i] -= offset;
        }
        mesh.vertices = vertices;

        // Important: Move the asteroid object to compensate for vertex offset.
        transform.position += transform.TransformVector(offset);

        mesh.RecalculateBounds();

        // Update collider to match the re-centered mesh
        MeshCollider collider = GetComponent<MeshCollider>();
        if (collider != null)
        {
            collider.sharedMesh = null;
            collider.sharedMesh = mesh;
        }
    }
}

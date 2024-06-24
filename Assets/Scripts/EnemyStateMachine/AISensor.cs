using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class AISensor : MonoBehaviour
{
    // input for the Mesh creator
    public float distance = 10;
    public float angle = 30f;
    public float height = 1.0f;
    public Color meshColour = Color.red;

    Mesh mesh;

    // scanning behaviour

    public int scanFrequency = 30; // how many times to scan
    public LayerMask layermasks; // what layers to scan 

    // store first 50 collisions
    Collider[] colliders = new Collider[50];
    int count;
    float scanInterval;
    float scanTimer;

    private void Start()
    {
        scanInterval = 1.0f / scanFrequency;
    }

    private void Update()
    {
        scanTimer -= Time.deltaTime;
        if (scanTimer < 0)
        {
            scanTimer += scanInterval;
            Scan();
        }
    }

    private void Scan()
    {
        count = Physics.OverlapSphereNonAlloc(transform.position, distance, colliders, layermasks,QueryTriggerInteraction.Collide);
    }

    Mesh CreateWedgeMesh()
    {
        // source:
        // https://www.youtube.com/watch?v=znZXmmyBF-o&t=565s&ab_channel=TheKiwiCoder

        Mesh mesh = new Mesh();

        int numTriangles = 8;
        int numVertices = numTriangles * 3;

        // init arrays for our mesh dimensions
        Vector3[] vertices = new Vector3[numVertices];
        int[] triangles = new int[numVertices];

        // create the 6 edges of the mesh FoV
        Vector3 bottomCenter = Vector3.zero;
        Vector3 bottomLeft = Quaternion.Euler(0, -angle, 0) * Vector3.forward * distance;
        Vector3 bottomRight = Quaternion.Euler(0, angle, 0) * Vector3.forward * distance;

        // top points are just higher up on Y axis
        Vector3 topCenter = bottomCenter + Vector3.up * height;
        Vector3 topLeft = bottomLeft + Vector3.up * height;
        Vector3 topRight = bottomRight + Vector3.up * height;

        // calculate vertices for sides of mesh

        int vert = 0;

        // left
        vertices[vert++] = bottomCenter;
        vertices[vert++] = bottomLeft;
        vertices[vert++] = topLeft;

        // top left

        vertices[vert++] = topLeft;
        vertices[vert++] = topCenter;
        vertices[vert++] = bottomCenter; // loop back down


        // right
        vertices[vert++] = bottomCenter;
        vertices[vert++] = topCenter;
        vertices[vert++] = topRight;

        // right top 

        vertices[vert++] = topRight;
        vertices[vert++] = bottomRight;
        vertices[vert++] = bottomCenter; // loop back down

        // far side facing player
        vertices[vert++] = bottomLeft;
        vertices[vert++] = bottomRight;
        vertices[vert++] = topRight; // loop back down

        vertices[vert++] = topRight;
        vertices[vert++] = topLeft;
        vertices[vert++] = bottomLeft; // loop back down

        // top triangle
        vertices[vert++] = topCenter;
        vertices[vert++] = topLeft;
        vertices[vert++] = topRight; // loop back down


        // top triangle
        vertices[vert++] = bottomCenter;
        vertices[vert++] = bottomRight;
        vertices[vert++] = bottomLeft; // loop back down

        // assign triangles
        for (int i = 0; i < numVertices; i++)
        {
            triangles[i] = i;
        }

        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.RecalculateNormals();

        return mesh;
    }

    // recreate whenever enemy is adjusted
    private void OnValidate()
    {
        mesh = CreateWedgeMesh();
    }

    private void OnDrawGizmos()
    {
        if (mesh)
        {
            Gizmos.color = meshColour;
            // draw where the enemy is facing
            Gizmos.DrawMesh(mesh, transform.position, transform.rotation);
        }

        Gizmos.DrawWireSphere(transform.position, distance);

        for(int i = 0; i < count; ++i)
        {
            Gizmos.DrawSphere(colliders[i].transform.position,0.2f);
        }
    }
}

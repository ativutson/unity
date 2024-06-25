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
    public LayerMask excludeLayers; // what layers to leave out 

    public List<GameObject> Objects = new List<GameObject>();


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
        count = Physics.OverlapSphereNonAlloc(transform.position,
            distance, colliders, layermasks,QueryTriggerInteraction.Collide);
        Objects.Clear();
        for(int i = 0; i < count; ++i)
        {
            // check if within vision cone
            GameObject obj = colliders[i].gameObject;
            if (IsInSight(obj))
            {
                Objects.Add(obj);
            }

        }
    }

    public bool IsInSight(GameObject obj) {
        // check if items in buffer are actually within the vision cone


        // first check height of object

        Vector3 agentPosition = transform.position;
        Vector3 targetPosition = obj.transform.position;

        Vector3 direction = targetPosition - agentPosition;

        if (direction.y < 0 || direction.y > height)
        {
            return false;
        }



        // check if it is in the slice of vision cone (angle)
        direction.y = 0; // zero this out so we don't have to worry about differences in height
        float angleFromTarget = Vector3.Angle(direction, transform.forward);

        // if more than our designed angles = not in vision cone
        if(angleFromTarget >= angle)
        {
            return false;
        }

        // exclude trees

        // raycast from the middle of the y height of the transform
        agentPosition.y += agentPosition.y / 2;
        targetPosition.y = agentPosition.y;

        // if it hits anything in between the two points, that means the monster can't see the player
        // ideally we would also check if the target was taller than whatever was hit (e.g. if PC is standing behind a tiny shrub)
        // that is a problem for future me
        if (Physics.Linecast(agentPosition,targetPosition, excludeLayers)) 
        {
            return false;
        }

        //Debug.Log(obj + " is in sight!");
        return true;

    }

    Mesh CreateWedgeMesh()
    {
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
            // draw vision cone where the enemy is facing
            Gizmos.color = meshColour;
            meshColour.a = 0.25f;
            Gizmos.DrawMesh(mesh, transform.position, transform.rotation);
        }

        //Gizmos.DrawWireSphere(transform.position, distance);

        Gizmos.color = Color.green;
        foreach (var obj in Objects) 
        {
            // draw sphere around objects that are in vision cone only
            Gizmos.DrawSphere(obj.transform.position, 0.5f);
        }
    }
}

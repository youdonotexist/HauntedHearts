using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/* NOTE: RopeMesh based heavily off of a script from the wiki written 
 * by Ray Nothnagel of Last Bastion Games. TubeRenderer.cs is (C) 2008 Last Bastion Games.
 */

/** \brief Creates a cylinder mesh.
 * 
 * Generates a tube mesh based off of a list of Vector3 or a list of GameObject.
 */
public class RopeMesh
{
    public Material material = null; /**< The material that will be used on the mesh. */
    public int crossSegments = 3; /**< The number of faces around the center of the rope. Hex = 6 */
    public float rebuildTime = 0f; /**< The yield time between rebuild cycles. 0 = every update, 1 = every 1 second */

    private float nextUpdate = 0;
    private CylVertex[] verts;
    private Vector3[] crossPoints;
    private int lastCrossSegments;
    private MeshRenderer mr;
    private Vector3 v0offset;
    private Vector3 v1offset;
    private float theta;
    private Vector3[] meshVertices;
    private Vector2[] uvs;
    private int[] tris;
    private int[] lastVertices;
    private int[] theseVertices;
    private Quaternion meshRotation;
    private Mesh mesh;
    private int vertexIndex;
    private int start;
    private GameObject renderObject = null;
    private GameObject ropeObject = null;

    class CylVertex
    {
        public Vector3 point = Vector3.zero;
        public float radius = 1.0f;

        public CylVertex(Vector3 pt, float r)
        {
            this.point = pt;
            this.radius = r;
        }
    }

    /** 
     * Initializes the ropemesh using the settings provided via this method.
     *
     * @param crossSegments The number of faces around the center of the rope. Hex = 6
     * @param rebuildTime The yield time between rebuild cycles. 0 = every update, 1 = every 1 second
     * @param ropeMaterial The material that will go onto this mesh
     * @param ropeObject The object this renderer will be attached to.
     * @param hideInScene If TRUE the render object will not show up in scene veiw.
     */
    public void Initialize(int crossSegments, float rebuildTime, Material ropeMaterial, GameObject ropeObject, bool hideInScene)
    {
        this.crossSegments = (int)Mathf.Clamp(crossSegments, 3, 125);
        this.rebuildTime = Mathf.Clamp(rebuildTime, 0, 10);
        this.ropeObject = ropeObject;
        this.material = ropeMaterial;

        mesh = new Mesh();
        renderObject = new GameObject("RopeRenderObject");
        renderObject.transform.position = ropeObject.transform.position;
        renderObject.transform.parent = ropeObject.transform;

        if (hideInScene)
            renderObject.hideFlags = HideFlags.HideInInspector | HideFlags.NotEditable | HideFlags.HideInHierarchy;

        renderObject.AddComponent<MeshFilter>();
        mr = renderObject.AddComponent<MeshRenderer>();
        mr.material = material;
    }

    /**
     *  Updates the mesh rope with the settings provided.
     *  
     * @param jointList A list of gameobject's that will represent the mesh points.
     * @param radius Used to set the radius of the mesh.
     */
    public void UpdateMeshRope(List<GameObject> jointList, float radius)
    {
        if (nextUpdate < Time.time)
        {
            SetPoints(jointList, radius);
            nextUpdate += rebuildTime;
        }
        else
        {
            return;
        }

        UpdateMesh();
    }

    /**
     *  Updates the mesh rope with the settings provided.
     *  
     * @param points A list of Vector3's that will represent the mesh points.
     * @param radius Used to set the radius of the mesh.
     */
    public void UpdateMeshRope(List<Vector3> points, float radius)
    {
        if (nextUpdate < Time.time)
        {
            SetPoints(points, radius);
            nextUpdate += rebuildTime;
        }
        else
        {
            return;
        }

        UpdateMesh();
    }

    void UpdateMesh()
    {
        if (verts.Length <= 1)
        {
            renderObject.renderer.enabled = false;
            return;
        }
        renderObject.renderer.enabled = true;

        if (crossSegments != lastCrossSegments)
        {
            crossPoints = new Vector3[crossSegments];
            theta = 2 * Mathf.PI / crossSegments;
            for (int c = 0; c < crossSegments; c++)
                crossPoints[c] = new Vector3(Mathf.Cos(theta * c), Mathf.Sin(theta * c), 0);
            lastCrossSegments = crossSegments;
        }

        meshVertices = new Vector3[verts.Length * crossSegments];
        uvs = new Vector2[verts.Length * crossSegments];
        tris = new int[verts.Length * crossSegments * 6];
        lastVertices = new int[crossSegments];
        theseVertices = new int[crossSegments];

        for (int p = 0; p < verts.Length; p++)
        {
            if (p!=0)
                meshRotation = Quaternion.FromToRotation(Vector3.forward, verts[p - 1].point - verts[p].point);

            for (int c = 0; c < crossSegments; c++)
            {
                vertexIndex = p * crossSegments + c;
                meshVertices[vertexIndex] = ropeObject.transform.InverseTransformPoint(verts[p].point + meshRotation * crossPoints[c] * verts[p].radius);
                uvs[vertexIndex] = new Vector2((float)c / crossSegments, (float)p / verts.Length);
                //uvs[vertexIndex] = new Vector2(c, p);

                lastVertices[c] = theseVertices[c];
                theseVertices[c] = p * crossSegments + c;
            }

            //make triangles
            for (int c = 0; c < crossSegments; c++)
            {
                start = (p * crossSegments + c) * 6;
                tris[start] = lastVertices[c];
                tris[start + 1] = lastVertices[(c + 1) % crossSegments];
                tris[start + 2] = theseVertices[c];
                tris[start + 3] = tris[start + 2];
                tris[start + 4] = tris[start + 1];
                tris[start + 5] = theseVertices[(c + 1) % crossSegments];
            }
        }

        mesh.Clear();
        mesh.vertices = meshVertices;
        mesh.triangles = tris;
        mesh.RecalculateNormals();
        mesh.uv = uvs;
        renderObject.GetComponent<MeshFilter>().mesh = mesh;
    }

    void SetPoints(List<GameObject> joints, float radius)
    {
        if (joints.Count < 2)
            return;

        verts = new CylVertex[joints.Count + 2];

        v0offset = (joints[0].transform.position - joints[1].transform.position) * 0.01f;
        verts[0] = new CylVertex(v0offset + joints[0].transform.position, 0);
        v1offset = (ropeObject.transform.position - joints[joints.Count - 2].transform.position) * 0.01f;
        verts[verts.Length - 1] = new CylVertex(v1offset + joints[joints.Count - 1].transform.position, 0);

        for (int p = 0; p < joints.Count; p++)
            verts[p + 1] = new CylVertex(joints[p].transform.position, radius);
    }

    void SetPoints(List<Vector3> points, float radius)
    {
        if (points.Count < 2)
            return;

        verts = new CylVertex[points.Count + 2];

        v0offset = (points[0] - points[1]) * 0.01f;
        verts[0] = new CylVertex(v0offset + points[0], 0);
        v1offset = (ropeObject.transform.position - points[points.Count - 2]) * 0.01f;
        verts[verts.Length - 1] = new CylVertex(v1offset + points[points.Count - 1], 0);

        for (int p = 0; p < points.Count; p++)
            verts[p + 1] = new CylVertex(points[p], radius);
    }
}

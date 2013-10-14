using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//Disable annoying warning
#pragma warning disable 414

/** \brief Mesh based ropes following splines.
 * 
 * Use this class to create tube mesh based ropes using QuickRopes. The spline will allow you to add more detail in the mesh without adding more joints.
 */
public class QuickRopeSplineMeshRope : QuickRopeBASE 
{
    public Material material = null; /**< The material to use on the generated mesh.*/
    public float meshRadius = 0.5f; /**< The radius (not diameter) of the tube mesh generated.*/
    public int splineDetail = 100; /**< The total amount of control points for the spline between rope pointA and pointB*/
    public int crossSegments = 3; /**< How many sides the generated cylinder has.*/
    public float rebuildTime = 0f; /**< The delay time between builds of the rope mesh.*/

    private RopeMesh ropeMesh = null;
    private RopeSpline spline = null;
    private bool isDynamicRope = false;
    private List<Vector3> splineData = new List<Vector3>();

    void OnDrawGizmos()
    {
        UpdateGizmos();

        if(Application.isPlaying && IsSuccess)
            spline.GizmoDraw(-1);
    }

    void Start()
    {
        if (GetComponent<ControllerBASE>())
            isDynamicRope = GetComponent<ControllerBASE>() ? true : false;

        if (!InitializeRope())
            return;

        ropeMesh = new RopeMesh();
        RopeRadius = meshRadius;

        spline = new RopeSpline();

        spline.ClearSplinePoints();
        spline.AddSplinePointRange(Joints);
        spline.InsertSplinePoint(0, pointB);
        
        ropeMesh.Initialize(crossSegments, rebuildTime, material, gameObject, hideRopeInScene);
    }

    void OnRopeAddJoint(GameObject joint)
    {
        spline.ClearSplinePoints();
        spline.AddSplinePointRange(Joints);
        spline.InsertSplinePoint(0, pointB);
    }

    void OnRopeDeleteJoint()
    {
        spline.ClearSplinePoints();
        spline.AddSplinePointRange(Joints);
        spline.InsertSplinePoint(0, pointB);
    }

    void LateUpdate()
    {
        if (!IsSuccess || splineDetail <= 1)
            return;

        splineData.Clear();
        for (int i = 0; i < splineDetail; i++)
        {
            splineData.Add(spline.Interp((1f / splineDetail) * i));
        }
        splineData.Add(gameObject.transform.position);

        ropeMesh.UpdateMeshRope(splineData, meshRadius);
    }

    static QuickRopeSplineMeshRope rObj;
    /** This static function allows you to create a new rope of this type with a single line of code!
     *
     * @param pointA The first gameObject of your rope.
     * @param pointB The ending gameObject of your rope.
     * @param pointAIsKinimatic Is the first point of your rope Kinematic? (Non-Physical)
     * @param pointBIsKinimatic Is the ending point of your rope Kinematic? (Non-Physical)
     * @param worldConstraints Is this rope bound to any particular plane via a RopeConstraint?
     * 
     * @return QuickRopeSplineMeshRope
     */
    public static QuickRopeSplineMeshRope NewRope(GameObject pointA, GameObject pointB, bool pointAIsKinimatic, bool pointBIsKinimatic, RopeConstraint worldConstraints)
    {
        rObj = pointA.AddComponent<QuickRopeSplineMeshRope>();
        rObj.pointB = pointB;
        rObj.pointAKinematic = pointAIsKinimatic;
        rObj.pointBKinematic = pointBIsKinimatic;
        rObj.worldConstraints = worldConstraints;

        return rObj;
    }
    /** This static function allows you to create a new rope of this type with a single line of code!
     *
     * @param pointA The first gameObject of your rope.
     * @param pointB The ending gameObject of your rope.
     * 
     * @return QuickRopeSplineMeshRope
     */
    public static QuickRopeSplineMeshRope NewRope(GameObject pointA, GameObject pointB)
    {
        return QuickRopeSplineMeshRope.NewRope(pointA, pointB, true, true, RopeConstraint.None);
    }
}
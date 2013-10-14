using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//Disable annoying warning
#pragma warning disable 414

/** \brief Line renderer based ropes following splines.
 * 
 * Use this class to create line renderer based ropes using QuickRopes. The spline will allow you to add more detail in the mesh without adding more joints.
 */
public class QuickRopeSplineLineRope : QuickRopeBASE 
{
    public Material material = null; /**< The material to use on the generated line mesh.*/
    public float ropeDiameter = 0.5f; /**< The diameter (not radius) of the rope line.*/
    public int splineDetail = 100; /**< The total amount of control points for the spline between rope pointA and pointB*/
    public int repeatPerLink = 3; /**< How many times does the material repeat along the spline per control point.*/

    private LineRenderer lRenderer = null;
    private RopeSpline spline = null;
    private bool isDynamicRope = false;
    private List<Vector3> splineData = new List<Vector3>();

    void OnDrawGizmos()
    {
        UpdateGizmos();
    }

    void Start()
    {
        if (GetComponent<ControllerBASE>())
            isDynamicRope = GetComponent<ControllerBASE>() ? true : false;

        if (!InitializeRope())
            return;

        lRenderer = gameObject.AddComponent<LineRenderer>();
        lRenderer.material = material;
        lRenderer.castShadows = false;
        lRenderer.receiveShadows = false;
        lRenderer.SetWidth(ropeDiameter, ropeDiameter);
        lRenderer.useWorldSpace = true;

        spline = new RopeSpline();
        spline.ClearSplinePoints();
        spline.AddSplinePointRange(Joints);
        spline.InsertSplinePoint(0, pointB);

        for (int i = 0; i < splineDetail; i++)
        {
            splineData.Add( spline.Interp((1f / splineDetail) * i) );
        }
        splineData.Add(gameObject.transform.position);

        lRenderer.material.SetTextureScale("_MainTex", new Vector2(Joints.Count * repeatPerLink, 1));
        lRenderer.SetVertexCount(splineData.Count);

        for (int i = 0; i < splineData.Count; i++)
            lRenderer.SetPosition(i, splineData[i]);
    }

    void OnRopeAddJoint(GameObject joint)
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

        lRenderer.material.SetTextureScale("_MainTex", new Vector2(Joints.Count * repeatPerLink, 1));
        lRenderer.SetVertexCount(splineData.Count);

        for (int i = 0; i < splineData.Count; i++)
            lRenderer.SetPosition(i, splineData[i]);
    }

    static QuickRopeSplineLineRope rObj;
    /** This static function allows you to create a new rope of this type with a single line of code!
     *
     * @param pointA The first gameObject of your rope.
     * @param pointB The ending gameObject of your rope.
     * @param pointAIsKinematic Is the first point of your rope Kinematic? (Non-Physical)
     * @param pointBIsKinematic Is the ending point of your rope Kinematic? (Non-Physical)
     * @param worldConstraints Is this rope bound to any particular plane via a RopeConstraint?
     * 
     * @return QuickRopeSplineLineRope
     */
    public static QuickRopeSplineLineRope NewRope(GameObject pointA, GameObject pointB, bool pointAIsKinematic, bool pointBIsKinematic, RopeConstraint worldConstraints)
    {
        rObj = pointA.AddComponent<QuickRopeSplineLineRope>();
        rObj.pointB = pointB;
        rObj.pointAKinematic = pointAIsKinematic;
        rObj.pointBKinematic = pointBIsKinematic;
        rObj.worldConstraints = worldConstraints;

        return rObj;
    }
    /** This static function allows you to create a new rope of this type with a single line of code!
     *
     * @param pointA The first gameObject of your rope.
     * @param pointB The ending gameObject of your rope.
     * 
     * @return QuickRopeSplineLineRope
     */
    public static QuickRopeSplineLineRope NewRope(GameObject pointA, GameObject pointB)
    {
        return QuickRopeSplineLineRope.NewRope(pointA, pointB, true, true, RopeConstraint.None);
    }
}

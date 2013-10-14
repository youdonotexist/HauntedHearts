using UnityEngine;
using System.Collections;

/** \brief Line renderer based ropes.
 * 
 * Use this class to create <a href="http://unity3d.com/support/documentation/ScriptReference/LineRenderer.html">LineRenderer</a> based ropes using QuickRopes.
 */
public class QuickRopeLineRope : QuickRopeBASE 
{
    public Material ropeMaterial; /**< The material to use on the mesh that is created.*/
    public float ropeDiameter = 0.5f; /**< The diameter (not radius) of the rope.*/
    public int repeatPerLink = 3; /**< How many times the material will be repeated per link (10 links with repeat value of 3 = repeated 30x along rope).*/

    private LineRenderer lRenderer;
    private bool success = false;

    void OnDrawGizmos()
    {
        UpdateGizmos();
    }

    void Start()
    {
        RopeRadius = ropeDiameter / 2;
        success = InitializeRope();

        lRenderer = gameObject.AddComponent<LineRenderer>();
        lRenderer.material = ropeMaterial;
        lRenderer.castShadows = false;
        lRenderer.receiveShadows = false;
        lRenderer.SetWidth(ropeDiameter, ropeDiameter);
		lRenderer.useWorldSpace = true;
    }
	
    void LateUpdate()
    {
        if (!success)
            return; 

        lRenderer.material.SetTextureScale("_MainTex", new Vector2(Joints.Count * repeatPerLink, 1));
        lRenderer.SetVertexCount(Joints.Count);
        for (int i = 0; i < Joints.Count; i++)
            lRenderer.SetPosition(i, Joints[i].transform.position);
    }

    static QuickRopeLineRope rObj;
    /** This static function allows you to create a new rope of this type with a single line of code!
     *
     * @param pointA The first gameObject of your rope.
     * @param pointB The ending gameObject of your rope.
     * @param pointAIsKinimatic Is the first point of your rope Kinematic? (Non-Physical)
     * @param pointBIsKinimatic Is the ending point of your rope Kinematic? (Non-Physical)
     * @param worldConstraints Is this rope bound to any particular plane via a RopeConstraint?
     * 
     * @return QuickRopeLineRope
     */
    public static QuickRopeLineRope NewRope(GameObject pointA, GameObject pointB, bool pointAIsKinimatic, bool pointBIsKinimatic, RopeConstraint worldConstraints)
    {
        rObj = pointA.AddComponent<QuickRopeLineRope>();
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
     * @return QuickRopeLineRope
     */
    public static QuickRopeLineRope NewRope(GameObject pointA, GameObject pointB)
    {
        return QuickRopeLineRope.NewRope(pointA, pointB, true, true, RopeConstraint.None);
    }
}

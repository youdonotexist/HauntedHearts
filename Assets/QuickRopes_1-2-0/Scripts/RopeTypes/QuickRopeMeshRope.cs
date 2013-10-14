using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/** \brief Tube mesh based ropes.
 * 
 * Use this class to create tube mesh based ropes using QuickRopes.
 */
public class QuickRopeMeshRope : QuickRopeBASE 
{
    public Material material = null;  /**< The material to use on the mesh that is created.*/
    public float meshRadius = 0.5f;  /**< The radius (not diameter) of the rope.*/
    public int crossSegments = 3; /**< How many sides the generated cylinder has.*/
    public float rebuildTime = 0f; /**< The delay time between builds of the rope mesh.*/

    private RopeMesh ropeMesh = null;
    
    void OnDrawGizmos()
    {
        UpdateGizmos();
    }

    void Start()
    {
        ropeMesh = new RopeMesh();
        RopeRadius = meshRadius;

        if (!InitializeRope())
            return;

        ropeMesh.Initialize(crossSegments, rebuildTime, material, gameObject, hideRopeInScene);
    }

    void LateUpdate()
    {
        if (!IsSuccess)
            return;

        ropeMesh.UpdateMeshRope(Joints, meshRadius);
    }

    static QuickRopeMeshRope rObj;
    /** This static function allows you to create a new rope of this type with a single line of code!
     *
     * @param pointA The first gameObject of your rope.
     * @param pointB The ending gameObject of your rope.
     * @param pointAIsKinimatic Is the first point of your rope Kinematic? (Non-Physical)
     * @param pointBIsKinimatic Is the ending point of your rope Kinematic? (Non-Physical)
     * @param worldConstraints Is this rope bound to any particular plane via a RopeConstraint?
     * 
     * @return QuickRopeMeshRope
     */
    public static QuickRopeMeshRope NewRope(GameObject pointA, GameObject pointB, bool pointAIsKinimatic, bool pointBIsKinimatic, RopeConstraint worldConstraints)
    {
        rObj = pointA.AddComponent<QuickRopeMeshRope>();
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
     * @return QuickRopeMeshRope
     */
    public static QuickRopeMeshRope NewRope(GameObject pointA, GameObject pointB)
    {
        return QuickRopeMeshRope.NewRope(pointA, pointB, true, true, RopeConstraint.None);
    }
}
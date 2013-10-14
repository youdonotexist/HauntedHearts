using UnityEngine;
using System.Collections;

/** \brief prefab (mesh template) based ropes.
 * 
 * Use this class to create mesh based ropes that are represented by prefabs using QuickRopes.
 */
public class QuickRopePrefabRope : QuickRopeBASE 
{
    public GameObject meshTemplate = null; /**< The mesh that will be used to generate the rope mesh.*/
    public Vector3 meshRotation = Vector3.zero; /**< The rotation of the odd mesh templates when placed on the rope.*/
    public Vector3 altMeshRotation = Vector3.zero; /**< The rotation of the even mesh template when placed on the rope.*/
    public float meshScale = 1.0f; /**< The scale of the mesh templates when placed on the rope.*/
    public bool useMeshAsCollider = false; /**< If set to TRUE the templates will also be used at the colliders.*/
    public bool isConvex = false; /**< More info visit the <a href="http://unity3d.com/support/documentation/ScriptReference/MeshCollider-convex.html">MeshCollider.convex</a> documentation.*/
    public bool isSmoothSphereCollisions = false; /**< More info visit the <a href="http://unity3d.com/support/documentation/ScriptReference/MeshCollider-smoothSphereCollisions.html">MeshCollider.smoothSphereCollisions</a> documentation.*/

    private GameObject nObject = null;
    private MeshCollider meshCol = null;

    void OnDrawGizmos()
    {
        UpdateGizmos();
    }

	void Start () 
    {
        if (meshTemplate == null)
		{
			Debug.LogError("["+ gameObject.name +"] - Joint Template must have an object assigned to it for this rope to function.");
			gameObject.active = false;
			return;
		}
		
        if (InitializeRope())
            AddMeshesToRope();

        RopeRadius = meshScale;
	}


    void OnRopeAddJoint(GameObject joint)
    {
        if (meshTemplate == null)
			return;

        nObject = (GameObject)Instantiate(meshTemplate, joint.transform.position + (UpdatedBuildDirection * (BuildDistance / 2)), Quaternion.FromToRotation(Vector3.forward, UpdatedBuildDirection));//Quaternion.Euler(joint.transform.eulerAngles));
        //nObject.transform.forward = (joint.transform.position - Joints[Joints.Count - 2].transform.position).normalized;

        if (Joints.Count % 2 == 0)
            nObject.transform.Rotate(meshRotation);
        else
            nObject.transform.Rotate(altMeshRotation);

 
        if (useMeshAsCollider)
        {
            meshCol = nObject.AddComponent<MeshCollider>();
            meshCol.convex = isConvex;
            meshCol.smoothSphereCollisions = isSmoothSphereCollisions;
        }

        nObject.transform.localScale = Vector3.one * meshScale;
        nObject.transform.parent = joint.transform;
    }

    void AddMeshesToRope()
    {
        for (int i = 1; i < Joints.Count - 1; i++)
        {
            nObject = (GameObject)Instantiate(meshTemplate, Joints[i].transform.position + ((BuildDirection * BuildDistance) / 2), Quaternion.Euler(Joints[i].transform.eulerAngles));
            nObject.transform.localScale = Vector3.one * meshScale;
            nObject.transform.parent = Joints[i].transform;

            if (i % 2 == 0)
                nObject.transform.Rotate(meshRotation);
            else
                nObject.transform.Rotate(altMeshRotation);

            if (useMeshAsCollider)
            {
                meshCol = nObject.AddComponent<MeshCollider>();
                meshCol.convex = isConvex;
                meshCol.smoothSphereCollisions = isSmoothSphereCollisions;
            }
        }
    }

    static QuickRopePrefabRope rObj;
    /** This static function allows you to create a new rope of this type with a single line of code!
     *
     * @param pointA The first gameObject of your rope.
     * @param pointB The ending gameObject of your rope.
     * @param meshTemplate The mesh that will be used to generate the rope mesh.
     * @param pointAIsKinematic Is the first point of your rope Kinematic? (Non-Physical)
     * @param pointBIsKinematic Is the ending point of your rope Kinematic? (Non-Physical)
     * @param worldConstraints Is this rope bound to any particular plane via a RopeConstraint?
     * 
     * @return QuickRopePrefabRope
     */
    public static QuickRopePrefabRope NewRope(GameObject pointA, GameObject pointB, GameObject meshTemplate, bool pointAIsKinematic, bool pointBIsKinematic, RopeConstraint worldConstraints)
    {
        rObj = pointA.AddComponent<QuickRopePrefabRope>();
        rObj.pointB = pointB;
        rObj.meshTemplate = meshTemplate;
        rObj.pointAKinematic = pointAIsKinematic;
        rObj.pointBKinematic = pointBIsKinematic;
        rObj.worldConstraints = worldConstraints;

        return rObj;
    }
    /** This static function allows you to create a new rope of this type with a single line of code!
     *
     * @param pointA The first gameObject of your rope.
     * @param pointB The ending gameObject of your rope.
     * @param meshTemplate The mesh that will be used to generate the rope mesh.
     * 
     * @return QuickRopePrefabRope
     */
    public static QuickRopePrefabRope NewRope(GameObject pointA, GameObject pointB, GameObject meshTemplate)
    {
        return QuickRopePrefabRope.NewRope(pointA, pointB, meshTemplate, true, true, RopeConstraint.None);
    }
}

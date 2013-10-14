using UnityEngine;
using System.Collections;

/** \brief Adds Colliders to Rope.
 *
 * Add colliders to your rope using this class as a component.
 */
public class QuickRopeCollider : MonoBehaviour 
{
    public float colliderRadiusOverride = 0; /**< The radius of the sphere and capsule colliders. */

    private QuickRopeBASE rope = null;
    private CapsuleCollider capCol = null;
    private SphereCollider sphCol = null;

    void OnRopeInitialized()
    {
        rope = GetComponent<QuickRopeBASE>();

        if (colliderRadiusOverride != 0)
            rope.RopeRadius = colliderRadiusOverride;

        AddCapsuleCollider();
        AddSphereCollider();
    }

    void OnRopeAddJoint(GameObject joint)
    {
        capCol = joint.AddComponent<CapsuleCollider>();
        capCol.height = rope.BuildDistance + (rope.RopeRadius * 2);
        capCol.center = new Vector3(0, 0, (rope.BuildDistance / 2));
        capCol.direction = 2;
        capCol.radius = rope.RopeRadius;
        sphCol = joint.AddComponent<SphereCollider>();
        sphCol.radius = rope.RopeRadius;
    }

    void AddCapsuleCollider()
    {
        for (int i = 1; i < rope.Joints.Count - 1; i++)
        {
            capCol = rope.Joints[i].AddComponent<CapsuleCollider>();
            capCol.height = rope.BuildDistance + (rope.RopeRadius * 2);
            capCol.center = new Vector3(0, 0, -(rope.BuildDistance / 2));
            capCol.direction = 2;
            capCol.radius = rope.RopeRadius;
        }
    }

    void AddSphereCollider()
    {
        for (int i = 1; i < rope.Joints.Count - 1; i++)
        {
            sphCol = rope.Joints[i].AddComponent<SphereCollider>();
            sphCol.radius = rope.RopeRadius;
        }
    }
}

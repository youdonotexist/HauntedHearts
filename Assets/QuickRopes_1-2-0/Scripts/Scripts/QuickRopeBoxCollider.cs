using UnityEngine;
using System.Collections;

/** \brief Adds box colliders to the rope.
 * 
 * Use this script as a component on the rope to generate box colliders which can be scaled into 2 directions.
 */
public class QuickRopeBoxCollider : MonoBehaviour 
{
    public Vector3 boxColliderScale1 = Vector3.one; /**< The scale of the first box collider.*/
    public Vector3 boxColliderScale2 = Vector3.one; /**< The scale of the second box collider.*/

    private QuickRopeBASE rope = null;
    private BoxCollider boxCol = null;

    /* Visualization System TODO
    void OnDrawGizmos()
    {
        if (Application.isPlaying)
            return;

        if (!rope)
            rope = GetComponent<QuickRopeBASE>();

        for (int i = 0; i < rope.GizmoTemplate.Count; i++)
        {
            if (i % 2 == 0)
            {
                Gizmos.color = Color.cyan;
                Gizmos.DrawWireCube(rope.GizmoTemplate[i].transform.position + ((rope.BuildDirection * rope.BuildDistance) / 2), boxColliderScale1);
            }
            else
            {
                Gizmos.color = Color.blue;
                Gizmos.DrawWireCube(rope.GizmoTemplate[i].transform.position + ((rope.BuildDirection * rope.BuildDistance)/2), boxColliderScale2);
            }
        }
    }
    */

    void OnRopeInitialized()
    {
        rope = GetComponent<QuickRopeBASE>();

        AddBoxCollider();
    }

    void OnRopeAddJoint(GameObject joint)
    {
        boxCol = joint.AddComponent<BoxCollider>();
        boxCol.center = new Vector3(0, 0, rope.BuildDistance / 2);

        if (rope.Joints.Count % 2 == 0)
            boxCol.size = boxColliderScale1;
        else
            boxCol.size = boxColliderScale2;
    }

    void AddBoxCollider()
    {
        for (int i = 1; i < rope.Joints.Count - 1; i++)
        {
            boxCol = rope.Joints[i].AddComponent<BoxCollider>();
            boxCol.center = new Vector3(0, 0, -(rope.BuildDistance / 2));

            if (i % 2 == 0)
                boxCol.size = boxColliderScale1;
            else
                boxCol.size = boxColliderScale2;
        }
    }
}

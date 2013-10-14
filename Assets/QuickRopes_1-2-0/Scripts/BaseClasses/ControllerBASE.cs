using UnityEngine;
using System.Collections;

/** \brief The base class to all rope controller scripts. 
 *
 *  This is the base class to all of the rope controller scripts. It is currently used by the QuickRopeSmoothController and QuickRopeController classes.
 */
public abstract class ControllerBASE : MonoBehaviour
{
    public int maximumJoints = 50; /**< This is the maximum amount of joints which can be emited. If the Joint Segment variable of the rope script is larger, then this will be ignored until the joint count falls below this number. */
    public int minimumJoints = 2; /**< This is the minimum amount of joints the rope can have when contracting the rope. If the Joint Segment variable of the rope is smaller, then this will be ignored until the joint count grows larger than this number. */
    //public bool dynamicEmission = true; /**< If this is set to true then the rope will be emitted along the Z-axis of the object its attached to. When contracting the rope this is ignored. */

    private QuickRopeBASE rope = null;
    private float controlVelocity = 0f;
    private bool changedState = true;

    [HideInInspector()]
    public bool AtMax = false;
    [HideInInspector()]
    public bool AtMin = false;

    void OnRopeInitialized()
    {
        rope = gameObject.GetComponent<QuickRopeBASE>();

        if (!rope.pointAKinematic)
        {
            Debug.LogWarning("[" + gameObject.name + "] - You cannot expand/contract ropes when pointA is not set to Kinematic.");
            Destroy(this);
        }

        minimumJoints = (int)Mathf.Clamp(minimumJoints, 4, Mathf.Infinity);
    }

    /** Function used to update the controllers state. This is usually placed in the FixedUpdate, LateUpdate, or Update methods inside of a controller class.
     * 
     * @param velocity The velocity of the controlled object. 0 = stopped, -N = contracting, +N = expanding.
     */
    public void UpdateControlState(float velocity)
    {
        if (!rope || !rope.IsSuccess)
            return;

        this.controlVelocity = velocity;

        if (controlVelocity == 0)
        {
            if (changedState)
            {
                changedState = false;
                rope.ControlJoint.transform.parent = null;
                rope.ControlJoint.rigidbody.isKinematic = false;
                try { rope.ControlJoint.GetComponent<CharacterJoint>().connectedBody = rigidbody; }
                catch { rope.ControlJoint.AddComponent<CharacterJoint>().connectedBody = rigidbody; }
            }
            return;
        }

        if (controlVelocity > 0)
        {
            if (AtMin)
                AtMin = false;

            if (!rope.ControlJoint.rigidbody.isKinematic)
            {
                changedState = true;
                rope.ControlJoint.transform.parent = transform;
                rope.ControlJoint.rigidbody.isKinematic = true;
                rope.ControlJoint.GetComponent<CharacterJoint>().connectedBody = null;
            }
            ExpandRope();
            //return;
        }

        if (controlVelocity < 0)
        {
            if (AtMax)
                AtMax = false;

            if (!rope.ControlJoint.rigidbody.isKinematic)
            {
                changedState = true;
                rope.ControlJoint.transform.parent = transform;
                rope.ControlJoint.rigidbody.isKinematic = true;
                rope.ControlJoint.GetComponent<CharacterJoint>().connectedBody = null;
            }
            ContractRope();
            //return;
        }


        if (velocity > 0 && rope.Joints.Count == maximumJoints)
        {
            controlVelocity = 0;
            AtMax = true;
        }

        if (velocity < 0 && rope.Joints.Count == minimumJoints)
        {
            controlVelocity = 0;
            AtMin = true;
        }
    }

    void ContractRope()
    {
        rope.ControlJoint.transform.position = Vector3.MoveTowards(rope.ControlJoint.transform.position, transform.position, -controlVelocity * Time.deltaTime);

        if (rope.Joints.Count == minimumJoints)
            return;

        if (Vector3.Distance(rope.ControlJoint.transform.position, transform.position) <= 0.01f) //(rope.BuildDistance * 0.01f))
        {
            rope.DeleteJoint();
        }
    }
    void ExpandRope()
    {
        //if(dynamicEmission)
        //    rope.ControlJoint.transform.position = Vector3.MoveTowards(rope.ControlJoint.transform.position, transform.position + (rope.UpdatedBuildDirection * rope.BuildDistance), controlVelocity * Time.deltaTime);
        //else

        if (rope.Joints.Count == maximumJoints)
            return;

        rope.ControlJoint.transform.position = Vector3.MoveTowards(rope.ControlJoint.transform.position, transform.position + rope.UpdatedBuildDirection * rope.BuildDistance, controlVelocity * Time.deltaTime);
        //rope.ControlJoint.transform.LookAt(rope.Joints[rope.Joints.Count - 3].transform);

        if (Vector3.Distance(rope.ControlJoint.transform.position, transform.position) >= (rope.BuildDistance - 0.001f))
        {
            rope.AddJoint();
        }
    }
}

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
/** \brief Container class used in QuickRopeAttachObjects. 
 * 
 * Class that is used by the QuickRopeAttachObjects class. It contains important data about the attaching object.  */
public class AttachableObject
{
    public GameObject obj = null; /**< The GameObject that is going to be attached to the rope.  */
    public Vector3 offset = Vector3.zero; /**< The offset of the GameObject. When 'Center On Joint' is true, this will be the offset from the assigned joint.*/
    public bool centerOnJoint = true; /**< When this is true the obj will be centered on the joint at jointIndex. */
    public int jointIndex = 0; /**< The index of the joint which this object will be attached. */
}

/** \brief Easy way to attach objects to rope. 
 * 
 * Drop this script into any object containting a QuickRope for an easy way to attach objects to the rope.  */
public class QuickRopeAttachObjects : MonoBehaviour 
{
    /** A list of the objects that are going to be attached to the QuickRope rope. */
    public List<AttachableObject> attachedObjects = new List<AttachableObject>();

    QuickRopeBASE rope = null;

    void Start()
    {
        rope = GetComponent<QuickRopeBASE>();
    }

    void OnRopeInitialized()
    {
        if (!enabled)
            return;

        foreach (AttachableObject aob in attachedObjects)
        {
            if (aob.jointIndex > rope.Joints.Count)
                return;

            if (aob.centerOnJoint)
                rope.AttachObjectToRope(aob.obj, aob.jointIndex, aob.centerOnJoint, aob.offset);
            else
                rope.AttachObjectToRope(aob.obj, aob.jointIndex, false, aob.offset);
        }
    }
}

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public enum RopeConstraint
{
    X_Y,
    Y_Z,
    Z_X,
    None,
}

[System.Serializable]
/** \brief Rigidbody settings of rope joints. 
 * 
 * Settings container for the rigidbody. This information about <a href="http://unity3d.com/support/documentation/Components/class-Rigidbody.html">Rigidbody</a> is taken from the unity Reference Manual.  */
public class RigidbodySettings
{
    public float mass = 0.5f; /**< The mass of the object (arbitrary units). It is recommended to make masses not more or less than 100 times that of other Rigidbodies. */
    public float drag = 0.2f; /**< 	How much air resistance affects the object when moving from forces. 0 means no air resistance, and infinity makes the object stop moving immediately. */
    public float angularDrag = 0.02f; /**< How much air resistance affects the object when rotating from torque. 0 means no air resistance, and infinity makes the object stop rotating immediately. */
    public bool useGravity = true; /**< If enabled, the object is affected by gravity. */
    public bool isKinematic = false; /**< 	If enabled, the object will not be driven by the physics engine, and can only be manipulated by its Transform. */
    public RigidbodyInterpolation interpolation = RigidbodyInterpolation.None; /**< Try one of the options only if you are seeing jerkiness in your Rigidbody's movement.*/
    public CollisionDetectionMode collisionMode = CollisionDetectionMode.Continuous; /**< Used to prevent fast moving objects from passing through other objects without detecting collisions. */
    //public Vector3 COM_Offset = Vector3.zero; /**< The center of mass offset will allow you to edit the center of mass in the joint rigibodies. */
}

[System.Serializable]
/** \brief Basic rope physics settings. 
 * 
 * Settings which include the limits for the low twist, high twist, swing1 and swing2 features of the CharacterJoint component. This information about <a href="http://unity3d.com/support/documentation/Components/class-CharacterJoint.html">CharacterJoint</a> is taken from the unity Reference Manual.  */
public class BasicJointSettings
{
    public float lowTwistLimit = -40; /**< The lower limit twist angle of the joint. Value must not be lower than -180 or higher than 180. */
    public float highTwistLimit = 40; /**< The higher limit twist angle of the joint. Value must not be lower than -180 or higher than 180. */
    public float swing1Limit = 120; /**< The higher limit swing angle of the joint. Value must not be lower than -180 or higher than 180. */
    public float swing2Limit = 120; /**< The higher limit swing angle of the joint. Value must not be lower than -180 or higher than 180. */
}

[System.Serializable]
/** \brief Advanced rope physics settings 
 * 
 * Settings which include the bounce, spring, and dampening features of the CharacterJoint component. This information about <a href="http://unity3d.com/support/documentation/Components/class-CharacterJoint.html">CharacterJoint</a>  is taken from the unity Reference Manual. */
public class AdvancedJointSettings
{
    //public Vector3 anchorOverride = Vector3.zero; /**< \todo This will be implimented in later versions. Right now it has not effect. */
    public float lowTwistBounce = 0; /**< The bounce of the low twist axis. When the joint hits the limit, it can be made to bounce off it. */
    public float lowTwistSpring = 0; /**< The spring of the low twist axis. If greater than zero, the limit is soft. The spring will pull the joint back. */
    public float lowTwistDamper = 0; /**< The dampening of the low twist axis. If spring is greater than zero, the limit is soft. */

    public float highTwistBounce = 0; /**< The bounce of the high twist axis. When the joint hits the limit, it can be made to bounce off it. */
    public float highTwistSpring = 0; /**< The spring of the high twist axis. If greater than zero, the limit is soft. The spring will pull the joint back.  */
    public float highTwistDamper = 0; /**< The dampening of the swing2 axis. If spring is greater than zero, the limit is soft.  */

    public float swing1Bounce = 0; /**< The bounce of the swing1 axis. When the joint hits the limit, it can be made to bounce off it. */
    public float swing1Spring = 0; /**< The spring of the swing1 axis. If greater than zero, the limit is soft. The spring will pull the joint back.  */
    public float swing1Damper = 0; /**< The dampening of the swing1 axis. If spring is greater than zero, the limit is soft.  */

    public float swing2Bounce = 0; /**< The bounce of the swing2 axis. When the joint hits the limit, it can be made to bounce off it. */
    public float swing2Spring = 0; /**< The spring of the swing2 axis. If greater than zero, the limit is soft. The spring will pull the joint back.  */
    public float swing2Damper = 0; /**< The dampening of the swing2 axis. If spring is greater than zero, the limit is soft.  */

    public float swingBreakForce = Mathf.Infinity;  /**< Sets the force at which the joint will be broken when pulled on. */
    public float twistBreakForce = Mathf.Infinity;  /**< Sets the force at which the joint will be broken when twisted (torque). */
}

[System.Serializable]
/** \brief All public rope physics settings. 
 * 
 * All of the joint settings that are publicly available to you. */
public class RopeJointSettings
{
    //public bool useThisAsOverride = true;
    public RigidbodySettings rigidbodySettings = new RigidbodySettings();  /**< For information see RigidbodySettings */
    public BasicJointSettings basicJointSettings = new BasicJointSettings();  /**< For information see BasicJointSettings */
    public AdvancedJointSettings advancedJointSettings = new AdvancedJointSettings();  /**< For information see AdvancedJointSettings */
}

/** \brief Base class for all Quickropes
 * 
 * The base class to all Quickrope ropes */
public abstract class QuickRopeBASE : MonoBehaviour 
{
    public bool showGizmos = true;  /**< Shows/Hides the gizmos. Sometimes it is annoying to see blue gizmos when you are sampling the ropes. */
    public GameObject pointB = null;  /**< This is the ropes end point. */
    public bool pointAKinematic = true;  /**< This sets whether the ropes starting point will be kinematic or not. If it is kinematic the joint will stick out in the build direction. */
    public bool pointBKinematic = true;  /**< This sets whether the ropes ending point will be kinematic or not. If it is kinematic the joint will stick out in the build direction.  */
    public int jointSegments = 10;  /**< Sets how many joints will be added between the gameobject and pointB. */
    public RopeJointSettings generalJointSettings = new RopeJointSettings();  /**< Settings that will be applied to all joints with exception of the end joints. */
    public RopeJointSettings pointAJointSettings = new RopeJointSettings() { basicJointSettings = new BasicJointSettings() { lowTwistLimit = -180, highTwistLimit = 180, swing1Limit = 180, swing2Limit = 180 } };  /**< Settings which will be applied to the starting joint. */
    public RopeJointSettings pointBJointSettings = new RopeJointSettings() { basicJointSettings = new BasicJointSettings() { lowTwistLimit = -180, highTwistLimit = 180, swing1Limit = 180, swing2Limit = 180 } };  /**< Settings which will be applied to the ending joint. */
    public bool hideRopeInScene = true;  /**< When true, this will allow the script to hide the joint gameobjects in the scene. It helps to keep your scene from getting cluttered when having multiple ropes or objects in the scene. */
    public RopeConstraint worldConstraints = RopeConstraint.None;  /**< Constrains the rope to a specified plane. */

    private Vector3 buildDirection = Vector3.down;
    private float buildDistance = 0;
    private float ropeRadius = 1;
    private List<GameObject> ropeJoints = new List<GameObject>();
    private GameObject controlObject = null;
    private bool isSuccess = false;

    // Temporary Variables
    private GameObject tJoint = null;
    private CharacterJoint tChar = null;
    private Rigidbody tRig = null;
    private SoftJointLimit nSJL;

    private GameObject tGO = null;
    private int prevTemplateSize = 0;
    private List<GameObject> gizmoTemplate = new List<GameObject>();

    // Public Getters
    public List<GameObject> Joints { get { return ropeJoints; } }  /**< Returns the entire list of joints. Index(0) will be pointB and Index(size) will be pointA of the rope. */
    public float BuildDistance { get { return buildDistance; } }  /**< Returns the distance which should be between 2 joints. */
    public Vector3 UpdatedBuildDirection { get { return (ropeJoints[ropeJoints.Count - 3].transform.position - transform.position).normalized; } }  /**< Returns the vector3 build direction. Can be used during updates or anytime after rope is initialized. */
    public Vector3 BuildDirection { get { return buildDirection; } }  /**< Returns the vector3 build direction. Best used during initialization of the rope. */
    public GameObject ControlJoint { get { return controlObject; } }  /**< Returns the current control object which when transformed will move the rope end. */
    public bool IsSuccess { get { return isSuccess; } }  /**< Returns whether the rope was constructed successfully. */
    public float RopeRadius { get { return ropeRadius; } set { ropeRadius = value; } }  /**< Returns or sets the current set radius of the rope. */
    public List<GameObject> GizmoTemplate { get { return gizmoTemplate; } } /**< The position that joints will be placed during runtime. This is used when previewing with gizmos. */

    /** Function that is called to upsdate the rope gizmos. Will not function when the showGizmos variable is set to false. */
    public void UpdateGizmos()
    {
        if (!pointB)
            pointB = GameObject.Find(gameObject.name + "_End");

        if (!showGizmos)
            return;

        if (pointB && !Application.isPlaying)
        {
            buildDirection = (pointB.transform.position - transform.position).normalized;
            buildDistance = Vector3.Distance(transform.position, pointB.transform.position) / jointSegments;

            if (jointSegments != prevTemplateSize)
            {
                try
                {
                    foreach (GameObject gt in gizmoTemplate)
                        DestroyImmediate(gt);
                }
                catch { }

                gizmoTemplate.Clear();

                for (int i = 0; i < jointSegments; i++)
                {
                    tGO = new GameObject("");
                    tGO.hideFlags = HideFlags.NotEditable | HideFlags.HideInInspector | HideFlags.HideInHierarchy;
                    tGO.transform.position = transform.position + (buildDirection * (i * buildDistance));
                    //tGO.transform.forward = -buildDirection;

                    gizmoTemplate.Add(tGO);
                }

                prevTemplateSize = jointSegments;
            }

            Gizmos.DrawLine(transform.position, pointB.transform.position);

            Gizmos.color = new Color(1f, 1f, 1f, 0.25f);

            if (pointAKinematic)
                Gizmos.color = Color.red;
            else
                Gizmos.color = Color.green;

            Gizmos.DrawWireSphere(transform.position, 0.25f);

            if (pointBKinematic)
                Gizmos.color = Color.red;
            else
                Gizmos.color = Color.green;

            Gizmos.DrawWireSphere(pointB.transform.position, 0.25f);
        }

        if (Application.isPlaying)
        {
            Gizmos.DrawWireCube(pointB.rigidbody.worldCenterOfMass, Vector3.one * 0.25f);
            Gizmos.DrawWireCube(rigidbody.worldCenterOfMass, Vector3.one * 0.25f);
        }

        Gizmos.color = Color.blue;

        if (ropeJoints.Count == 0)
            return;

        Gizmos.DrawRay(transform.position, UpdatedBuildDirection);

        Gizmos.DrawLine(transform.position, ropeJoints[ropeJoints.Count-1].transform.position);
        Gizmos.DrawWireSphere(ropeJoints[0].transform.position, 0.25f);
        for (int i = 1; i < ropeJoints.Count; i++)
        {
            Gizmos.DrawLine(ropeJoints[i - 1].transform.position, ropeJoints[i].transform.position);
            Gizmos.DrawWireSphere(ropeJoints[i].transform.position, 0.25f);

            Gizmos.color = Color.white;
            Gizmos.DrawWireCube(ropeJoints[i].rigidbody.worldCenterOfMass, Vector3.one * 0.1f);
            Gizmos.color = Color.grey;
        }
        Gizmos.DrawLine(pointB.transform.position, ropeJoints[0].transform.position);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(controlObject.transform.position, 0.35f);
    }

    /** Initializes the rope with the current settings. You may call this more than once but if you do not delete the rope it will result in multiple ropes between the same objects. 
     *
     * When rope is finished building, the function will send <b>OnRopeInitialized</b>. 
     * 
     * @returns bool If the rope was built successfully then this will be TRUE else it will be FALSE.
     */
    public bool InitializeRope()
    {
        if (!CheckSettings())
            return false;

        buildDirection = (pointB.transform.position - transform.position);
        buildDirection.Normalize();
        buildDistance =  Vector3.Distance(transform.position, pointB.transform.position) / jointSegments;

        SetRigidbody(gameObject, pointAJointSettings.rigidbodySettings);
        SetConstraints(gameObject.rigidbody, worldConstraints);

        ropeJoints.Add(gameObject);
        controlObject = gameObject;

        for (int i = 0; i < jointSegments; i++)
        {
            tJoint = new GameObject("nJoint_" + ropeJoints.Count);
            tJoint.transform.position = transform.position + (i * buildDirection * buildDistance);
            tJoint.transform.LookAt(tJoint.transform.position - buildDirection);
            //tJoint.transform.forward = -buildDirection;

            if(hideRopeInScene)
                tJoint.hideFlags = HideFlags.HideInInspector | HideFlags.NotEditable | HideFlags.HideInHierarchy;

            if (i == 0)
            {
                SetRigidbody(tJoint, pointAJointSettings.rigidbodySettings);
                SetCharacterJoint(tJoint, controlObject.rigidbody, new Vector3(0, 0, -1), pointAJointSettings);
                SetConstraints(tJoint.rigidbody, worldConstraints);
            }
            else
            {
                SetRigidbody(tJoint, generalJointSettings.rigidbodySettings);
                SetCharacterJoint(tJoint, controlObject.rigidbody, new Vector3(0, 0, -1), generalJointSettings);
                SetConstraints(tJoint.rigidbody, worldConstraints);
            }
            
            controlObject = tJoint;
            ropeJoints.Insert(0, tJoint);
        }

        SetRigidbody(pointB, pointBJointSettings.rigidbodySettings);
        SetCharacterJoint(pointB, tJoint.rigidbody, Vector3.zero, pointBJointSettings);
        SetConstraints(pointB.rigidbody, worldConstraints);

        ropeJoints.Insert(0, pointB);
        controlObject = ropeJoints[ropeJoints.Count-2];

        if (pointAKinematic)
            gameObject.rigidbody.isKinematic = true;
        if (pointBKinematic)
            pointB.rigidbody.isKinematic = true;

        SendMessage("OnRopeInitialized", SendMessageOptions.DontRequireReceiver);

        isSuccess = true;
        return isSuccess;
    }

    /** Adds a new joint at the position of pointA. When joint is added the function will send <b>OnRopeAddJoint</b>.  */
    public void AddJoint()
    {
        tJoint = new GameObject("nJoint_" + ropeJoints.Count);
        //tJoint.transform.forward = (ropeJoints[ropeJoints.Count - 2].transform.position - transform.position).normalized;
        tJoint.transform.position = transform.position;
        tJoint.transform.LookAt(tJoint.transform.position + UpdatedBuildDirection);

        if (hideRopeInScene)
            tJoint.hideFlags = HideFlags.HideInInspector | HideFlags.NotEditable | HideFlags.HideInHierarchy;

        SetRigidbody(tJoint, generalJointSettings.rigidbodySettings);
        SetCharacterJoint(tJoint, controlObject.rigidbody, new Vector3(0, 0, 1), generalJointSettings);
        SetConstraints(tJoint.rigidbody, worldConstraints);

        controlObject.transform.parent = null;
        controlObject.rigidbody.isKinematic = false;
        try { controlObject.GetComponent<CharacterJoint>().connectedBody = tJoint.rigidbody; }
        catch { controlObject.AddComponent<CharacterJoint>().connectedBody = tJoint.rigidbody; }

        tJoint.rigidbody.isKinematic = true;
        tJoint.transform.parent = transform;
        ropeJoints.Insert(ropeJoints.Count - 1, tJoint);

        controlObject = tJoint;
        SendMessage("OnRopeAddJoint", controlObject, SendMessageOptions.DontRequireReceiver);
    }

    /** Deletes the joint closest to pointA or the last joint created by AddJoint() method. When joint is deleted the function will send <b>OnRopeDeleteJoint</b>. */
    public void DeleteJoint()
    {
        ropeJoints.Remove(controlObject);
        Destroy(controlObject);
        controlObject = ropeJoints[ropeJoints.Count - 2];

        controlObject.transform.parent = transform;
        controlObject.rigidbody.isKinematic = true;

        ropeJoints.TrimExcess();

        SendMessage("OnRopeDeleteJoint", SendMessageOptions.DontRequireReceiver);
    }

    /** Destroys entire rope including the gameobject and pointB gameobject. */
    public void Destroy()
    {
        for (int i = 1; i < ropeJoints.Count; i++)
        {
            Destroy(ropeJoints[i]);
        }

        Destroy(pointB);
        Destroy(gameObject);

        Resources.UnloadUnusedAssets();
    }

    private bool CheckSettings()
    {
        if (pointB == null)
        {
            Debug.LogError("[" + gameObject.name + "] - PointB must have an object assigned to it for the rope to function.");
            return false;
        }

        //if (jointSegments > (int)Vector3.Distance(transform.position, pointB.transform.position)*2)
        //{
        //    jointSegments = (int)Mathf.Clamp(jointSegments, 1, (int)Vector3.Distance(transform.position, pointB.transform.position)*2);
        //    Debug.LogWarning("[" + gameObject.name + "] - The number of joint segments must not exceed 3 times the distance of both ends for stability reasons. If you require more joints please scale up your scene.");
        //}

        return true;
    }
    private void SetRigidbody(GameObject joint, RigidbodySettings settings)
    {
        try { tRig = joint.AddComponent<Rigidbody>(); }
        catch { tRig = joint.GetComponent<Rigidbody>(); }

        tRig.mass = settings.mass;
        tRig.drag = settings.drag;
        tRig.angularDrag = settings.angularDrag;
        tRig.useGravity = settings.useGravity;
        tRig.interpolation = settings.interpolation;
        tRig.collisionDetectionMode = settings.collisionMode;
    }
    private void SetConstraints(Rigidbody rBody, RopeConstraint constraints)
    {
        switch (constraints)
        {
            case RopeConstraint.None:
                rBody.constraints = RigidbodyConstraints.None;
                break;
            case RopeConstraint.X_Y:
                rBody.constraints = RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY;
                break;
            case RopeConstraint.Y_Z:
                rBody.constraints = RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ;
                break;
            case RopeConstraint.Z_X:
                rBody.constraints = RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
                break;
        }
    }
    private void SetCharacterJoint(GameObject joint, Rigidbody connectedBody, Vector3 jointDirection, RopeJointSettings settings)
    {
        if (!connectedBody)
            return;

        tChar = joint.AddComponent<CharacterJoint>();
        
        tChar.connectedBody = connectedBody;
        tChar.anchor = Vector3.zero;
        tChar.axis = jointDirection;

        nSJL = new SoftJointLimit();
        nSJL.limit = settings.basicJointSettings.lowTwistLimit;
        nSJL.bounciness = settings.advancedJointSettings.lowTwistBounce;
        nSJL.damper = settings.advancedJointSettings.lowTwistDamper;
        nSJL.spring = settings.advancedJointSettings.lowTwistSpring;
        tChar.lowTwistLimit = nSJL;

        nSJL.limit = settings.basicJointSettings.highTwistLimit;
        nSJL.bounciness = settings.advancedJointSettings.highTwistBounce;
        nSJL.damper = settings.advancedJointSettings.highTwistDamper;
        nSJL.spring = settings.advancedJointSettings.highTwistSpring;
        tChar.highTwistLimit = nSJL;

        nSJL.limit = settings.basicJointSettings.swing1Limit;
        nSJL.bounciness = settings.advancedJointSettings.swing1Bounce;
        nSJL.damper = settings.advancedJointSettings.swing1Damper;
        nSJL.spring = settings.advancedJointSettings.swing1Spring;
        tChar.swing1Limit = nSJL;

        nSJL.limit = settings.basicJointSettings.swing2Limit;
        nSJL.bounciness = settings.advancedJointSettings.swing2Bounce;
        nSJL.damper = settings.advancedJointSettings.swing2Damper;
        nSJL.spring = settings.advancedJointSettings.swing2Spring;
        tChar.swing2Limit = nSJL;

        tChar.breakForce = settings.advancedJointSettings.swingBreakForce;
        tChar.breakTorque = settings.advancedJointSettings.twistBreakForce;
    }

    /** Method that is used to attach a gameobject to the current rope object. 
     *
     * @param obj The gameobject which will be attached to this rope.
     * @param jointIndex The index of the joint inside the rope that obj will be attached to.
     * @param centerOnJoint Will the obj's position be set to that of the attaching joints posision.
     * @param offset The offset of the object after being centered if centerOnJoint is checked.
     */
    public void AttachObjectToRope(GameObject obj, int jointIndex, bool centerOnJoint, Vector3 offset)
    {
        if (jointIndex > Joints.Count - 1)
            return;

        if (centerOnJoint) { obj.transform.position = Joints[jointIndex].transform.position; }
        obj.transform.position += offset;
        FixedJoint fj = obj.AddComponent<FixedJoint>();
        fj.connectedBody = Joints[jointIndex].rigidbody;
    }
    /** Method that is used to attach a gameobject to the current rope object. 
     *
     * @param obj The gameobject which will be attached to this rope.
     * @param jointIndex The index of the joint inside the rope that obj will be attached to.
     * @param offset The offset of the obj.
     */
    public void AttachObjectToRope(GameObject obj, int jointIndex, Vector3 offset)
    {
        AttachObjectToRope(obj, jointIndex, false, obj.transform.position + offset);
    }
    /** Method that is used to attach a gameobject to the current rope object. 
     *
     * @param obj The gameobject which will be attached to this rope.
     * @param jointIndex The index of the joint inside the rope that obj will be attached to.
     * @param centerOnJoint Will the obj's position be set to that of the attaching joints posision.
     */
    public void AttachObjectToRope(GameObject obj, int jointIndex, bool centerOnJoint)
    {
        AttachObjectToRope(obj, jointIndex, centerOnJoint, Vector3.zero);
    }
    /** Method that is used to attach a gameobject to the current rope object. Obj will be automatically centered on the joint.
     *
     * @param obj The gameobject which will be attached to this rope.
     * @param jointIndex The index of the joint inside the rope that obj will be attached to.
     */
    public void AttachObjectToRope(GameObject obj, int jointIndex)
    {
        AttachObjectToRope(obj, jointIndex, true, Vector3.zero);
    }
	
	public void DetachObjectFromRope(GameObject objToRemove)
	{
		if(objToRemove.GetComponent<FixedJoint>() != null)
			Destroy(objToRemove.GetComponent<FixedJoint>());
	}
}

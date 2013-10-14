using UnityEngine;
using System.Collections;

/** \brief Raise and lower your ropes.
 * 
 * Control your ropes by adding this as a component to your rope script objects. With this component you will be able to raise and lower your joints. This is best used with slow speeds 
 * but for fast speeds use QuickRopeSmoothController.
 */
public class QuickRopeController : ControllerBASE 
{
    public float speed = 3.0f; /**< Sets the speed at which the rope is contracted or expanded.*/
    public KeyCode expandRopeControlKey = KeyCode.UpArrow; /**< This sets the key that will be used to make the rope larger (expand).*/
    public KeyCode contractRopeControlKey = KeyCode.DownArrow; /**< This sets the key that will be used to make the rope smaller (contract).*/

	void Start () 
    {
		speed = Mathf.Clamp(speed, 0f, 250f);
		minimumJoints = (int)Mathf.Clamp(minimumJoints, 2, Mathf.Infinity);
		maximumJoints = (int)Mathf.Clamp(maximumJoints, minimumJoints + 2, Mathf.Infinity);
	}

	void LateUpdate () 
    {
        if (Input.GetKey(expandRopeControlKey))
            UpdateControlState(speed);

        if (Input.GetKey(contractRopeControlKey))
            UpdateControlState(-speed);

        if (!Input.GetKey(contractRopeControlKey) && !Input.GetKey(expandRopeControlKey))
            UpdateControlState(0);
	}

    /** Function that is called which expands the rope. */
    public void ExpandRope()
    {
        UpdateControlState(speed);
    }

    /** Function that is called which contracts the rope. */
    public void ContractRope()
    {
        UpdateControlState(-speed);
    }

    /** Function that is called which stops the rope. */
    public void StopRope()
    {
        UpdateControlState(0);
    }
}

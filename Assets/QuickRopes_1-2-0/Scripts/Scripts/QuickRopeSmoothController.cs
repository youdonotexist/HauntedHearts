using UnityEngine;
using System.Collections;


/** \brief Smoothly raise and lower your ropes.
 * 
 * Control your ropes by adding this as a component to your rope script objects. With this component you will be able to raise and lower your rope joints smoothly as if
 * by the physics system. This is best used for faster movements and helps prevent jitter.
 */
public class QuickRopeSmoothController : ControllerBASE 
{
    public float rampSpeed = 3.0f; /**< The speed at which the velocity of the rope ramps up per frame of the key held down.*/
    public float maxSpeed = 5.0f; /**< The speed at which the ramp speed is no longer added to the velocity*/
    public float dampSpeed = 0.98f; /**< The speed at which the velocity is multiplied by per frame of the key held down.*/
    public float sleepSpeed = 0.01f; /**< The speed at which the velocity of the rope is set to zero.*/
    public KeyCode expandRopeControlKey = KeyCode.DownArrow; /**< This sets the key that will be used to make the rope larger (expand).*/
    public KeyCode contractRopeControlKey = KeyCode.UpArrow; /**< This sets the key that will be used to make the rope smaller (contract).*/

    private float controllerVelocity = 0;

	void Start () 
    {
		rampSpeed = Mathf.Clamp(rampSpeed, 0f, 250f);
		maxSpeed = Mathf.Clamp(maxSpeed, 0, 250f);
		dampSpeed = Mathf.Clamp(dampSpeed, 0, 5);
		sleepSpeed = Mathf.Clamp(sleepSpeed, 0, 1);
		minimumJoints = (int)Mathf.Clamp(minimumJoints, 2, Mathf.Infinity);
		maximumJoints = (int)Mathf.Clamp(maximumJoints, minimumJoints + 2, Mathf.Infinity);
	}

	void LateUpdate () 
    {
        if (Input.GetKey(expandRopeControlKey))
            controllerVelocity = Mathf.Clamp(controllerVelocity += Time.deltaTime * rampSpeed, -maxSpeed, maxSpeed);

        if (Input.GetKey(contractRopeControlKey))
            controllerVelocity = Mathf.Clamp(controllerVelocity -= Time.deltaTime * rampSpeed, -maxSpeed, maxSpeed);

        if (!Input.GetKey(contractRopeControlKey) && !Input.GetKey(expandRopeControlKey))
        {
            controllerVelocity *= dampSpeed;

            if (controllerVelocity < sleepSpeed && controllerVelocity > -sleepSpeed)
                controllerVelocity = 0;
        }

        UpdateControlState(controllerVelocity);

        if (AtMin || AtMax)
            controllerVelocity = 0;
	}

    /** Function that is called which expands the rope. */
    public void ExpandRope()
    {
        controllerVelocity = Mathf.Clamp(controllerVelocity += Time.deltaTime * rampSpeed, -maxSpeed, maxSpeed);
    }

    /** Function that is called which contracts the rope. */
    public void ContractRope()
    {
        controllerVelocity = Mathf.Clamp(controllerVelocity -= Time.deltaTime * rampSpeed, -maxSpeed, maxSpeed);
    }
}

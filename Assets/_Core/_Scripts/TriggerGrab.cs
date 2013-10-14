using UnityEngine;
using System.Collections;

public class TriggerGrab : MonoBehaviour
{
	public GameObject _triggerObject;
	// Use this for initialization
	
	void OnTriggerEnter(Collider c) {
		if (c.gameObject.name == "Boy") {
			_triggerObject = c.gameObject;	
		}
		else if (c.gameObject.name.Contains( "Crate")) {
			_triggerObject = c.gameObject;
		}
	}
	
	void OnTriggerExit(Collider c) {
		if (c.gameObject == _triggerObject)
			_triggerObject = null;
	}
}


using UnityEngine;
using System.Collections;

public class PassablePlatform : MonoBehaviour {
	public TriggerGrab crateTrigger;
	public TriggerGrab boyTrigger;
	public GameObject jumpPoint;
	
	public GameObject cratespawn;
	public GameObject crateprefab;
	
	public AudioClip _collideClip;
	
	void Update() {
		GameObject crate = crateTrigger._triggerObject != null ? crateTrigger._triggerObject.gameObject : this.gameObject;
		GameObject boy = boyTrigger._triggerObject != null ? boyTrigger._triggerObject.gameObject : this.gameObject;
		
		if (crate.name.Contains("Crate") && boy.name == "Boy") {
			CharacterMotor motor = boy.gameObject.GetComponentInChildren<CharacterMotor>();
			
			audio.PlayOneShot(_collideClip);
			
			boyTrigger.collider.enabled = false;
			boyTrigger._triggerObject = null;
			
			StartCoroutine(MoveBoy(motor));
		}
		else {
			if (crate.name.Contains("Crate")) {
				Destroy(crateTrigger._triggerObject);
				StartCoroutine(CreateCrate());
			}
		}
	}
	
	IEnumerator MoveBoy(CharacterMotor boy) {
		Vector3 boyPos = boy.transform.position;
		float elapsed = 0.0f;
			//boy.transform.position = Vector3.Lerp(boyPos, jumpPoint.transform.position, elapsed / 1.0f);
		
		foreach (Collider c in gameObject.GetComponentsInChildren<Collider>()) {
			//c.collider.enabled = false;
			Physics.IgnoreCollision(c, boy.collider, true);
		}
		
		while (Vector3.Distance(jumpPoint.transform.position, boy.transform.position) > 2.0f) {
			Vector3 dir = (jumpPoint.transform.position - boy.transform.position).normalized;
			boy.controller.Move(dir * 50.0f * Time.deltaTime);	
			yield return null;
		}
		
		//boy.canControl = true;
		//boy.controller.detectCollisions = true;
		boyTrigger._triggerObject = null;
		boyTrigger.collider.enabled = true;
		
		foreach (Collider c in gameObject.GetComponentsInChildren<Collider>()) {
			Physics.IgnoreCollision(c, boy.collider, false);
		}
	}
	
	IEnumerator CreateCrate() {
		yield return new WaitForSeconds(2.0f);
		Instantiate(crateprefab, cratespawn.transform.position, crateprefab.transform.rotation);	
	}
}

using UnityEngine;
using System.Collections;

public class EndTrigger : MonoBehaviour {
	
	public GameObject heart;
	void OnTriggerEnter(Collider c) {
		if (!c.gameObject.name.Contains("GhostBoy")) {
			PlayerControl pc = c.GetComponent<PlayerControl>();	
			if (pc != null) {
				//End Game
				pc._motor.canControl = false;
				StartCoroutine (End());
			}
		}
	}
	
	IEnumerator End() {
		Vector3 sc = heart.transform.localScale;
		
		heart.SetActive(true);
		
		while (true) {
			Vector3 sc1 = heart.transform.localScale; 
			sc1.x += 4.0f * Time.deltaTime;
			sc1.y += 4.0f * Time.deltaTime;
			sc1.z += 4.0f * Time.deltaTime;
			
			heart.transform.localScale = sc1; 
			yield return null;	
		}
	}
					
		
}

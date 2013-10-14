using UnityEngine;
using System.Collections;

public class Ladder : MonoBehaviour
{
	PlayerControl currentPlayer;
	public Transform ladderMount;
	public Transform ladderEnd;
	
	Vector3 startMountPoint;
	
	void Start() {
		startMountPoint = ladderMount.transform.localPosition;
	}
	
	void FixedUpdate() {
		if (currentPlayer) {
			bool moving = false;
			
			if (Input.GetKey(KeyCode.UpArrow)) {
				Vector3 mountPos = ladderMount.transform.position;
				mountPos.y += 5.0f * Time.deltaTime;
				ladderMount.transform.position = mountPos;			
				
				if (!currentPlayer._animation.IsPlaying("BoyClimb"))
					currentPlayer._animation.Play("BoyClimb");
				
				moving = true;
			}
			else if (Input.GetKey(KeyCode.DownArrow)) {
				Vector3 mountPos = ladderMount.transform.position;
				mountPos.y -= 5.0f * Time.deltaTime;
				ladderMount.transform.position = mountPos;			
				
				moving = true;
				//currentPlayer._motor.controller.Move (new Vector3(0.0f, 5.0f, 0.0f));
			}
			else if (Input.GetKey(KeyCode.RightArrow)) {
				currentPlayer._motor.canControl = true;
				currentPlayer._motor.controller.enabled = true;
				currentPlayer._motor.enabled = true;
				currentPlayer._motor.controller.Move(new Vector3(1.0f, 0.0f, 0.0f));
				currentPlayer.transform.parent = null;
				currentPlayer = null;
			}
			else if (Input.GetKey(KeyCode.LeftArrow)) {
				currentPlayer._motor.canControl = true;
				currentPlayer._motor.controller.enabled = true;
				currentPlayer._motor.enabled = true;
				currentPlayer._motor.controller.Move(new Vector3(-1.0f, 0.0f, 0.0f));
				currentPlayer.transform.parent = null;
				currentPlayer = null;
			}
			
			if (!moving) {
				if (currentPlayer != null) {
					currentPlayer._animation.Pause();	
				}
			}
			else {
				currentPlayer._animation.Resume();	
			}
		}
	}
	
	void OnTriggerEnter(Collider c) {
		if (c.name.Contains("GhostBoy")) {
			StartCoroutine(DropLadder());
		}
	}
	
	void OnTriggerStay(Collider c) {
		if (!c.gameObject.name.Contains("GhostBoy")) {
			if (currentPlayer == null) {
				if (Input.GetKey(KeyCode.UpArrow)) {
					currentPlayer =  c.GetComponent<PlayerControl>();
					ladderMount.transform.localPosition = startMountPoint;
					if (currentPlayer != null) {
						currentPlayer._motor.canControl = false;
						currentPlayer._motor.controller.enabled = false;
						currentPlayer._motor.enabled = false;
						currentPlayer._motor.sliding.enabled = false;
						currentPlayer.transform.parent = ladderMount.transform;
						currentPlayer.transform.localPosition = Vector3.zero;
					}
				}
			}
		}
	}
	
	IEnumerator DropLadder() {
		float duration = 2.0f;
		float elapsed = 0.0f;
		Vector3 start = transform.position;
		
		while (elapsed < duration) {
			transform.position = Vector3.Lerp(start, ladderEnd.position, elapsed/duration);
			elapsed += Time.deltaTime;
			yield return null;
		}
	}
}


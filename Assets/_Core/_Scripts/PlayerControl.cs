using UnityEngine;
using System.Collections;

public class PlayerControl : MonoBehaviour {
	
	public exSprite _sprite = null;
	public exSpriteAnimation _animation = null;
	public CharacterMotor _motor = null;
	
	PlayerControl _ghostPlayer = null;
	PlayerControl _boyPlayer = null;
	
	GameObject grabbedObject;
	
	public bool isGhost = false;
	public float pushPower = 2.0F;
	public GameObject ghostPrefab;
	
	bool isPushing = true;
	
	public string WalkAnimName;
	public string PushAnimName;
	public string StandAnimName;
	
	
	void Start() {
		_sprite = GetComponentInChildren<exSprite>();
		_animation = GetComponentInChildren<exSpriteAnimation>();
		_motor = GetComponent<CharacterMotor>();
		
		if (isGhost) {
			_boyPlayer = GameObject.Find("Boy").GetComponent<PlayerControl>();
			_ghostPlayer = this;
		}
		else {
			_boyPlayer = this;
		}
	}
	
	void Update() {
		
		if (Input.GetKeyDown(KeyCode.R)) {
			Application.LoadLevel("Scene01");	
		}
		//if hflip true, right
		//if hflip false, left
		Vector3 vel = _motor.movement.velocity;
		if (vel.x > 0.0f) {
			Vector3 scale = transform.localScale;
			scale.x = 1.0f;
			transform.localScale = scale;
		}
		else if (vel.x < 0.0f){
			Vector3 scale = transform.localScale;
			scale.x = -1.0f;
			transform.localScale = scale;
		}
		
		if (_motor.canControl) {
			if (Mathf.Abs(vel.x) > 0.0f) {
				if (!_animation.IsPlaying(WalkAnimName) && !_animation.IsPlaying(PushAnimName))
					_animation.Play(WalkAnimName);	
			}
			else {
				_animation.Stop();	
				_animation.PlayDefault();
			}
		}
		
		if (isGhost && _boyPlayer._sprite.collider.enabled == false) {
			if (Vector3.Distance(_boyPlayer.transform.position, _ghostPlayer.transform.position) > 4.0f) {
				_boyPlayer._sprite.collider.enabled = true;
			}
		}
		
		/*RaycastHit hit;
		if (Input.GetKey(KeyCode.DownArrow)) {
			
			if (grabbedObject != null)
				return;
			
			if (Physics.Raycast(transform.position, transform.forward * 7.0f, out hit, LayerMaskHelper.OnlyIncluding(LayerMask.NameToLayer("Environment")))) {
				Debug.Log("Hit");
				grabbedObject = hit.collider.gameObject;	
			}
			else {
				grabbedObject = null;	
			}
		}
		else {
			grabbedObject = null;	
		}
		
		if (grabbedObject != null && Input.GetKey(transform.forward.x < 0.0f ? KeyCode.LeftArrow : KeyCode.RightArrow)) {
			if (grabbedObject.rigidbody == null || grabbedObject.rigidbody.isKinematic) {
				return;
			}	
        
	        Vector3 pushDir = new Vector3(transform.forward.x * -1.0f, 0, 0.0f);
	        grabbedObject.rigidbody.velocity = pushDir * pushPower;
			
			if (!_animation.IsPlaying(PushAnimName))
				_animation.Play (PushAnimName);	
		}*/
	}
	
	void OnTriggerEnter(Collider c) {
		if (!isGhost) {
			if (gameObject.name.Contains("Capsule")) {
				DestroySpirit();
			}
		}
	}
	
	public void DestroySpirit() {
		Destroy (_ghostPlayer.gameObject);	
				
		_boyPlayer._motor.canControl = true;
		_ghostPlayer = null;
		
		FollowCam cam = Camera.mainCamera.GetComponent<FollowCam>();
		cam.targets[0] = _boyPlayer.transform;
		
		HeartController hc = GameObject.Find("HealthAmount").GetComponent<HeartController>();
		hc.SetAmt(0.25f);	
		
		Camera.mainCamera.audio.bypassEffects = true;
		Camera.mainCamera.audio.volume = 0.5f;
		
		_animation.PlayDefault();
		
	}
	
    void OnControllerColliderHit(ControllerColliderHit hit) {
        Rigidbody body = hit.collider.attachedRigidbody;
		
		if (hit.gameObject.name.Contains("Capsule") || hit.gameObject.name.Contains("GhostBoy")) {
				DestroySpirit();
		}
	
		if (body == null) {
			if (hit.collider.name == "StairTopCollider") {
				return;
				
			}
			else {
				return;	
			}
		}
		else {
			if (body.isKinematic) {
				return;	
			}
		}
        	
        
        if (hit.moveDirection.y < -0.3F)
            return;
        
        Vector3 pushDir = new Vector3(hit.moveDirection.x, 0, hit.moveDirection.z);
        body.velocity = pushDir * pushPower;
		
		if (!_animation.IsPlaying(PushAnimName))
			_animation.Play (PushAnimName);	
    }
	
	public bool SpiritOut() {
		return _ghostPlayer != null;	
	}
	
	public void SpawnSpirit() {
		if (!isGhost) {
			GameObject go = (GameObject) Instantiate(ghostPrefab, transform.position, transform.rotation);
			_ghostPlayer = go.GetComponent<PlayerControl>();
			
			_sprite.collider.enabled = false;
			
			_motor.canControl = false;
			
			FollowCam cam = Camera.mainCamera.GetComponent<FollowCam>();
			cam.targets[0] = go.transform;
			
			Physics.IgnoreCollision(go.GetComponent<CharacterController>(), transform.GetComponent<CharacterController>(), true);
			
			Camera.mainCamera.audio.volume = 0.2f;
			Camera.mainCamera.audio.bypassEffects = false;
			
			_animation.Play("BoyDead");
		}
	}
}

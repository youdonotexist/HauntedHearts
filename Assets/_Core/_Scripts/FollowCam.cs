using UnityEngine;
using System.Collections;

public class FollowCam : MonoBehaviour 
{
	//Vector3 velocity = Vector3.zero;
	//float dampTime = 0.3f; //offset from the viewport center to fix damping
	
	public Transform[] targets = new Transform[2];
	public float offset = 0.0f;
	
	public float interpSpeed = 1.0f;
	
	public Vector3 positionOffset = new Vector3(0,0,0);
	
	public float speedSensitivity = 1.0f;
	
	public GameObject PlayerPrefab;
	
	private float _moveTime = 5.0f;
	
	private Vector3 velocity = Vector3.zero;
	
	bool fixedFrame = false;
	
	
	//Parallax
	public Vector3 lastPosition = Vector3.zero;
	
	public GameObject Layer0;
	public GameObject Layer1;
	public GameObject Layer2;
	
	
	void Start()
	{
		//offset = camera.transform.position.y - targets[0].transform.position.y;
		//GameObject newPlayer = (GameObject)Instantiate(PlayerPrefab, GameObject.Find("Start").transform.position, new Quaternion(0,0,0,0));
		//newPlayer.name = "Player";
		//targets[0] = newPlayer.transform;
		
		//lastPosition = transform.position;
		//UpdateParallax(0.0f, true);
		lastPosition = transform.position;
	}
	
	void Awake()
	{
		
	}
	
	void Update()
	{
		
		if (Input.GetKey(KeyCode.R))
		{
			Destroy(GameObject.Find("Player"));
			GameObject newPlayer = (GameObject)Instantiate(PlayerPrefab, GameObject.Find("Start").transform.position, new Quaternion(0,0,0,0));
			newPlayer.name = "Player";
			targets[0] = newPlayer.transform;
		}
		
	}
	
	void FixedUpdate()
	{
		fixedFrame = true;	
	}
	
	void LateUpdate() {
		if(fixedFrame == true)
		{
	    	if(targets[0]) 
	    	{
				float speedScalar = 1.0f;
				
				CharacterMotor cm = targets[0].GetComponent<CharacterMotor>();
				if(cm != null)
				{
					speedScalar = speedSensitivity * cm.movement.velocity.magnitude;
					speedScalar = Mathf.Clamp(speedScalar, 1.0f, 2.0f);
				}
				else {
					PlayerControl pc = targets[0].GetComponent<PlayerControl>();
					speedScalar = speedSensitivity * pc.rigidbody.velocity.magnitude;
					speedScalar = Mathf.Clamp(speedScalar, 1.0f, 2.0f);
				}
				
				Vector3 point = camera.WorldToViewportPoint(targets[0].position);
	        	Vector3 delta = targets[0].position - camera.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, point.z));
	        	Vector3 destination = transform.position + delta;
				transform.position = Vector3.SmoothDamp(transform.position, destination + positionOffset, ref velocity, 0.5f, 10000000.0f, Time.deltaTime * interpSpeed * speedScalar);
				
				//UpdateParallax(Time.deltaTime * interpSpeed * speedScalar, false);
				lastPosition = transform.position;
	    	}
		}
		
		fixedFrame = false;
   }
	
	IEnumerator MoveToTarget() {
		if (targets[0]) {
			float elapsed = 0.0f;
			
			while (elapsed < _moveTime) {
				Vector3 forward = targets[0].transform.forward;
				Vector3 forwardOffset = forward * offset;// new Vector3 (offset, 0.0f, offset);
				transform.position = new Vector3(targets[0].transform.position.x + forwardOffset.x, transform.position.y, targets[0].transform.position.z + forwardOffset.z);
				yield return null;
			}
		}
	}
	
	/*void UpdateParallax(float delta, bool now) {
		float velx = lastPosition.x - transform.position.x;
		float vely = lastPosition.y - transform.position.y;
		if (Layer0 != null) {
			Vector3 pos = Layer0.transform.position;
			pos.x = -transform.position.x  * 0.3f;
			pos.y = -transform.position.y  * 0.3f;
			
			Vector3 vel = Vector3.zero;
			
			if (now) {
				Vector3 p = transform.position;
				p.z = Layer0.transform.position.z;
				Layer0.transform.position = p;
				Debug.Log(p.ToString());
			}
			else {
				Layer0.transform.position = pos;
			}
		}
		
		if (Layer1 != null) {
			Vector3 pos = Layer1.transform.position;
			pos.x = -transform.position.x  * 0.5f;
			pos.y = -transform.position.y  * 0.5f;
			
			Vector3 vel = Vector3.zero;
			
			if (now) {
				Vector3 p = transform.position;
				p.z = Layer1.transform.position.z;
				Layer1.transform.position = p;
				Debug.Log(p.ToString());
			}
			else {
				Layer1.transform.position = pos;
			}
		}
		
		if (Layer2 != null) {
			Vector3 pos = Layer2.transform.position;
			pos.x = -transform.position.x * 0.8f;
			pos.y = -transform.position.y * 0.8f;
			
			Vector3 vel = Vector3.zero;
			
			if (now) {
				Vector3 p = transform.position;
				p.z = Layer2.transform.position.z;
				Layer2.transform.position = p;
				Debug.Log(p.ToString());
			}
			else {
				Layer2.transform.position = pos;
			}
		}
	}*/
	
	void SetMoveTime(float movetime) {
		_moveTime = movetime;	
	}
	
	public void SetTarget(Transform target) {
		targets[0] = target;	
	}
	
	public Transform GetTarget() {
		return targets[0];
	}
}
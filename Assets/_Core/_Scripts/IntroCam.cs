using UnityEngine;
using System.Collections;

[RequireComponent (typeof (FollowCam))]
public class IntroCam : MonoBehaviour {
	
	private FollowCam followCam;
	
	IEnumerator ChangeTarget()
	{
		followCam.interpSpeed = 0.0f;
		
		// stuff
		yield return new WaitForSeconds(3.0f);
		
		followCam.interpSpeed = 0.5f;
		
		yield return new WaitForSeconds(3.0f);
		followCam.interpSpeed = 1.0f;
	}
	
	// Use this for initialization
	void Start () {
		followCam = GetComponent<FollowCam>();
		StartCoroutine(ChangeTarget());
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}

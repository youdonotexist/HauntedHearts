using UnityEngine;
using System.Collections;

public class Rope : MonoBehaviour
{

	
	MeshCollider _collider = null;
	InteractiveCloth _mesh = null;
	// Use this for initialization
	void Start ()
	{
		_collider = gameObject.AddComponent<MeshCollider>();
		_mesh = gameObject.GetComponent<InteractiveCloth>();
	}
	
	// Update is called once per frame
	void Update ()
	{
		Mesh m = new Mesh();
		m.vertices = _mesh.vertices;
		_collider.sharedMesh = m;
		
	}
	
	void OnCollisionEnter (Collision c) {
		Debug.Log (c);	
	}
}


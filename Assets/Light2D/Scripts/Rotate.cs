using UnityEngine;
using System.Collections;

public class Rotate : MonoBehaviour 
{
    public Vector3 axis = new Vector3(0, 0, 1);
    public float speed = 30.0f;

	void Update () 
    {
        transform.Rotate(axis * speed * Time.deltaTime);
	}
}

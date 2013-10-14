using UnityEngine;
using System.Collections;

public class SinWave : MonoBehaviour 
{
    public float magnitude = 5;
    public float speed = 10;

    private Vector3 pos = Vector3.zero;

    void Start()
    {
        pos = transform.position;
    }

	void Update () 
    {
        transform.position = new Vector3(pos.x - Mathf.Sin(Time.time * speed) * magnitude, pos.y, pos.z);
  	}
}

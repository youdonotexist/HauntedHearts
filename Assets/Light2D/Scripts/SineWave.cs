using UnityEngine;
using System;
using System.Collections;

public class SineWave : MonoBehaviour
{
    public bool useRandomStart = true;
    public Vector3 axis = Vector3.up;
    public float speed = 5.0f;
    public float magnitude = 1.0f;

    private float startTime = 0;
    private Vector3 startPos;

    void Start()
    {
        if (useRandomStart)
        {
            //UnityEngine.Random.seed = System.DateTime.Now.Millisecond;
            startTime = UnityEngine.Random.Range(-1500f, 1500f);
        }

        startPos = transform.localPosition;
    }


	void Update() 
    {
        transform.localPosition = startPos + (axis * Mathf.Sin((startTime + Time.time) * speed) * magnitude);
	}
}

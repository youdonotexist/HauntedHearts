using UnityEngine;using System.Collections;

public class SlideObject : MonoBehaviour 
{
    public Transform start;
    public Transform end;
    public int detail = 100;
    public float position = 50;

    public float speed = 5.0f;
    public float terminalSpeed = 30f;
    public float dampening = 0.98f;
    public KeyCode forwardKey = KeyCode.KeypadPlus;
    public KeyCode reverseKey = KeyCode.KeypadMinus;

    float velocity = 0;
    Vector3 direction = Vector3.zero;

	void Start () 
    {
        direction = (start.position - end.position)/detail;
	}
	
	void Update () 
    {
        velocity *= dampening;

        if (Input.GetKey(forwardKey)) { velocity += speed * Time.deltaTime; }
        if (Input.GetKey(reverseKey)) { velocity -= speed * Time.deltaTime; }

        velocity = Mathf.Clamp(velocity, -terminalSpeed, terminalSpeed);
        position += velocity;
        if (position >= 100 || position <= 0) { velocity = 0; }
        position = Mathf.Clamp(position, 0, 100);

        direction = (end.position - start.position) / detail;
        transform.position = start.position + (direction * Mathf.Clamp(position,0,detail));
	}
}

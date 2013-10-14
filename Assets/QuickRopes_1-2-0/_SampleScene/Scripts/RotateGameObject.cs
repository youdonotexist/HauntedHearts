using UnityEngine;
using System.Collections;

public class RotateGameObject : MonoBehaviour
{
    public Vector3 axis = Vector3.up;
    public float speed = 5.0f;
    public float terminalSpeed = 30f;
    public float dampening = 0.98f;
    public KeyCode addRotation = KeyCode.A;
    public KeyCode subRotation = KeyCode.D;
    float velocity = 0;

    void Update()
    {
        velocity *= dampening;

        if (Input.GetKey(addRotation)) { velocity += speed * Time.deltaTime; }
        if (Input.GetKey(subRotation)) { velocity -= speed * Time.deltaTime; }

        velocity = Mathf.Clamp(velocity, -terminalSpeed, terminalSpeed);

        transform.Rotate(axis * velocity);
    }
}

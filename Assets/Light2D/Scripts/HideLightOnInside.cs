using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(Light2D), typeof(SphereCollider), typeof(Rigidbody))]
public class HideLightOnInside : MonoBehaviour
{
    Light2D hLight;
    SphereCollider sCollider;

    void Start()
    {
        sCollider = GetComponent<SphereCollider>();
        sCollider.radius = 0.1f;
        collider.isTrigger = true;
        rigidbody.isKinematic = true;
        rigidbody.useGravity = false;
        hLight = GetComponent<Light2D>();
    }

    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.isStatic)
            return;

        hLight.LightEnable = false;
    }

    void OnTriggerExit(Collider col)
    {
        hLight.LightEnable = true;
    }
}

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EventSample : MonoBehaviour
{
    Color collidedColor = Color.red;
    Color nonCollidedColor = Color.blue;

    void Start()
    {
        Light2D.RegisterEventListener(LightEventListenerType.OnEnter, OnEnterEvent);
        Light2D.RegisterEventListener(LightEventListenerType.OnStay, OnStayEvent);
        Light2D.RegisterEventListener(LightEventListenerType.OnExit, OnExitEvent);
    }

    void OnEnterEvent(Light2D light, GameObject go)
    {
        if (go.GetInstanceID() == gameObject.GetInstanceID())
            light.lightColor = collidedColor;
    }

    void OnStayEvent(Light2D light, GameObject go)
    {
        // Stuff to do during "OnStay" event
    }

    void OnExitEvent(Light2D light, GameObject go)
    {
        if (go.GetInstanceID() == gameObject.GetInstanceID())
            light.lightColor = nonCollidedColor;
    }
}

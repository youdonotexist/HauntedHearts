using UnityEngine;
using System.Collections;

public class SceneSwapper : MonoBehaviour 
{
    void OnGUI()
    {
        if (GUI.Button(new Rect(Screen.width - 520, 10, 120, 25), "2500 Lights"))
            Application.LoadLevel(0);

        if (GUI.Button(new Rect(Screen.width - 390, 10, 120, 25), "Event Sample"))
            Application.LoadLevel(1);

        if (GUI.Button(new Rect(Screen.width - 260, 10, 120, 25), "Multiple Planes"))
            Application.LoadLevel(2);

        if (GUI.Button(new Rect(Screen.width - 130, 10, 120, 25), "RGB Lights"))
            Application.LoadLevel(3);
    }
}

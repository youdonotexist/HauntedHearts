using UnityEngine;
using UnityEditor;
using System.Collections;

public class Light2DMenu : Editor
{
    [MenuItem("GameObject/Create Other/2D RadialLight")]
    public static void CreateNewRadialLight()
    {
        Light2D light = Light2D.Create(Vector3.zero, (Material)Resources.Load("SampleLightMaterial"), Color.blue);
        light.shadowLayer = LayerMask.NameToLayer("Everything");
    }

    [MenuItem("GameObject/Create Other/2D SpotLight")]
    public static void CreateNewSpotLight()
    {
        Light2D light = Light2D.Create(Vector3.zero, (Material)Resources.Load("SampleLightMaterial"), Color.green);
        light.sweepSize = 45;
        light.sweepStart = -110;
        light.lightDetail = Light2D.LightDetailSetting.VeryLow;
        light.shadowLayer = LayerMask.NameToLayer("Everything");
    }
}

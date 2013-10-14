using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SpawnLight : MonoBehaviour 
{
    static int lightCount = 0;

    public bool randomPlacement = false;
    public int totalLights = 10;
    public float spawnSize = 50f;
    public float updateInterval = 0.5f;
    public float lightMinScale = 15;
    public float lightMaxScale = 35;

    private float progress = 0f;
    private float accum = 0.0f; // FPS accumulated over the interval
    private float frames = 0; // Frames drawn over the interval
    private float timeleft; // Left time for current interval
    private string display = "";

	// Use this for initialization
	void Start () 
    {
        timeleft = updateInterval;
        Random.seed = System.DateTime.Now.Millisecond;

        if(randomPlacement)
            StartCoroutine(AddRandomLights());
        else
            StartCoroutine(AddOrderedLights());

	}

    float lightIteration = 0.25f;
    float nextAdd = 0;
    void Update()
    {
        timeleft -= Time.deltaTime;
        accum += Time.timeScale / Time.deltaTime;
        ++frames;

        // Interval ended - update GUI text and start new interval
        if (timeleft <= 0.0f)
        {
            // display two fractional digits (f2 format)
            display = "" + (accum / frames).ToString("f2");
            timeleft = updateInterval;
            accum = 0.0f;
            frames = 0;
        }
    }

    IEnumerator AddRandomLights()
    {
        for (int i = 0; i < (totalLights*totalLights); i++)
        {
            nextAdd += lightIteration;
            AddNewLight(new Vector3(Random.Range(-(lightMinScale * lightCount) / 4, (lightMinScale * lightCount) / 4), Random.Range(-(lightMinScale * lightCount) / 4, (lightMinScale * lightCount) / 4), 0));
            yield return new WaitForFixedUpdate();
        }
    }

    IEnumerator AddOrderedLights()
    {
        for (int y = 0; y < totalLights; y++)
        {
            for (int x = 0; x < totalLights; x++)
            {
                AddNewLight(new Vector3((x * (lightMinScale / 2)), (y * (lightMinScale / 2)), 0));
                progress = (float)lightCount / ((float)totalLights * (float)totalLights);
                yield return new WaitForFixedUpdate();
            }
        }
    }

    void OnGUI()
    {
        //Light2D.LightUpdates = 0;

        GUI.Box(new Rect(0, 0, 120, 110), "");
        GUI.Label(new Rect(10, 10, 150, 30), "FPS: " + display);
        GUI.Label(new Rect(10, 40, 150, 30), "Lights: " + lightCount);
        GUI.Label(new Rect(10, 70, 150, 30), "Rendering: 0");

        GUI.Box(new Rect(0, 110, 120, 25), (progress).ToString("000%"));
    }

    void AddNewLight(Vector3 position)
    {
        Light2D l2d = Light2D.Create(position, (Material)Resources.Load("SampleLightMaterial"), new Color((float)Random.Range(0f, 1f), (float)Random.Range(0f, 1f), (float)Random.Range(0f, 1f), 1f), Random.Range(lightMinScale, lightMaxScale));
        l2d.gameObject.AddComponent<SineWave>();
        //GameObject g = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        //g.transform.position = l2d.transform.position;
        //g.transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);
        //g.isStatic = true;
        l2d.shadowLayer = LayerMask.NameToLayer("Everything");
        
        lightCount++;
    }
}

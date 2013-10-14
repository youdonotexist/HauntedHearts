using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum LightEventListenerType { OnEnter, OnStay, OnExit }
public delegate void Light2DEvent(Light2D lightObject, GameObject objectInLight);

[ExecuteInEditMode()]
public class Light2D : MonoBehaviour
{
    public enum LightDetailSetting
    {
        VeryLow     =   75,
        Low         =   250,
        Normal      =   500,
        Medium      =   750,
        High        =   1000,
        VeryHigh    =   2000,
        Extreme     =   4000
    }

    private static event Light2DEvent OnBeamEnter = null;
    private static event Light2DEvent OnBeamStay = null;
    private static event Light2DEvent OnBeamExit = null;

    public bool ignoreOptimizations = false;
    public bool useEvents = false;
    public float lightRadius = 25;
    public Color lightColor = Color.white;
    public Material lightMaterial = null;
    public float sweepStart = 0;
    public int sweepSize = 360;
    public LightDetailSetting lightDetail = LightDetailSetting.Normal;
    public LayerMask shadowLayer;

    private bool lightEnabled = true;
    public bool LightEnable
    {
        set { if (value) { lightObject.active = true; lightEnabled = true; } else { lightObject.active = false; lightEnabled = false; } }
        get { return lightEnabled; }
    }

    private bool updateMesh = true;
    private Mesh mesh;
    private Collider[] potentialShadowObject;
    private Color[] mColors = null;

    private MeshFilter loMesh = null;
    private GameObject lightObject = null;
    private List<int> tris = new List<int>();
    private List<Vector2> uvs = new List<Vector2>();
    private List<Vector3> verts = new List<Vector3>();
    private List<Vector3> norms = new List<Vector3>();

    void OnDrawGizmos()
    {
        Gizmos.color = Color.black;
        Gizmos.DrawWireSphere(transform.position, 0.25f);
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.magenta;
        Gizmos.DrawWireSphere(transform.position, 0.251f);
    }

    void GenerateLightObject()
    {
        lightObject = new GameObject("LO");
        lightObject.layer = gameObject.layer;
        lightObject.hideFlags = HideFlags.HideAndDontSave;

        lightObject.transform.position = transform.position;
        lightObject.transform.rotation = transform.rotation;

        loMesh = lightObject.AddComponent<MeshFilter>();
        lightObject.AddComponent<MeshRenderer>();

        loMesh.sharedMesh = new Mesh();

        UpdateMesh();
        UpdateColor();
    }

    // Update is called once per frame
    void LateUpdate()
    {
        UpdateLight2D();
		Collider c = collider;
		if (c == null) {
			c = gameObject.AddComponent<MeshCollider>();	
		}
		MeshCollider mc = (MeshCollider) c;
		mc.sharedMesh = loMesh.sharedMesh;
		
    }

    public void UpdateLight2D()
    {
        if (!lightObject)
            GenerateLightObject();

        if (!Application.isPlaying || (lightObject.renderer.isVisible && updateMesh) || (lightObject.renderer.isVisible && ignoreOptimizations))
        {
            UpdateMesh();
            UpdateColor();
        }

        if (!lightObject.renderer.isVisible)
            return;

        potentialShadowObject = Physics.OverlapSphere(transform.position, lightRadius / 2, shadowLayer);
        foreach (Collider c in potentialShadowObject)
        {
            if (c == gameObject.collider)
                continue;

            if (!c.gameObject.isStatic)
            {
                updateMesh = true;
                break;
            }
        }

        lightObject.transform.position = transform.position;
        lightObject.transform.rotation = transform.rotation;
    }

    private List<GameObject> knownObjects = new List<GameObject>(256);
    private List<GameObject> unknownObjects = new List<GameObject>(256);
    void UpdateMesh()
    {
        lightObject.transform.position = transform.position;
        lightObject.transform.rotation = transform.rotation;

        sweepSize = (int)Mathf.Clamp(sweepSize, 1, 360);
        lightRadius = Mathf.Clamp(lightRadius, 0.5f, 5000);
        lightObject.layer = gameObject.layer;

        loMesh.sharedMesh.Clear();
        verts.Clear();
        tris.Clear();
        norms.Clear();
        uvs.Clear();

        float ld = ((float)sweepSize / (int)lightDetail);
        Vector3 v1, v2;
        Vector3 pv = Vector3.zero;
        RaycastHit rh;

        if (useEvents && Application.isPlaying)
            unknownObjects.Clear();

        for (int i = 0; i < (int)lightDetail; i++)
        {
            verts.Add(Vector3.zero);
            uvs.Add(new Vector2(0.5f, 0.5f));

            if (i == 0)
            {
                v1 = new Vector3(lightRadius * (Mathf.Cos((sweepStart + (ld * i)) * Mathf.Deg2Rad)), lightRadius * (Mathf.Sin((sweepStart + (ld * i)) * Mathf.Deg2Rad)), 0) / 2;
                if (Physics.Raycast(transform.position, transform.TransformDirection(v1), out rh, lightRadius / 2, shadowLayer))
                {
                    v1 = lightObject.transform.InverseTransformPoint(rh.point);

                    if (useEvents && Application.isPlaying && !unknownObjects.Contains(rh.collider.gameObject))
                        unknownObjects.Add(rh.collider.gameObject);
                }

                verts.Add(v1);
                uvs.Add(new Vector2((0.5f + (v1.x) / lightRadius), (0.5f + (v1.y) / lightRadius)));
            }
            else
            {
                verts.Add(pv);
                uvs.Add(new Vector2((0.5f + (pv.x) / lightRadius), (0.5f + (pv.y) / lightRadius)));
            }


            v2 = pv = new Vector3(lightRadius * (Mathf.Cos((sweepStart + (ld * (i + 1))) * Mathf.Deg2Rad)), lightRadius * (Mathf.Sin((sweepStart + (ld * (i + 1))) * Mathf.Deg2Rad)), 0) / 2;
            if (Physics.Raycast(transform.position, transform.TransformDirection(v2), out rh, lightRadius / 2, shadowLayer))
            {
                v2 = pv = lightObject.transform.InverseTransformPoint(rh.point);

                if (useEvents && Application.isPlaying && !unknownObjects.Contains(rh.collider.gameObject))
                    unknownObjects.Add(rh.collider.gameObject);
            }

            verts.Add(v2);
            uvs.Add(new Vector2((0.5f + (v2.x) / lightRadius), (0.5f + (v2.y) / lightRadius)));

            tris.Add((i * 3) + 2); tris.Add((i * 3) + 1); tris.Add((i * 3) + 0);

            norms.Add(-transform.up); norms.Add(-transform.up); norms.Add(-transform.up);
        }

        if(useEvents && Application.isPlaying)
            DelegateEvents();

        loMesh.sharedMesh.vertices = verts.ToArray();
        loMesh.sharedMesh.uv = uvs.ToArray();
        loMesh.sharedMesh.uv2 = uvs.ToArray();
        loMesh.sharedMesh.normals = norms.ToArray();
        loMesh.sharedMesh.triangles = tris.ToArray();
        loMesh.sharedMesh.RecalculateBounds();

        lightObject.renderer.sharedMaterial = lightMaterial ?? CreateMaterial();
        
        UpdateColor();
        updateMesh = false;
    }

    Material CreateMaterial()
    {
        string shader =
            "Shader \"Mobile/Particles/Alpha Blended\" {\n" +
            "Properties {\n" +
                "_MainTex (\"Particle Texture\", 2D) = \"white\" {}\n" +
            "}\n\n" +
       
            "Category {\n" +
                "Tags { \"Queue\"=\"Transparent\" \"IgnoreProjector\"=\"True\" \"RenderType\"=\"Transparent\" }\n" +
                "Blend SrcAlpha OneMinusSrcAlpha\n"+
                "Cull Off Lighting Off ZWrite Off Fog { Color (0,0,0,0) }\n" +
                "BindChannels {\n" +
                    "Bind \"Color\", color\n" +
                    "Bind \"Vertex\", vertex\n" +
                    "Bind \"TexCoord\", texcoord\n" +
                "}\n" +
           
            "SubShader" + 
            "{\n" +
                "Pass" + 
                "{\n" +
                    "SetTexture [_MainTex] {\n" +
                    "combine texture * primary" +
                "}" +
            "}}}}";

        return new Material(shader);
    }

    void DelegateEvents()
    {
        for (int i = 0; i < unknownObjects.Count; i++)
        {
            if (knownObjects.Contains(unknownObjects[i]))
            {
                if (OnBeamStay != null)
                    OnBeamStay(this, unknownObjects[i]);
            }
            else
            {
                knownObjects.Add(unknownObjects[i]);

                if (OnBeamEnter != null)
                    OnBeamEnter(this, unknownObjects[i]);
            }
        }

        for (int i = 0; i < knownObjects.Count; i++)
        {
            if (!unknownObjects.Contains(knownObjects[i]))
            {
                if (OnBeamExit != null)
                    OnBeamExit(this, knownObjects[i]);

                knownObjects.Remove(knownObjects[i]);
            }
        }
    }

    public static void RegisterEventListener(LightEventListenerType eventType, Light2DEvent eventMethod)
    {
        if (eventType == LightEventListenerType.OnEnter)
            OnBeamEnter += eventMethod;

        if (eventType == LightEventListenerType.OnStay)
            OnBeamStay += eventMethod;

        if (eventType == LightEventListenerType.OnExit)
            OnBeamExit += eventMethod;
    }
    public static void UnregisterEventListener(LightEventListenerType eventType, Light2DEvent eventMethod)
    {
        if (eventType == LightEventListenerType.OnEnter)
            OnBeamEnter -= eventMethod;

        if (eventType == LightEventListenerType.OnStay)
            OnBeamStay -= eventMethod;

        if (eventType == LightEventListenerType.OnExit)
            OnBeamExit -= eventMethod;
    }

    void UpdateColor()
    {
        if (mColors == null || mColors.Length != loMesh.sharedMesh.vertexCount || mColors[0] != lightColor || !Application.isPlaying)
        {
            Mesh m = lightObject.GetComponent<MeshFilter>().sharedMesh;
            mColors = new Color[m.vertexCount];
            for (int c = 0; c < mColors.Length; c++)
            {
                mColors[c] = lightColor;
            }
            m.colors = mColors;
        }
        else
        {
            lightObject.GetComponent<MeshFilter>().sharedMesh.colors = mColors;
        }
    }

    void OnDestroy()
    {
        OnBeamEnter = null;
        OnBeamExit = null;
        OnBeamStay = null;
        DestroyImmediate(lightObject);
    }

    void OnEnable()
    {
        if(lightObject)
            lightObject.active = true;
    }

    void OnDisable()
    {
        if(lightObject)
            lightObject.active = false;
    }

    public static Light2D Create(Vector3 position, Material lightMaterial, Color lightColor)
    {
        return Create(position, lightMaterial, lightColor, 10, 0, 360, LightDetailSetting.Normal);
    }

    public static Light2D Create(Vector3 position, Material lightMaterial, Color lightColor, float lightRadius)
    {
        return Create(position, lightMaterial, lightColor, lightRadius, 0, 360, LightDetailSetting.Normal);
    }

    public static Light2D Create(Vector3 position, Material lightMaterial, Color lightColor, float lightRadius, float sweepStart, int sweepSize)
    {
        return Create(position, lightMaterial, lightColor, lightRadius, sweepStart, sweepSize, LightDetailSetting.Normal);
    }

    public static Light2D Create(Vector3 position, Material lightMaterial, Color lightColor, float lightRadius, float sweepStart, int sweepSize, LightDetailSetting detailSetting)
    {
        GameObject go = new GameObject("Created Light");
        go.transform.position = position;
        
        Light2D nLight = go.AddComponent<Light2D>();
        nLight.lightMaterial = lightMaterial;
        nLight.lightColor = lightColor;
        nLight.lightRadius = lightRadius;
        nLight.sweepStart = sweepStart;
        nLight.sweepSize = sweepSize;
        nLight.lightDetail = detailSetting;

        return nLight;
    }
}

using UnityEngine;
using System.Collections;

public class RopeRandomize : MonoBehaviour 
{
    public float buildDelay = 0.5f;
    public GameObject linkPrefab;
    public Material lineMaterial;
    public Material meshMaterial;

    private float nextBuild = 0;

	// Use this for initialization
	void Start () 
    {
        RandomGenerateRope();
        nextBuild = buildDelay;
	}
	
	// Update is called once per frame
	void Update () 
    {
        if (Time.time >= nextBuild)
        {
            prevRope.Destroy();
            RandomGenerateRope();
            nextBuild = Time.time + buildDelay;
        }
	
	}

    int curCase = 0;
    QuickRopeBASE prevRope = null;
    void RandomGenerateRope()
    {
        curCase = GetRandom(curCase);

        switch (curCase)
        {
            case 0:
                QuickRopeLineRope qlr = QuickRopes.LineRope(new Vector3(-10, 0, 0), new Vector3(10, 0, 0));
                qlr.ropeMaterial = lineMaterial;
                qlr.pointBKinematic = false;
                prevRope = qlr;
                break;
            case 1:
                QuickRopeMeshRope qmr = QuickRopes.MeshRope(new Vector3(-10, 0, 0), new Vector3(10, 0, 0));
                qmr.material = meshMaterial;
                qmr.crossSegments = 6;
                qmr.pointBKinematic = false;
                prevRope = qmr;
                break;
            case 2:
                QuickRopePrefabRope qpr = QuickRopes.PrefabRope(new Vector3(-10, 0, 0), new Vector3(10, 0, 0), linkPrefab);
                qpr.meshRotation = new Vector3(0, 90, 0);
                qpr.altMeshRotation = new Vector3(90, 0, 90);
                qpr.meshScale = 0.8f;
                qpr.pointBKinematic = false;
                prevRope = qpr;
                break;
        }
    }

    int GetRandom(int last)
    {
        int cur = Random.Range(0, 3);
        if (cur == last)
            return (int)Mathf.Repeat(cur + 1, 3);
        else
            return cur;
    }
}

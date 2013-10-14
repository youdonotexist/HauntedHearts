/** \cond Excluded from documentation */
using UnityEngine;
using UnityEditor;
using System.Collections;

public class QuickRopeHelpers : MonoBehaviour 
{	
    static QuickRopeBASE ropeObject;
    [MenuItem("QuickRopes/GameObjects/Prefab Rope")]
    static void AddPrefabRope()
    {
		
        GameObject ropeA = new GameObject("New PrefabRope_A");
        GameObject ropeB = new GameObject("New PrefabRope_B");
		
		Undo.RegisterCreatedObjectUndo(ropeA ,"Create Prefab Rope");
		Undo.RegisterCreatedObjectUndo(ropeB ,"Create Prefab Rope");
		
        ropeA.transform.position = new Vector3(-10, 0, 0);
        ropeB.transform.position = new Vector3(10, 0, 0);

        ropeObject = QuickRopes.PrefabRope(ropeA, ropeB, null);
        ropeObject.jointSegments = 10;
    }
    [MenuItem("QuickRopes/GameObjects/Mesh Rope")]
    static void AddMeshRope()
    {			
        GameObject ropeA = new GameObject("New MeshRope_A");
        GameObject ropeB = new GameObject("New MeshRope_B");
		
		Undo.RegisterCreatedObjectUndo(ropeA ,"Create Mesh Rope");
		Undo.RegisterCreatedObjectUndo(ropeB ,"Create Mesh Rope");
		
        ropeA.transform.position = new Vector3(-10, 0, 0);
        ropeB.transform.position = new Vector3(10, 0, 0);

        ropeObject = QuickRopes.MeshRope(ropeA, ropeB);
        ropeObject.jointSegments = 10;
    }
    [MenuItem("QuickRopes/GameObjects/Line Rope")]
    static void AddLineRope() 
    {			
        GameObject ropeA = new GameObject("New LineRope_A");
        GameObject ropeB = new GameObject("New LineRope_B");
		
		Undo.RegisterCreatedObjectUndo(ropeA ,"Create Line Rope");
		Undo.RegisterCreatedObjectUndo(ropeB ,"Create Line Rope");
		
        ropeA.transform.position = new Vector3(-10,0,0);
        ropeB.transform.position = new Vector3(10, 0, 0);

        ropeObject = QuickRopes.LineRope(ropeA, ropeB);
        ropeObject.jointSegments = 10;
    }
    [MenuItem("QuickRopes/GameObjects/Invisible Rope")]
    static void AddInvisibleRope()
    {	
        GameObject ropeA = new GameObject("New InvisibleRope_A");
        GameObject ropeB = new GameObject("New InvisibleRope_B");
		
		Undo.RegisterCreatedObjectUndo(ropeA ,"Create Invisible Rope");
		Undo.RegisterCreatedObjectUndo(ropeB ,"Create Invisible Rope");

        ropeA.transform.position = new Vector3(-10, 0, 0);
        ropeB.transform.position = new Vector3(10, 0, 0);

        ropeObject = QuickRopes.InvisibleRope(ropeA, ropeB);
        ropeObject.jointSegments = 10;
    }
    [MenuItem("QuickRopes/GameObjects/Spline Line Rope")]
    static void AddSplineLineRope()
    {
        GameObject ropeA = new GameObject("New SplineLineRope_A");
        GameObject ropeB = new GameObject("New SplineLineRope_B");

        Undo.RegisterCreatedObjectUndo(ropeA, "Create SplineLine Rope");
        Undo.RegisterCreatedObjectUndo(ropeB, "Create SplineLine Rope");

        ropeA.transform.position = new Vector3(-10, 0, 0);
        ropeB.transform.position = new Vector3(10, 0, 0);

        ropeObject = QuickRopes.SplineLineRope(ropeA, ropeB);
        ropeObject.jointSegments = 10;
    }
    [MenuItem("QuickRopes/GameObjects/Spline Mesh Rope")]
    static void AddSplineMeshRope()
    {
        GameObject ropeA = new GameObject("New SplineMeshRope_A");
        GameObject ropeB = new GameObject("New SplineMeshRope_B");

        Undo.RegisterCreatedObjectUndo(ropeA, "Create SplineMesh Rope");
        Undo.RegisterCreatedObjectUndo(ropeB, "Create SplineMesh Rope");

        ropeA.transform.position = new Vector3(-10, 0, 0);
        ropeB.transform.position = new Vector3(10, 0, 0);

        ropeObject = QuickRopes.SplineMeshRope(ropeA, ropeB);
        ropeObject.jointSegments = 10;
    }

    [MenuItem("QuickRopes/Scripts/Basic Control")]
    static void AddRopeController()
    {
        foreach (GameObject go in Selection.gameObjects)
        {
            if (go.GetComponent<QuickRopeSmoothController>() != null)
                DestroyImmediate(go.GetComponent<QuickRopeSmoothController>());

            if (go.GetComponent<QuickRopeController>() != null)
                return;

            if (go.GetComponent<QuickRopeBASE>() == null)
            {
                Debug.Log("Gameobject must have a QuickRope type attached to it.");
                return;
            }

            Undo.RegisterUndo(go, "Add Basic Controller " + go.name);
            go.AddComponent<QuickRopeController>();
        }
    }
    [MenuItem("QuickRopes/Scripts/Smooth Control")]
    static void AddRopeSmoothController()
    {
        foreach (GameObject go in Selection.gameObjects)
        {
            if (go.GetComponent<QuickRopeController>() != null)
                DestroyImmediate(go.GetComponent<QuickRopeController>());

            if (go.GetComponent<QuickRopeSmoothController>() != null)
                return;

            if (go.GetComponent<QuickRopeBASE>() == null)
            {
                Debug.Log("Gameobject must have a QuickRope type attached to it.");
                return;
            }

            Undo.RegisterUndo(go, "Add Smooth Controller " + go.name);
            go.AddComponent<QuickRopeSmoothController>();
        }
    }
    [MenuItem("QuickRopes/Scripts/Basic Collider")]
    static void AddRopeColliders()
    {
        foreach (GameObject go in Selection.gameObjects)
        {
            if (go.GetComponent<QuickRopeCollider>() != null)
                return;

            if (go.GetComponent<QuickRopeBASE>() == null)
            {
                Debug.Log("Gameobject must have a QuickRope type attached to it.");
                return;
            }

            Undo.RegisterUndo(go, "Add Rope Colliders " + go.name);
            go.AddComponent<QuickRopeCollider>();
        }
    }
    [MenuItem("QuickRopes/Scripts/Box Collider")]
    static void AddRopeBoxColliders()
    {
        foreach (GameObject go in Selection.gameObjects)
        {
            if (go.GetComponent<QuickRopeBoxCollider>() != null)
                return;

            if (go.GetComponent<QuickRopeBASE>() == null)
            {
                Debug.Log("Gameobject must have a QuickRope type attached to it.");
                return;
            }

            Undo.RegisterUndo(go, "Add Rope Box Colliders " + go.name);
            go.AddComponent<QuickRopeBoxCollider>();
        }
    }
    [MenuItem("QuickRopes/Scripts/Attach Objects")]
    static void AddAttachObjects()
    {
        foreach (GameObject go in Selection.gameObjects)
        {
            if (go.GetComponent<QuickRopeAttachObjects>() != null)
                return;

            if (go.GetComponent<QuickRopeBASE>() == null)
            {
                Debug.Log("Gameobject must have a QuickRopfe type attached to it.");
                return;
            }

            Undo.RegisterUndo(go, "Add Rope Box Colliders " + go.name);
            go.AddComponent<QuickRopeAttachObjects>();
        }
    }
	

    [MenuItem("QuickRopes/Components/QuickRope PrefabRope")]
    static void AddPrefabRopeComponent()
    {
        foreach (GameObject go in Selection.gameObjects)
        {
            if (go.GetComponent<QuickRopePrefabRope>() != null)
                return;
			
			Undo.RegisterUndo(go, "Add PrefabRope " + go.name);
            go.AddComponent<QuickRopePrefabRope>();
        }
    }
    [MenuItem("QuickRopes/Components/QuickRope MeshRope")]
    static void AddMeshRopeComponent()
    {
        foreach (GameObject go in Selection.gameObjects)
        {
            if (go.GetComponent<QuickRopeMeshRope>() != null)
                return;
			
			Undo.RegisterUndo(go, "Add MeshRope " + go.name);
            go.AddComponent<QuickRopeMeshRope>();
        }
    }
    [MenuItem("QuickRopes/Components/QuickRope LineRope")]
    static void AddLineRopeComponent()
    {
        foreach (GameObject go in Selection.gameObjects)
        {
            if (go.GetComponent<QuickRopeLineRope>() != null)
                return;
			
			Undo.RegisterUndo(go, "Add LineRope " + go.name);
            go.AddComponent<QuickRopeLineRope>();
        }
    }
    [MenuItem("QuickRopes/Components/QuickRope InvisibleRope")]
    static void AddInvisibleRopeComponent()
    {
        foreach (GameObject go in Selection.gameObjects)
        {
            if (go.GetComponent<QuickRopeInvisible>() != null)
                return;
			
			Undo.RegisterUndo(go, "Add InvisibleRope " + go.name);
            go.AddComponent<QuickRopeInvisible>();
        }
    }
    [MenuItem("QuickRopes/Components/QuickRope SplineLineRope")]
    static void AddSplineLineRopeComponent()
    {
        foreach (GameObject go in Selection.gameObjects)
        {
            if (go.GetComponent<QuickRopeSplineLineRope>() != null)
                return;

            Undo.RegisterUndo(go, "Add SplineLineRope " + go.name);
            go.AddComponent<QuickRopeSplineLineRope>();
        }
    }
    [MenuItem("QuickRopes/Components/QuickRope SplineLineRope")]
    static void AddSplineMeshRopeComponent()
    {
        foreach (GameObject go in Selection.gameObjects)
        {
            if (go.GetComponent<QuickRopeSplineMeshRope>() != null)
                return;

            Undo.RegisterUndo(go, "Add SplineLineRope " + go.name);
            go.AddComponent<QuickRopeSplineMeshRope>();
        }
    }

	[MenuItem("QuickRopes/Help/Documentation")]
	static void OpenQuickRopesHelp()
	{
		Application.OpenURL("http://reverieinteractive.com/quickrope-documentation");
	}
	[MenuItem("QuickRopes/Help/Contact Me")]
	static void OpenQuickRopesEmail()
	{
		Application.OpenURL("http://reverieinteractive.com/contact");
	}
}
/** \endcond */
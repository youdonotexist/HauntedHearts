using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/** \brief Custom spline for rope mesh.
 * 
 * This class creates splines from lists of gameobjects or lists of Vector3.
 */
public class RopeSpline
{
    public List<GameObject> pts = new List<GameObject>(); /**< List of gameobjects to represent the control points on the spline */
    int splineDetail = 20;

    /** Default constructor for this class. */
    public RopeSpline() { }

    /** Clears the entire list of points that has been set. */
    public void ClearSplinePoints()
    {
        pts.Clear();
    }

    /** Inserts a new gameobject into the list of points at the index
     * 
     * @param index The index at which to place the new point.
     * @param obj The object representing the point.
     */
    public void InsertSplinePoint(int index, GameObject obj)
    {
        pts.Insert(index, obj);
    }

    /** Inserts a new gameobject into the list of points at end of list
     * 
     * @param obj The object representing the point.
     */
    public void AddSplinePoint(GameObject obj)
    {
        pts.Add(obj);
    }

    /** Inserts a new list of gameobjects into the list of points at the end of list
     * 
     * @param ropeJoints The list of points representing the point.
     */
    public void AddSplinePointRange(List<GameObject> ropeJoints)
    {
        pts.AddRange(ropeJoints);
    }

    /** Used to interpolate the position along the spline. 0 = beginning, 1 = ending 
     *
     * @param t The value 0-1 that should be used to interpolate the position on the spline.
     * @returns Vector3 Returns a position along the spline at t.
     */
    public Vector3 Interp(float t)
    {
        try
        {
            if (pts.Count <= 3)
                return Vector3.zero;

            int numSections = pts.Count - 3;
            int currPt = Mathf.Min(Mathf.FloorToInt(t * (float)numSections), numSections - 1);
            float u = t * (float)numSections - (float)currPt;

            Vector3 a = pts[currPt].transform.position;
            Vector3 b = pts[currPt + 1].transform.position;
            Vector3 c = pts[currPt + 2].transform.position;
            Vector3 d = pts[currPt + 3].transform.position;

            return .5f * (
                    (-a + 3f * b - 3f * c + d) * (u * u * u)
                    + (2f * a - 5f * b + 4f * c - d) * (u * u)
                    + (-a + c) * u
                    + 2f * b
            );
        }
        catch { return new Vector3(0,0,0); }
    }

    private Vector3 currPt = Vector3.zero;
    /** 
     * Draws the gizmos to represent the spline object.
     * 
     * @param t Shows where the interpolation is going to occur (Represented by a box)
     */
    public void GizmoDraw(float t)
    {
        if (pts.Count == 0)
            return;

        if (pts.Count <= 3)
            return;

        Gizmos.color = Color.white;
        Vector3 prevPt = Interp(0);

        for (int i = 1; i <= splineDetail; i++)
        {
            currPt = Interp((float)i / (float)splineDetail);
            Gizmos.DrawLine(currPt, prevPt);
            prevPt = currPt;
        }

        if (t < 0)
            return;

        Gizmos.color = Color.cyan;
        Gizmos.DrawWireCube(Interp(t), new Vector3(0.4f, 0.4f, 0.4f));
    }
}

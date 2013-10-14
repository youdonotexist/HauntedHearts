/*! \mainpage QuickRopes Unity rope physics tool!
 *
 * \section intro_sec Introduction
 *
 * Thank you for choosing QuickRopes! The following documentation covers <b>V1.1</b> of QuickRopes.
 *
 * <b>If you have an earlier version of QuickRopes, please visit <a href="http://reverieinteractive.com/quickrope-documentation/v_1_0">V1.0 Documentation</a></b>
 */

/**  \page todo The TODO List
 * 
 *  \section TODO
 *   - Need to impliment the QuickRopeClothRope type.
*/

/** \page bugs Known Bugs
 * 
 *  \section Bugs
 *   - No Known Bugs
 */


using UnityEngine;
using System.Collections;

/** \brief A quick way to create ropes via script. 
 *
 * This class is used primarily to generate ropes during runtime. Simply call one of these methods and a rope will be generated during runtime!
 */
public sealed class QuickRopes
{
    static GameObject pA;
    static GameObject pB;

    #region QuickRopes Invisible Rope Static Methods
    static QuickRopeInvisible rObj;
    /** Generates an Invisible Rope. The invisible rope is used if you want objects to react as if they were on a rope without being able to see the rope. 
     *
     * @param pointA A gameobject that will contain the rope script. It wall also be the starting point of the rope.
     * @param pointB A gameobject that will contain the ropes ending point.
     * @param pointAKinematic Sets point A to be either Kinematic or free falling.
     * @param pointBKinematic Sets point B to be either Kinematic or free falling.
     * @param worldConstraints Allows you to constrain the created rope to one of 3 axis'.
     * @returns QuickRopeInvisible
     */
    public static QuickRopeInvisible InvisibleRope(GameObject pointA, GameObject pointB, bool pointAKinematic, bool pointBKinematic, RopeConstraint worldConstraints)
    {
        rObj = pointA.AddComponent<QuickRopeInvisible>();
        rObj.pointB = pointB;
        rObj.pointAKinematic = pointAKinematic;
        rObj.pointBKinematic = pointBKinematic;
        rObj.worldConstraints = worldConstraints;
        return rObj;
    }
    /** Generates a Mesh Rope. The end points will be set to kinematic and the world constraints will be set to none.
     *
     * @param pointA A gameobject that will contain the rope script. It wall also be the starting point of the rope.
     * @param pointB A gameobject that will contain the ropes ending point.
     * @returns QuickRopeInvisible
     */
    public static QuickRopeInvisible InvisibleRope(GameObject pointA, GameObject pointB)
    {
        return QuickRopes.InvisibleRope(pointA, pointB, true, true, RopeConstraint.None);
    }
    /** Generates a Mesh Rope. The end points will be set to kinematic and the world constraints will be set to none.
     *
     * @param pointA The position that the rope will put the pointA gameobject and script. It wall also be the starting point of the rope.
     * @param pointB The position of the ending point.
     * @returns QuickRopeInvisible
     */
    public static QuickRopeInvisible InvisibleRope(Vector3 pointA, Vector3 pointB)
    {
        pA = new GameObject("InvisibleRope_A");
        pA.transform.position = pointA;
        pB = new GameObject("InvisibleRope_B");
        pB.transform.position = pointB;

        return QuickRopes.InvisibleRope(pA, pB, true, true, RopeConstraint.None);
    }
    #endregion

    #region QuickRopes Prefab Rope Static Methods
    static QuickRopePrefabRope prObj;
    /** Generates a Prefab Rope. A prefab rope is the type of rope which will generate meshes based off of the mesh template provided. This is best used for chains! 
     *
     * @param pointA A gameobject that will contain the rope script. It wall also be the starting point of the rope.
     * @param pointB A gameobject that will contain the ropes ending point.
     * @param meshTemplate The prefab that will be used to generate the mesh.
     * @param pointAKinematic Sets point A to be either Kinematic or free falling.
     * @param pointBKinematic Sets point B to be either Kinematic or free falling.
     * @param worldConstraints Allows you to constrain the created rope to one of 3 axis'.
     * @returns QuickRopePrefabRope
     */
    public static QuickRopePrefabRope PrefabRope(GameObject pointA, GameObject pointB, GameObject meshTemplate, bool pointAKinematic, bool pointBKinematic, RopeConstraint worldConstraints)
    {
        prObj = pointA.AddComponent<QuickRopePrefabRope>();
        prObj.pointB = pointB;
        prObj.pointAKinematic = pointAKinematic;
        prObj.pointBKinematic = pointBKinematic;
        prObj.worldConstraints = worldConstraints;
        prObj.meshTemplate = meshTemplate;

        return prObj;
    }
    /** Generates a Prefab Rope. The end points will be set to kinematic and the world constraints will be set to none.
     *
     * @param pointA A gameobject that will contain the rope script. It wall also be the starting point of the rope.
     * @param pointB A gameobject that will contain the ropes ending point.
     * @param meshTemplate The prefab that will be used to generate the mesh.
     * @returns QuickRopePrefabRope
     */
    public static QuickRopePrefabRope PrefabRope(GameObject pointA, GameObject pointB, GameObject meshTemplate)
    {
        return QuickRopes.PrefabRope(pointA, pointB, meshTemplate, true, true, RopeConstraint.None);
    }
    /** Generates a Prefab Rope. The end points will be set to kinematic and the world constraints will be set to none.
     *
     * @param pointA The position that the rope will put the pointA gameobject and script. It wall also be the starting point of the rope.
     * @param pointB The position of the ending point.
     * @param meshTemplate The prefab that will be used to generate the mesh.
     * @returns QuickRopePrefabRope
     */
    public static QuickRopePrefabRope PrefabRope(Vector3 pointA, Vector3 pointB, GameObject meshTemplate)
    {
        pA = new GameObject("PrefabRope_A");
        pA.transform.position = pointA;
        pB = new GameObject("PrefabRope_B");
        pB.transform.position = pointB;

        return QuickRopes.PrefabRope(pA, pB, meshTemplate, true, true, RopeConstraint.None);
    }
    #endregion

    #region QuickRopes Mesh Rope Static Methods
    static QuickRopeMeshRope mrObj;
    /** Generates a Mesh Rope. A mesh rope is the type of rope which will generate a simple proceedural mesh around your rope joints. 
     *
     * @param pointA A gameobject that will contain the rope script. It wall also be the starting point of the rope.
     * @param pointB A gameobject that will contain the ropes ending point.
     * @param pointAKinematic Sets point A to be either Kinematic or free falling.
     * @param pointBKinematic Sets point B to be either Kinematic or free falling.
     * @param worldConstraints Allows you to constrain the created rope to one of 3 axis'.
     * @returns QuickRopeMeshRope
     */
    public static QuickRopeMeshRope MeshRope(GameObject pointA, GameObject pointB, bool pointAKinematic, bool pointBKinematic, RopeConstraint worldConstraints)
    {
        mrObj = pointA.AddComponent<QuickRopeMeshRope>();
        mrObj.pointB = pointB;
        mrObj.pointAKinematic = pointAKinematic;
        mrObj.pointBKinematic = pointBKinematic;
        mrObj.worldConstraints = worldConstraints;
        return mrObj;
    }
    /** Generates a Mesh Rope. The end points will be set to kinematic and the world constraints will be set to none.
     *
     * @param pointA A gameobject that will contain the rope script. It wall also be the starting point of the rope.
     * @param pointB A gameobject that will contain the ropes ending point.
     * @returns QuickRopeMeshRope
     */
    public static QuickRopeMeshRope MeshRope(GameObject pointA, GameObject pointB)
    {
        return QuickRopes.MeshRope(pointA, pointB, true, true, RopeConstraint.None);
    }
    /** Generates a Mesh Rope. The end points will be set to kinematic and the world constraints will be set to none.
     *
     * @param pointA The position that the rope will put the pointA gameobject and script. It wall also be the starting point of the rope.
     * @param pointB The position of the ending point.
     * @returns QuickRopeMeshRope
     */
    public static QuickRopeMeshRope MeshRope(Vector3 pointA, Vector3 pointB)
    {
        pA = new GameObject("MeshRope_A");
        pA.transform.position = pointA;
        pB = new GameObject("MeshRope_B");
        pB.transform.position = pointB;

        return QuickRopes.MeshRope(pA, pB, true, true, RopeConstraint.None);
    }
    #endregion

    #region QuickRopes Line Rope Static Methods
    static QuickRopeLineRope lrObj;
    /** Generates a line Rope. A line rope uses Unitys built in line renderer to generate the rope.
     *
     * @param pointA A gameobject that will contain the rope script. It wall also be the starting point of the rope.
     * @param pointB A gameobject that will contain the ropes ending point.
     * @param pointAKinematic Sets point A to be either Kinematic or free falling.
     * @param pointBKinematic Sets point B to be either Kinematic or free falling.
     * @param worldConstraints Allows you to constrain the created rope to one of 3 axis'.
     * @returns QuickRopeLineRope
     */
    public static QuickRopeLineRope LineRope(GameObject pointA, GameObject pointB, bool pointAKinematic, bool pointBKinematic, RopeConstraint worldConstraints)
    {
        lrObj = pointA.AddComponent<QuickRopeLineRope>();
        lrObj.pointB = pointB;
        lrObj.pointAKinematic = pointAKinematic;
        lrObj.pointBKinematic = pointBKinematic;
        lrObj.worldConstraints = worldConstraints;
        return lrObj;
    }
    /** Generates a Line Rope. The end points will be set to kinematic and the world constraints will be set to none.
     *
     * @param pointA A gameobject that will contain the rope script. It wall also be the starting point of the rope.
     * @param pointB A gameobject that will contain the ropes ending point.
     * @returns QuickRopeLineRope
     */
    public static QuickRopeLineRope LineRope(GameObject pointA, GameObject pointB)
    {
        return QuickRopes.LineRope(pointA, pointB, true, true, RopeConstraint.None);
    }
    /** Generates a Line Rope. The end points will be set to kinematic and the world constraints will be set to none.
     *
     * @param pointA The position that the rope will put the pointA gameobject and script. It wall also be the starting point of the rope.
     * @param pointB The position of the ending point.
     * @returns QuickRopeLineRope
     */
    public static QuickRopeLineRope LineRope(Vector3 pointA, Vector3 pointB)
    {
        pA = new GameObject("LineRope_A");
        pA.transform.position = pointA;
        pB = new GameObject("LineRope_B");
        pB.transform.position = pointB;

        return QuickRopes.LineRope(pA, pB, true, true, RopeConstraint.None);
    }
    #endregion

    #region QuickRopes Spline Mesh Rope Static Methods
    static QuickRopeSplineMeshRope msrObj;
    /** Generates a Spline Mesh Rope. The spline mesh rope is the same as a mesh rope except the mesh follows the points along a spline and
     * allows for the creation of higher detailed meshes without the added physics overhead.
     *
     * @param pointA A gameobject that will contain the rope script. It wall also be the starting point of the rope.
     * @param pointB A gameobject that will contain the ropes ending point.
     * @param pointAKinematic Sets point A to be either Kinematic or free falling.
     * @param pointBKinematic Sets point B to be either Kinematic or free falling.
     * @param worldConstraints Allows you to constrain the created rope to one of 3 axis'.
     * @returns QuickRopeSplineMeshRope
     */
    public static QuickRopeSplineMeshRope SplineMeshRope(GameObject pointA, GameObject pointB, bool pointAKinematic, bool pointBKinematic, RopeConstraint worldConstraints)
    {
        msrObj = pointA.AddComponent<QuickRopeSplineMeshRope>();
        msrObj.pointB = pointB;
        msrObj.pointAKinematic = pointAKinematic;
        msrObj.pointBKinematic = pointBKinematic;
        msrObj.worldConstraints = worldConstraints;
        return msrObj;
    }
    /** Generates a Spline Mesh Rope. The end points will be set to kinematic and the world constraints will be set to none.
     *
     * @param pointA A gameobject that will contain the rope script. It wall also be the starting point of the rope.
     * @param pointB A gameobject that will contain the ropes ending point.
     * @returns QuickRopeSplineMeshRope
     */
    public static QuickRopeSplineMeshRope SplineMeshRope(GameObject pointA, GameObject pointB)
    {
        return QuickRopes.SplineMeshRope(pointA, pointB, true, true, RopeConstraint.None);
    }
    /** Generates a Spline Mesh Rope. The end points will be set to kinematic and the world constraints will be set to none.
     *
     * @param pointA The position that the rope will put the pointA gameobject and script. It wall also be the starting point of the rope.
     * @param pointB The position of the ending point.
     * @returns QuickRopeSplineMeshRope
     */
    public static QuickRopeSplineMeshRope SplineMeshRope(Vector3 pointA, Vector3 pointB)
    {
        pA = new GameObject("LineRope_A");
        pA.transform.position = pointA;
        pB = new GameObject("LineRope_B");
        pB.transform.position = pointB;

        return QuickRopes.SplineMeshRope(pA, pB, true, true, RopeConstraint.None);
    }
    #endregion

    #region QuickRopes Spline Line Rope Static Methods
    static QuickRopeSplineLineRope slrObj;
    /** Generates a Spline Line Rope. The spline line rope is the same as a line rope except the mesh follows the points along a spline and
     * allows for the creation of higher detailed meshes without the added physics overhead.
     *
     * @param pointA A gameobject that will contain the rope script. It wall also be the starting point of the rope.
     * @param pointB A gameobject that will contain the ropes ending point.
     * @param pointAKinematic Sets point A to be either Kinematic or free falling.
     * @param pointBKinematic Sets point B to be either Kinematic or free falling.
     * @param worldConstraints Allows you to constrain the created rope to one of 3 axis'.
     * @returns QuickRopeSplineLineRope
     */
    public static QuickRopeSplineLineRope SplineLineRope(GameObject pointA, GameObject pointB, bool pointAKinematic, bool pointBKinematic, RopeConstraint worldConstraints)
    {
        slrObj = pointA.AddComponent<QuickRopeSplineLineRope>();
        slrObj.pointB = pointB;
        slrObj.pointAKinematic = pointAKinematic;
        slrObj.pointBKinematic = pointBKinematic;
        slrObj.worldConstraints = worldConstraints;
        return slrObj;
    }
    /** Generates a Spline Line Rope. The end points will be set to kinematic and the world constraints will be set to none.
     *
     * @param pointA A gameobject that will contain the rope script. It wall also be the starting point of the rope.
     * @param pointB A gameobject that will contain the ropes ending point.
     * @returns QuickRopeSplineLineRope
     */
    public static QuickRopeSplineLineRope SplineLineRope(GameObject pointA, GameObject pointB)
    {
        return QuickRopes.SplineLineRope(pointA, pointB, true, true, RopeConstraint.None);
    }
    /** Generates a Spline Line Rope. The end points will be set to kinematic and the world constraints will be set to none.
     *
     * @param pointA The position that the rope will put the pointA gameobject and script. It wall also be the starting point of the rope.
     * @param pointB The position of the ending point.
     * @returns QuickRopeSplineLineRope
     */
    public static QuickRopeSplineLineRope SplineLineRope(Vector3 pointA, Vector3 pointB)
    {
        pA = new GameObject("LineRope_A");
        pA.transform.position = pointA;
        pB = new GameObject("LineRope_B");
        pB.transform.position = pointB;

        return QuickRopes.SplineLineRope(pA, pB, true, true, RopeConstraint.None);
    }
    #endregion

    /** Destroys specified rope object by calling the ropes destroy method. 
     *
     * @param rope The rope you want to be destroyed.
     */
    public static void Destroy(QuickRopeBASE rope)
    {
        rope.Destroy();
    }
}

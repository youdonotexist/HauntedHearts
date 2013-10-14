using UnityEngine;
using System.Collections;

/** \brief The most basic rope type.
 * 
 * This is the most basic rope type you can have. This rope does not build a mesh and you will not be able to see it during gameplay. 
 * A use for this would be for anything involving rope physics without the use of a rope or possibly ghostly effects.
 */
public class QuickRopeInvisible : QuickRopeBASE 
{
    void OnDrawGizmos()
    {
        UpdateGizmos();
    }

    void Start()
    {
        InitializeRope();
    }

    static QuickRopeInvisible rObj;
    /** This static function allows you to create a new rope of this type with a single line of code!
     *
     * @param pointA The first gameObject of your rope.
     * @param pointB The ending gameObject of your rope.
     * @param pointAIsKinimatic Is the first point of your rope Kinematic? (Non-Physical)
     * @param pointBIsKinimatic Is the ending point of your rope Kinematic? (Non-Physical)
     * @param worldConstraints Is this rope bound to any particular plane via a RopeConstraint?
     * 
     * @return QuickRopeInvisible
     */
    public static QuickRopeInvisible NewRope(GameObject pointA, GameObject pointB, bool pointAIsKinimatic, bool pointBIsKinimatic, RopeConstraint worldConstraints)
    {
        rObj = pointA.AddComponent<QuickRopeInvisible>();
        rObj.pointB = pointB;
        rObj.pointAKinematic = pointAIsKinimatic;
        rObj.pointBKinematic = pointBIsKinimatic;
        rObj.worldConstraints = worldConstraints;

        return rObj;
    }
    /** This static function allows you to create a new rope of this type with a single line of code!
     *
     * @param pointA The first gameObject of your rope.
     * @param pointB The ending gameObject of your rope.
     * 
     * @return QuickRopeInvisible
     */
    public static QuickRopeInvisible NewRope(GameObject pointA, GameObject pointB)
    {
        return QuickRopeInvisible.NewRope(pointA, pointB, true, true, RopeConstraint.None);
    }
}

using UnityEngine;

public class LayerMaskHelper {
	
	public static int OnlyIncluding( params int[] layers ){
		return MakeMask( layers );
	}
	
	public static int EverythingBut( params int[] layers ){
		return ~MakeMask( layers );
	}
	
	public static int MakeMask( params int[] layers ){
		int mask = 0;
		foreach ( int item in layers ) {
			mask |= 1 << item;
		}
		return mask;	
	}
	
	public static bool IsInLayerMask(GameObject obj, LayerMask layerMask)
	{
	    // Convert the object's layer to a bitfield for comparison
	    int objLayerMask = (1 << obj.layer);
	    if ((layerMask.value & objLayerMask) > 0)  // Extra round brackets required!
	        return true;
	    else
	        return false;
	}
	
}

Shader "Transparent/VertexLitCutoutAndFade" {
	Properties 
	{
		_Color ("Main Color", Color) = (1,1,1,1)
		_MainTex ("Texture", 2D) = "white" {}
		_Cutoff ("Alpha cutoff", Range(0,1)) = 0.5
	}
	
	SubShader 
	{
		Cull Off
		Tags 
		{
			"Queue" = "Transparent" 
			"IgnoreProjector" = "True" 
			"RenderType" = "TransparentCutoff"
		}
		Pass
		{
			Blend One One
			Alphatest Greater [_Cutoff]
			AlphaToMask True
			ColorMask RGB
			ZWrite Off
		
	
			SetTexture [_MainTex] 
			{
				Combine texture, texture
			}
			SetTexture [_MainTex] {
	            constantColor [_Color]
	            Combine previous * constant, previous * constant
	        }  
		}
	}
}


Shader "Shader Forge/ShaderForge_CustomCharacter" {
	Properties {
		_Diffuse ("Diffuse", 2D) = "white" {}
		_Color ("Color", Vector) = (0.5,0.5,0.5,1)
		_SpecColor ("Spec Color", Vector) = (1,1,1,1)
		_Posterize ("Posterize", Float) = 2
		_StunEffect ("StunEffect", Range(0, 1)) = 1
		[HDR] _node_8733 ("node_8733", Vector) = (0.4980392,0.4980392,0.4980392,1)
		_ambatten ("amb atten", Range(0, 1)) = 1
		_LowStamina ("LowStamina", Range(0, 1)) = 1
		[HDR] _node_8733_copy ("node_8733_copy", Vector) = (1,0.4074074,0,1)
		_OutlineWidth ("Outline Width", Range(0, 0.1)) = 0.1
	}
	//DummyShaderTextExporter
	SubShader{
		Tags { "RenderType"="Opaque" }
		LOD 200
		CGPROGRAM
#pragma surface surf Standard
#pragma target 3.0

		fixed4 _Color;
		struct Input
		{
			float2 uv_MainTex;
		};
		
		void surf(Input IN, inout SurfaceOutputStandard o)
		{
			o.Albedo = _Color.rgb;
			o.Alpha = _Color.a;
		}
		ENDCG
	}
	Fallback "Diffuse"
	//CustomEditor "ShaderForgeMaterialInspector"
}
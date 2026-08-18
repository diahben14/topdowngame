Shader "Shader Forge/dashbolt shader" {
	Properties {
		[HDR] _Color ("Color", Vector) = (0.8342488,0.1544589,0.8396226,1)
		_dotproductcutoff ("dot product cutoff", Range(-0.1, 1)) = -0.1
		[HDR] _Color_alt ("Color_alt", Vector) = (0.2877358,0.5367516,1,1)
		[HideInInspector] _Cutoff ("Alpha cutoff", Range(0, 1)) = 0.5
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
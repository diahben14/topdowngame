Shader "Custom/EdgeEffect" {
	Properties {
		_EdgeColor ("EdgeColor", Vector) = (1,1,1,1)
		_EdgeColorScale ("EdgeColorScale", Float) = 1
		_InsideColor ("InsideColor", Vector) = (1,1,1,1)
		_InsideColorScale ("InsideColorScale", Float) = 1
		_Glossiness ("Smoothness", Range(0, 1)) = 0.5
		_Metallic ("Metallic", Range(0, 1)) = 0
	}
	//DummyShaderTextExporter
	SubShader{
		Tags { "RenderType" = "Opaque" }
		LOD 200
		CGPROGRAM
#pragma surface surf Standard
#pragma target 3.0

		struct Input
		{
			float2 uv_MainTex;
		};

		void surf(Input IN, inout SurfaceOutputStandard o)
		{
			o.Albedo = 1;
		}
		ENDCG
	}
	Fallback "Diffuse"
}
Shader "Custom/WaterEdgeSubtle" {
	Properties {
		_MainColor ("MainColor", Vector) = (1,1,1,1)
		_ColourScale ("Colour Scale", Float) = 1
		_Glossiness ("Smoothness", Range(0, 1)) = 0.5
		_Metallic ("Metallic", Range(0, 1)) = 0
		_Speed ("Speed", Float) = 1
		_WorldSpeedOffset ("WorldSpeedOffset", Float) = 1
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
Shader "Shader Forge/d20" {
	Properties {
		[HDR] _Color ("Color", Vector) = (0.5019608,0.5019608,0.5019608,1)
		_Metallic ("Metallic", Range(0, 1)) = 0
		_Gloss ("Gloss", Range(0, 1)) = 0.8
		_scale ("scale", Float) = 1
		_mainTex ("mainTex", 2D) = "white" {}
		_marbling ("marbling", Float) = 1
		_Opacity ("Opacity", Range(0, 1)) = 0.6495732
		_rotatespeed ("rotatespeed", Float) = 1
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
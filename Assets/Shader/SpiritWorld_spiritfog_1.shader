Shader "SpiritWorld/spiritfog_1" {
	Properties {
		[HDR] _fogcolour ("fog colour", Vector) = (0.5,0.5,0.5,1)
		_fogdensity ("fog density", Float) = 0
		_colour_density ("colour_density", 2D) = "white" {}
		_scrollspeed ("scrollspeed", Vector) = (0,0,0,0)
		_closefade ("close fade", Float) = 16
		_texScale ("texScale", Vector) = (1,1,1,1)
		[HDR] _colouradd ("colour add", Vector) = (0,0,0,1)
		_multvsmin ("mult vs min", Range(0, 1)) = 0
		_fresnelfade ("fresnel fade", Range(0, 4)) = 2.065127
		_wavespeed ("wavespeed", Vector) = (1,1,0,0)
		[HideInInspector] _Cutoff ("Alpha cutoff", Range(0, 1)) = 0.5
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
	//CustomEditor "ShaderForgeMaterialInspector"
}
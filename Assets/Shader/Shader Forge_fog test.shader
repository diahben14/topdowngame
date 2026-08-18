Shader "Shader Forge/fog test" {
	Properties {
		_fogcolour ("fog colour", Vector) = (0.5,0.5,0.5,1)
		_fogdensity ("fog density", Float) = 0
		_colour_density ("colour_density", 2D) = "white" {}
		_scrollspeed ("scrollspeed", Vector) = (0,0,0,0)
		_closefade ("close fade", Float) = 16
		_texScale ("texScale", Vector) = (1,1,1,1)
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
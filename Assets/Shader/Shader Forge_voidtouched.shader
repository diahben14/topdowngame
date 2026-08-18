Shader "Shader Forge/voidtouched" {
	Properties {
		_outlinewidth ("outline width", Range(0, 0.2)) = 0
		_edgetexture ("edge texture", 2D) = "white" {}
		_outlineboost ("outline boost", Range(0, 16)) = 3.418812
		_outlinecolour ("outline colour", Vector) = (0.5,0.5,0.5,1)
		_interior ("interior", 2D) = "white" {}
		_dotPower ("dotPower", Float) = 8
		_interiorScale ("interiorScale", Float) = 64
		_outlinespeed ("outline speed", Range(0, 1)) = 0.09825268
		_vertexoffset ("vertex offset", Range(0, 4)) = 0
		_cutoff ("cutoff", Range(-0.5, 0.5)) = 0.5
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
	Fallback "Diffuse"
	//CustomEditor "ShaderForgeMaterialInspector"
}
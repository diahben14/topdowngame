Shader "Unlit/VoidShader" {
	Properties {
		_SparkleTex ("Sparkle Texture", 2D) = "white" {}
		_VoidTex ("Void Texture", 2D) = "white" {}
		_BrightnessScale ("Brightness Scale", Float) = 8
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
}
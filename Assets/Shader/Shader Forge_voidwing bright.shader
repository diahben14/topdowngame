Shader "Shader Forge/voidwing bright" {
	Properties {
		_CutoffTexture ("Cutoff Texture", 2D) = "white" {}
		_CutoffNoise ("Cutoff Noise", 2D) = "white" {}
		_CutoffNoiseScrollSpeed ("Cutoff Noise Scroll Speed", Float) = 0.1
		_CutoffTexureNoiseAmount ("Cutoff Texure Noise Amount", Float) = 0.2
		_Appearance ("Appearance", Range(0, 1)) = 0.4488755
		_WingTex ("Wing Tex", 2D) = "white" {}
		_Colour ("Colour", Vector) = (0.5,0.5,0.5,1)
		_ColourScale ("Colour Scale", Float) = 1
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
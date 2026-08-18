Shader "Shader Forge/iceshader" {
	Properties {
		_Albedo ("Albedo", Vector) = (0.2874612,0.5237306,0.76,1)
		_Smoothness ("Smoothness", Range(0, 1)) = 0.2691369
		_Specular ("Specular", Vector) = (0.2661644,0.2780822,0.29,1)
		_rimPower ("rimPower", Float) = 4
		_rimcolour ("rim colour", Vector) = (0.5,0.5,0.5,1)
		_noise ("noise", 2D) = "white" {}
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
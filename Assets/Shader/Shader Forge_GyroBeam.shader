Shader "Shader Forge/GyroBeam" {
	Properties {
		_Tex ("Tex", 2D) = "white" {}
		[HDR] _TintColor ("Emission", Vector) = (0.5,0.5,0.5,1)
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
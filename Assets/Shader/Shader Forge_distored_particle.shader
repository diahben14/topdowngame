Shader "Shader Forge/distored_particle" {
	Properties {
		[HDR] _Color ("Color", Vector) = (0.07843138,0.3921569,0.7843137,1)
		_caustic ("caustic", 2D) = "white" {}
		_distortion_tex ("distortion_tex", 2D) = "white" {}
		_distortion ("distortion", Range(0, 1)) = 0.465499
		_ramp ("ramp", 2D) = "white" {}
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
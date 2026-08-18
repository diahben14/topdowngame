Shader "Shader Forge/RampFX" {
	Properties {
		[HDR] _Color ("Color", Vector) = (1,1,1,1)
		_texture ("texture", 2D) = "white" {}
		_ramp ("ramp", 2D) = "white" {}
		_rampprogress ("ramp progress", Range(0, 1)) = 0.3923272
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
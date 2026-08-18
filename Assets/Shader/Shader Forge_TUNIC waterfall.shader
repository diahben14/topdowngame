Shader "Shader Forge/TUNIC waterfall" {
	Properties {
		[HDR] _Color ("Color", Vector) = (0.06444601,0.4008805,0.934,0.4901961)
		_ramp ("ramp", 2D) = "white" {}
		_waterdetph ("water detph", Float) = 4
		_speed ("speed", Float) = 1
		_foam ("foam", 2D) = "white" {}
		_foamspeed ("foam speed", Vector) = (0,0,0,0)
		_foamthreshold ("foam threshold", Range(0, 2)) = 0
		_foamdepth ("foam depth", Range(0, 1)) = 0
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
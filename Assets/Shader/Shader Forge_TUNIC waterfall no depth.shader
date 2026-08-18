Shader "Shader Forge/TUNIC waterfall no depth" {
	Properties {
		[HDR] _Color ("Color", Vector) = (0.06444601,0.4008805,0.934,0.4901961)
		_Specular ("Specular", Vector) = (0.5,0.5,0.5,1)
		_Color_Foam ("Color_Foam", Vector) = (0.5,0.5,0.5,1)
		_foam ("foam", 2D) = "white" {}
		_foamspeed ("foam speed", Vector) = (0,0,0,0)
		_foamthreshold ("foam threshold", Range(0, 2)) = 0.5616726
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
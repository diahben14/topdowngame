Shader "Shader Forge/gyro shadow" {
	Properties {
		_Diffuse ("Diffuse", 2D) = "white" {}
		_Color ("Color", Vector) = (0.5,0.5,0.5,1)
		_Gloss ("Gloss", Range(0, 1)) = 0.5
		_SpecColor ("Spec Color", Vector) = (1,1,1,1)
		[HDR] _emissioncolour ("emission colour", Vector) = (0,0,0,1)
		_emissiontex ("emission tex", 2D) = "black" {}
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
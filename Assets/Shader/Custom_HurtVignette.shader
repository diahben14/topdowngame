Shader "Custom/HurtVignette" {
	Properties {
		_HurtColor ("Hurt Color", Vector) = (1,1,1,1)
		_StaminaColor ("Stamina Color", Vector) = (1,1,1,1)
		_HurtShow ("Show", Float) = 1
		_StaminaShow ("Stamina Show", Float) = 1
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
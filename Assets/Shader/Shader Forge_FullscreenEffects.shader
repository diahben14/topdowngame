Shader "Shader Forge/FullscreenEffects" {
	Properties {
		_grad_UR ("grad_UR", Vector) = (1,0.6667008,0,1)
		_grad_LL ("grad_LL", Vector) = (0.122597,0.3293381,0.8962264,1)
		_letterboxParam ("letterboxParam", Range(0, 1)) = 1
		_hurtParam ("hurtParam", Range(0, 1)) = 0
		_radiationParam ("radiationParam", Range(0, 1)) = 0
		_ghostParam ("ghostParam", Range(0, 1)) = 0
		_MainTex ("MainTex", 2D) = "white" {}
		_letterboxDensity ("letterboxDensity", Float) = 4
		_letterboxWidthFactor ("letterboxWidthFactor", Float) = 8
		[HDR] _hurtcolour ("hurt colour", Vector) = (0.5,0.5,0.5,1)
		_hurtpower ("hurt power", Float) = 2
		_radiationpower ("radiation power", Float) = 2
		_whiteout ("whiteout", Range(0, 1)) = 0
		_blackout ("blackout", Range(0, 1)) = 0
		_maxGhostAmount ("maxGhostAmount", Float) = 0.5
		_flip ("flip", Float) = 0
		[HDR] _radiationtint ("radiation tint", Vector) = (0.5535364,0,1,1)
		[HideInInspector] _Cutoff ("Alpha cutoff", Range(0, 1)) = 0.5
	}
	//DummyShaderTextExporter
	SubShader{
		Tags { "RenderType"="Opaque" }
		LOD 200
		CGPROGRAM
#pragma surface surf Standard
#pragma target 3.0

		sampler2D _MainTex;
		struct Input
		{
			float2 uv_MainTex;
		};

		void surf(Input IN, inout SurfaceOutputStandard o)
		{
			fixed4 c = tex2D(_MainTex, IN.uv_MainTex);
			o.Albedo = c.rgb;
			o.Alpha = c.a;
		}
		ENDCG
	}
	//CustomEditor "ShaderForgeMaterialInspector"
}
Shader "Shader Forge/2019 obsidian" {
	Properties {
		_MainTex ("Base Color", 2D) = "white" {}
		_Color ("Color", Vector) = (0.5019608,0.5019608,0.5019608,1)
		_specular ("specular", 2D) = "white" {}
		_specularscale ("specular scale", Range(0, 1)) = 0
		_Gloss ("Gloss", Range(0, 1)) = 0.9115284
		_matcap ("matcap", 2D) = "black" {}
		_matcapamount ("matcap amount", Range(0, 1)) = 0.4273504
		_matcap_worldScale ("matcap_worldScale", Float) = 0.125
		_matcap_screenScale ("matcap_screenScale", Float) = 0.1
		_matcap_screenNormalScale ("matcap_screenNormalScale", Float) = 0
		_pattern ("pattern", 2D) = "black" {}
		[HDR] _glow_colour ("glow_colour", Vector) = (0.1254895,0.5243154,0.5660378,1)
		_glow ("glow", Range(0, 1)) = 0.6595793
		_glow_exponent ("glow_exponent", Range(0, 16)) = 0
		_glow_noise ("glow_noise", 2D) = "white" {}
		_glow_ramp ("glow_ramp", 2D) = "white" {}
		_BumpMap ("Normal Map", 2D) = "bump" {}
		_playerGlowBoost ("playerGlowBoost", Range(0, 8)) = 0
		_playerPos ("playerPos", Vector) = (0,0,0,0)
		_scrollspeed ("scrollspeed", Float) = 1
	}
	//DummyShaderTextExporter
	SubShader{
		Tags { "RenderType"="Opaque" }
		LOD 200
		CGPROGRAM
#pragma surface surf Standard
#pragma target 3.0

		sampler2D _MainTex;
		fixed4 _Color;
		struct Input
		{
			float2 uv_MainTex;
		};
		
		void surf(Input IN, inout SurfaceOutputStandard o)
		{
			fixed4 c = tex2D(_MainTex, IN.uv_MainTex) * _Color;
			o.Albedo = c.rgb;
			o.Alpha = c.a;
		}
		ENDCG
	}
	Fallback "Diffuse"
	//CustomEditor "ShaderForgeMaterialInspector"
}
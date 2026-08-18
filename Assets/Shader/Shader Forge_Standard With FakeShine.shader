Shader "Shader Forge/Standard With FakeShine" {
	Properties {
		_Color ("Color", Vector) = (0.5019608,0.5019608,0.5019608,1)
		_MainTex ("Base Albedo/Shinemap", 2D) = "white" {}
		_Specular ("Specular", 2D) = "black" {}
		_BumpMap ("Normal Map", 2D) = "bump" {}
		_Detail ("Detail", 2D) = "gray" {}
		_MossAlbedo ("Moss Albedo", 2D) = "white" {}
		_SpecularMoss ("Specular Moss", 2D) = "black" {}
		_MossNormalMap ("Moss Normal Map", 2D) = "bump" {}
		_MossColor ("Moss Color", Vector) = (0.5,0.5,0.5,1)
		_upnormalmoss ("up normal moss", Range(0, 2)) = 0
		_colourasnormal ("colour as normal", Range(0, 1)) = 1
		_paintedmoss ("painted moss", Range(0, 2)) = 0
		_shine_tex ("shine_tex", 2D) = "black" {}
		_shine_intensity ("shine_intensity", Float) = 0
		_matcap_worldScale ("matcap_worldScale", Float) = 0.125
		_matcap_screenScale ("matcap_screenScale", Float) = 0.1
		_matcap_screenNormalScale ("matcap_screenNormalScale", Float) = 0
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
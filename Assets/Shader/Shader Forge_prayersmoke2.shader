Shader "Shader Forge/prayersmoke2" {
	Properties {
		_MainTex ("MainTex", 2D) = "white" {}
		[HDR] _TintColor ("Color", Vector) = (0.5,0.5,0.5,1)
		_Color_interior ("Color_interior", Vector) = (0.5,0.5,0.5,1)
		_scrollspeeds ("scrollspeeds", Vector) = (1,1,1,1)
		_normaloffset ("normal offset", Float) = 0.1
		_min ("min", Range(0, 1)) = 0
		_max ("max", Range(0, 1)) = 1
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
Shader "Custom/DappleCookie" {
	Properties {
		_MainTex ("Texture", 2D) = "white" {}
		_EdgeWrap ("Edge Wrap Texture", 2D) = "white" {}
		_Offsets ("Offsets", Vector) = (0,0,0,0)
		_Scales ("Scales", Vector) = (1,1,1,1)
		_Add ("Add", Float) = 0
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
}
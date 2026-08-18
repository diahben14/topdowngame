Shader "Shader Forge/omnifuse glow beams" {
	Properties {
		[HDR] _TintColor ("Color", Vector) = (0.5,0.5,0.5,1)
		_causticscales ("caustic scales", Vector) = (0.33,0.33,0.22,0.22)
		_causticscrollspeeds ("caustic scrollspeeds", Vector) = (0.02,0.02,0.05,-0.03)
		_node_6366 ("node_6366", 2D) = "white" {}
		_node_1157 ("node_1157", 2D) = "white" {}
		_heightclip ("heightclip", Float) = 0
		_heightclipwidth ("heightclip width", Float) = 1
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
	//CustomEditor "ShaderForgeMaterialInspector"
}
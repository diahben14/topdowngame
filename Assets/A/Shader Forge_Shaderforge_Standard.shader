Shader "Shader Forge/Shaderforge_Standard" {
	Properties {
		_Color ("Color", Color) = (0.5019608,0.5019608,0.5019608,1)
		_MainTex ("Base Albedo", 2D) = "white" {}
		_Specular ("Specular", 2D) = "black" {}
		_BumpMap ("Normal Map", 2D) = "bump" {}
		_Detail ("Detail", 2D) = "gray" {}
		_MossColor ("Moss Color", Color) = (0.5,0.5,0.5,1)
		_MossAlbedo ("Moss Albedo", 2D) = "white" {}
		_SpecularMoss ("Specular Moss", 2D) = "black" {}
		_MossNormalMap ("Moss Normal Map", 2D) = "bump" {}
		_upnormalmoss ("up normal moss", Range(0, 2)) = 0
		_colourasnormal ("colour as normal", Range(0, 1)) = 1
		_paintedmoss ("painted moss", Range(0, 2)) = 0
	}
	SubShader {
		Tags { "RenderType" = "Opaque" }
		Pass {
			Name "FORWARD"
			Tags { "LIGHTMODE" = "FORWARDBASE" "RenderType" = "Opaque" "SHADOWSUPPORT" = "true" }
			GpuProgramID 35319
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			
			#include "UnityCG.cginc"
			struct v2f
			{
				float4 position : SV_POSITION0;
				float2 texcoord : TEXCOORD0;
				float2 texcoord1 : TEXCOORD1;
				float2 texcoord2 : TEXCOORD2;
				float4 texcoord3 : TEXCOORD3;
				float3 texcoord4 : TEXCOORD4;
				float3 texcoord5 : TEXCOORD5;
				float3 texcoord6 : TEXCOORD6;
				float4 color : COLOR0;
				float4 texcoord10 : TEXCOORD10;
			};
			struct fout
			{
				float4 sv_target : SV_Target0;
			};
			// $Globals ConstantBuffers for Vertex Shader
			// $Globals ConstantBuffers for Fragment Shader
			float4 _LightColor0;
			float4 _Color;
			float4 _MainTex_ST;
			float4 _BumpMap_ST;
			float4 _Specular_ST;
			float4 _Detail_ST;
			float4 _MossAlbedo_ST;
			float4 _MossNormalMap_ST;
			float4 _MossColor;
			float4 _SpecularMoss_ST;
			float _upnormalmoss;
			float _colourasnormal;
			float _paintedmoss;
			// Custom ConstantBuffers for Vertex Shader
			// Custom ConstantBuffers for Fragment Shader
			// Texture params for Vertex Shader
			// Texture params for Fragment Shader
			sampler2D _BumpMap;
			sampler2D _MossNormalMap;
			sampler2D _MainTex;
			sampler2D _Specular;
			sampler2D _SpecularMoss;
			sampler2D _MossAlbedo;
			sampler2D _Detail;
			
			// Keywords: DIRECTIONAL LIGHTMAP_OFF DIRLIGHTMAP_OFF DYNAMICLIGHTMAP_OFF
			v2f vert(appdata_full v)
			{
                v2f o;
                float4 tmp0;
                float4 tmp1;
                float4 tmp2;
                tmp0 = v.vertex.yyyy * unity_ObjectToWorld._m01_m11_m21_m31;
                tmp0 = unity_ObjectToWorld._m00_m10_m20_m30 * v.vertex.xxxx + tmp0;
                tmp0 = unity_ObjectToWorld._m02_m12_m22_m32 * v.vertex.zzzz + tmp0;
                tmp1 = tmp0 + unity_ObjectToWorld._m03_m13_m23_m33;
                o.texcoord3 = unity_ObjectToWorld._m03_m13_m23_m33 * v.vertex.wwww + tmp0;
                tmp0 = tmp1.yyyy * unity_MatrixVP._m01_m11_m21_m31;
                tmp0 = unity_MatrixVP._m00_m10_m20_m30 * tmp1.xxxx + tmp0;
                tmp0 = unity_MatrixVP._m02_m12_m22_m32 * tmp1.zzzz + tmp0;
                o.position = unity_MatrixVP._m03_m13_m23_m33 * tmp1.wwww + tmp0;
                o.texcoord.xy = v.texcoord.xy;
                o.texcoord1.xy = v.texcoord1.xy;
                o.texcoord2.xy = v.texcoord2.xy;
                tmp0.x = dot(v.normal.xyz, unity_WorldToObject._m00_m10_m20);
                tmp0.y = dot(v.normal.xyz, unity_WorldToObject._m01_m11_m21);
                tmp0.z = dot(v.normal.xyz, unity_WorldToObject._m02_m12_m22);
                tmp0.w = dot(tmp0.xyz, tmp0.xyz);
                tmp0.w = rsqrt(tmp0.w);
                tmp0.xyz = tmp0.www * tmp0.xyz;
                o.texcoord4.xyz = tmp0.xyz;
                tmp1.xyz = v.tangent.yyy * unity_ObjectToWorld._m01_m11_m21;
                tmp1.xyz = unity_ObjectToWorld._m00_m10_m20 * v.tangent.xxx + tmp1.xyz;
                tmp1.xyz = unity_ObjectToWorld._m02_m12_m22 * v.tangent.zzz + tmp1.xyz;
                tmp0.w = dot(tmp1.xyz, tmp1.xyz);
                tmp0.w = rsqrt(tmp0.w);
                tmp1.xyz = tmp0.www * tmp1.xyz;
                o.texcoord5.xyz = tmp1.xyz;
                tmp2.xyz = tmp0.zxy * tmp1.yzx;
                tmp0.xyz = tmp0.yzx * tmp1.zxy + -tmp2.xyz;
                tmp0.xyz = tmp0.xyz * v.tangent.www;
                tmp0.w = dot(tmp0.xyz, tmp0.xyz);
                tmp0.w = rsqrt(tmp0.w);
                o.texcoord6.xyz = tmp0.www * tmp0.xyz;
                o.color = v.color;
                o.texcoord10 = float4(0.0, 0.0, 0.0, 0.0);
                return o;
			}
			// Keywords: DIRECTIONAL LIGHTMAP_OFF DIRLIGHTMAP_OFF DYNAMICLIGHTMAP_OFF
			fout frag(v2f inp)
			{
                fout o;
                float4 tmp0;
                float4 tmp1;
                float4 tmp2;
                float4 tmp3;
                float4 tmp4;
                float4 tmp5;
                float4 tmp6;
                float4 tmp7;
                float4 tmp8;
                float4 tmp9;
                float4 tmp10;
                float4 tmp11;
                float4 tmp12;
                float4 tmp13;
                float4 tmp14;
                tmp0.x = dot(inp.texcoord4.xyz, inp.texcoord4.xyz);
                tmp0.x = rsqrt(tmp0.x);
                tmp0.yzw = tmp0.xxx * inp.texcoord4.xyz;
                tmp1.xyz = _WorldSpaceCameraPos - inp.texcoord3.xyz;
                tmp1.w = dot(tmp1.xyz, tmp1.xyz);
                tmp1.w = rsqrt(tmp1.w);
                tmp2.xyz = tmp1.www * tmp1.xyz;
                tmp3.xy = inp.texcoord.xy * _BumpMap_ST.xy + _BumpMap_ST.zw;
                tmp3 = tex2D(_BumpMap, tmp3.xy);
                tmp3.x = tmp3.w * tmp3.x;
                tmp3.xy = tmp3.xy * float2(2.0, 2.0) + float2(-1.0, -1.0);
                tmp2.w = dot(tmp3.xy, tmp3.xy);
                tmp2.w = min(tmp2.w, 1.0);
                tmp2.w = 1.0 - tmp2.w;
                tmp3.z = sqrt(tmp2.w);
                tmp4.xy = inp.texcoord.xy * _MossNormalMap_ST.xy + _MossNormalMap_ST.zw;
                tmp4 = tex2D(_MossNormalMap, tmp4.xy);
                tmp4.x = tmp4.w * tmp4.x;
                tmp4.xy = tmp4.xy * float2(2.0, 2.0) + float2(-1.0, -1.0);
                tmp2.w = dot(tmp4.xy, tmp4.xy);
                tmp2.w = min(tmp2.w, 1.0);
                tmp2.w = 1.0 - tmp2.w;
                tmp4.z = sqrt(tmp2.w);
                tmp5.xy = inp.texcoord.xy * _MainTex_ST.xy + _MainTex_ST.zw;
                tmp5 = tex2D(_MainTex, tmp5.xy);
                tmp2.w = 1.0 - inp.color.x;
                tmp6.xyz = inp.color.xyz * float3(2.0, 2.0, 2.0) + float3(-1.0, -1.0, -1.0);
                tmp3.w = tmp6.y * unity_ObjectToWorld._m11;
                tmp3.w = unity_ObjectToWorld._m10 * tmp6.x + tmp3.w;
                tmp3.w = unity_ObjectToWorld._m12 * tmp6.z + tmp3.w;
                tmp0.x = -inp.texcoord4.y * tmp0.x + tmp3.w;
                tmp0.x = saturate(_colourasnormal * tmp0.x + tmp0.z);
                tmp0.x = tmp0.x * _upnormalmoss;
                tmp0.x = tmp2.w * _paintedmoss + tmp0.x;
                tmp0.x = tmp0.x >= tmp5.w;
                tmp0.x = tmp0.x ? 1.0 : 0.0;
                tmp4.xyz = tmp4.xyz - tmp3.xyz;
                tmp3.xyz = tmp0.xxx * tmp4.xyz + tmp3.xyz;
                tmp4.xyz = tmp3.yyy * inp.texcoord6.xyz;
                tmp3.xyw = tmp3.xxx * inp.texcoord5.xyz + tmp4.xyz;
                tmp0.yzw = tmp3.zzz * tmp0.yzw + tmp3.xyw;
                tmp2.w = dot(tmp0.xyz, tmp0.xyz);
                tmp2.w = rsqrt(tmp2.w);
                tmp0.yzw = tmp0.yzw * tmp2.www;
                tmp2.w = dot(-tmp2.xyz, tmp0.xyz);
                tmp2.w = tmp2.w + tmp2.w;
                tmp3.xyz = tmp0.yzw * -tmp2.www + -tmp2.xyz;
                tmp2.w = dot(_WorldSpaceLightPos0.xyz, _WorldSpaceLightPos0.xyz);
                tmp2.w = rsqrt(tmp2.w);
                tmp4.xyz = tmp2.www * _WorldSpaceLightPos0.xyz;
                tmp1.xyz = tmp1.xyz * tmp1.www + tmp4.xyz;
                tmp1.w = dot(tmp1.xyz, tmp1.xyz);
                tmp1.w = rsqrt(tmp1.w);
                tmp1.xyz = tmp1.www * tmp1.xyz;
                tmp1.w = unity_ProbeVolumeParams.x == 1.0;
                if (tmp1.w) {
                    tmp1.w = unity_ProbeVolumeParams.y == 1.0;
                    tmp6.xyz = inp.texcoord3.yyy * unity_ProbeVolumeWorldToObject._m01_m11_m21;
                    tmp6.xyz = unity_ProbeVolumeWorldToObject._m00_m10_m20 * inp.texcoord3.xxx + tmp6.xyz;
                    tmp6.xyz = unity_ProbeVolumeWorldToObject._m02_m12_m22 * inp.texcoord3.zzz + tmp6.xyz;
                    tmp6.xyz = tmp6.xyz + unity_ProbeVolumeWorldToObject._m03_m13_m23;
                    tmp6.xyz = tmp1.www ? tmp6.xyz : inp.texcoord3.xyz;
                    tmp6.xyz = tmp6.xyz - unity_ProbeVolumeMin;
                    tmp6.yzw = tmp6.xyz * unity_ProbeVolumeSizeInv;
                    tmp1.w = tmp6.y * 0.25 + 0.75;
                    tmp2.w = unity_ProbeVolumeParams.z * 0.5 + 0.75;
                    tmp6.x = max(tmp1.w, tmp2.w);
                    tmp6 = UNITY_SAMPLE_TEX3D_SAMPLER(unity_ProbeVolumeSH, unity_ProbeVolumeSH, tmp6.xzw);
                } else {
                    tmp6 = float4(1.0, 1.0, 1.0, 1.0);
                }
                tmp1.w = saturate(dot(tmp6, unity_OcclusionMaskSelector));
                tmp6.xyz = tmp1.www * _LightColor0.xyz;
                tmp7.xy = inp.texcoord.xy * _Specular_ST.xy + _Specular_ST.zw;
                tmp7 = tex2D(_Specular, tmp7.xy);
                tmp8.xy = inp.texcoord.xy * _SpecularMoss_ST.xy + _SpecularMoss_ST.zw;
                tmp8 = tex2D(_SpecularMoss, tmp8.xy);
                tmp8 = tmp8.wxyz - tmp7.wxyz;
                tmp7 = tmp0.xxxx * tmp8 + tmp7.wxyz;
                tmp8 = float4(1.0, 1.0, 1.0, 1.0) - tmp7;
                tmp1.w = tmp8.x * tmp8.x;
                tmp2.w = unity_SpecCube0_ProbePosition.w > 0.0;
                if (tmp2.w) {
                    tmp2.w = dot(tmp3.xyz, tmp3.xyz);
                    tmp2.w = rsqrt(tmp2.w);
                    tmp9.xyz = tmp2.www * tmp3.xyz;
                    tmp10.xyz = unity_SpecCube0_BoxMax.xyz - inp.texcoord3.xyz;
                    tmp10.xyz = tmp10.xyz / tmp9.xyz;
                    tmp11.xyz = unity_SpecCube0_BoxMin.xyz - inp.texcoord3.xyz;
                    tmp11.xyz = tmp11.xyz / tmp9.xyz;
                    tmp12.xyz = tmp9.xyz > float3(0.0, 0.0, 0.0);
                    tmp10.xyz = tmp12.xyz ? tmp10.xyz : tmp11.xyz;
                    tmp2.w = min(tmp10.y, tmp10.x);
                    tmp2.w = min(tmp10.z, tmp2.w);
                    tmp10.xyz = inp.texcoord3.xyz - unity_SpecCube0_ProbePosition.xyz;
                    tmp9.xyz = tmp9.xyz * tmp2.www + tmp10.xyz;
                } else {
                    tmp9.xyz = tmp3.xyz;
                }
                tmp2.w = -tmp8.x * 0.7 + 1.7;
                tmp2.w = tmp2.w * tmp8.x;
                tmp2.w = tmp2.w * 6.0;
                tmp9 = UNITY_SAMPLE_TEXCUBE_SAMPLER(unity_SpecCube0, unity_SpecCube0, float4(tmp9.xyz, tmp2.w));
                tmp3.w = tmp9.w - 1.0;
                tmp3.w = unity_SpecCube0_HDR.w * tmp3.w + 1.0;
                tmp3.w = tmp3.w * unity_SpecCube0_HDR.x;
                tmp10.xyz = tmp9.xyz * tmp3.www;
                tmp4.w = unity_SpecCube0_BoxMin.w < 0.99999;
                if (tmp4.w) {
                    tmp4.w = unity_SpecCube1_ProbePosition.w > 0.0;
                    if (tmp4.w) {
                        tmp4.w = dot(tmp3.xyz, tmp3.xyz);
                        tmp4.w = rsqrt(tmp4.w);
                        tmp11.xyz = tmp3.xyz * tmp4.www;
                        tmp12.xyz = unity_SpecCube1_BoxMax.xyz - inp.texcoord3.xyz;
                        tmp12.xyz = tmp12.xyz / tmp11.xyz;
                        tmp13.xyz = unity_SpecCube1_BoxMin.xyz - inp.texcoord3.xyz;
                        tmp13.xyz = tmp13.xyz / tmp11.xyz;
                        tmp14.xyz = tmp11.xyz > float3(0.0, 0.0, 0.0);
                        tmp12.xyz = tmp14.xyz ? tmp12.xyz : tmp13.xyz;
                        tmp4.w = min(tmp12.y, tmp12.x);
                        tmp4.w = min(tmp12.z, tmp4.w);
                        tmp12.xyz = inp.texcoord3.xyz - unity_SpecCube1_ProbePosition.xyz;
                        tmp3.xyz = tmp11.xyz * tmp4.www + tmp12.xyz;
                    }
                    tmp11 = UNITY_SAMPLE_TEXCUBE_SAMPLER(unity_SpecCube0, unity_SpecCube0, float4(tmp3.xyz, tmp2.w));
                    tmp2.w = tmp11.w - 1.0;
                    tmp2.w = unity_SpecCube1_HDR.w * tmp2.w + 1.0;
                    tmp2.w = tmp2.w * unity_SpecCube1_HDR.x;
                    tmp3.xyz = tmp11.xyz * tmp2.www;
                    tmp9.xyz = tmp3.www * tmp9.xyz + -tmp3.xyz;
                    tmp10.xyz = unity_SpecCube0_BoxMin.www * tmp9.xyz + tmp3.xyz;
                }
                tmp2.w = dot(tmp0.xyz, tmp4.xyz);
                tmp2.w = max(tmp2.w, 0.0);
                tmp3.x = min(tmp2.w, 1.0);
                tmp3.y = saturate(dot(tmp4.xyz, tmp1.xyz));
                tmp3.zw = inp.texcoord.xy * _MossAlbedo_ST.xy + _MossAlbedo_ST.zw;
                tmp4 = tex2D(_MossAlbedo, tmp3.zw);
                tmp3.zw = inp.texcoord.xy * _Detail_ST.xy + _Detail_ST.zw;
                tmp9 = tex2D(_Detail, tmp3.zw);
                tmp5.xyz = tmp5.xyz * _Color.xyz;
                tmp4.xyz = tmp4.xyz * _MossColor.xyz + -tmp5.xyz;
                tmp4.xyz = tmp0.xxx * tmp4.xyz + tmp5.xyz;
                tmp4.xyz = tmp9.xyz * tmp4.xyz;
                tmp4.xyz = tmp4.xyz + tmp4.xyz;
                tmp0.x = max(tmp7.z, tmp7.y);
                tmp0.x = max(tmp7.w, tmp0.x);
                tmp0.x = 1.0 - tmp0.x;
                tmp4.xyz = tmp0.xxx * tmp4.xyz;
                tmp0.x = 1.0 - tmp0.x;
                tmp2.x = dot(tmp0.xyz, tmp2.xyz);
                tmp0.y = saturate(dot(tmp0.xyz, tmp1.xyz));
                tmp0.z = -tmp8.x * tmp8.x + 1.0;
                tmp0.w = abs(tmp2.x) * tmp0.z + tmp1.w;
                tmp0.z = tmp3.x * tmp0.z + tmp1.w;
                tmp0.z = tmp0.z * abs(tmp2.x);
                tmp0.z = tmp3.x * tmp0.w + tmp0.z;
                tmp0.z = tmp0.z + 0.00001;
                tmp0.z = 0.5 / tmp0.z;
                tmp0.w = tmp1.w * tmp1.w;
                tmp1.x = tmp0.y * tmp0.w + -tmp0.y;
                tmp0.y = tmp1.x * tmp0.y + 1.0;
                tmp0.w = tmp0.w * 0.3183099;
                tmp0.y = tmp0.y * tmp0.y + 0.0000001;
                tmp0.y = tmp0.w / tmp0.y;
                tmp0.y = tmp0.y * tmp0.z;
                tmp0.y = tmp0.y * 3.141593;
                tmp0.y = max(tmp0.y, 0.0001);
                tmp0.y = sqrt(tmp0.y);
                tmp0.y = tmp3.x * tmp0.y;
                tmp0.z = tmp1.w * tmp8.x;
                tmp0.z = -tmp0.z * 0.28 + 1.0;
                tmp0.w = dot(tmp7.xyz, tmp7.xyz);
                tmp0.w = tmp0.w != 0.0;
                tmp0.w = tmp0.w ? 1.0 : 0.0;
                tmp0.y = tmp0.w * tmp0.y;
                tmp1.xyz = tmp0.yyy * tmp6.xyz;
                tmp0.y = 1.0 - tmp3.y;
                tmp0.w = tmp0.y * tmp0.y;
                tmp0.w = tmp0.w * tmp0.w;
                tmp0.y = tmp0.y * tmp0.w;
                tmp3.xzw = tmp8.yzw * tmp0.yyy + tmp7.yzw;
                tmp0.y = saturate(tmp0.x + tmp7.x);
                tmp0.w = 1.0 - abs(tmp2.x);
                tmp1.w = tmp0.w * tmp0.w;
                tmp1.w = tmp1.w * tmp1.w;
                tmp0.w = tmp0.w * tmp1.w;
                tmp2.xyz = tmp0.yyy - tmp7.yzw;
                tmp2.xyz = tmp0.www * tmp2.xyz + tmp7.yzw;
                tmp2.xyz = tmp2.xyz * tmp10.xyz;
                tmp2.xyz = tmp0.zzz * tmp2.xyz;
                tmp1.xyz = tmp1.xyz * tmp3.xzw + tmp2.xyz;
                tmp0.y = tmp3.y + tmp3.y;
                tmp0.y = tmp3.y * tmp0.y;
                tmp0.z = 1.0 - tmp2.w;
                tmp1.w = tmp0.z * tmp0.z;
                tmp1.w = tmp1.w * tmp1.w;
                tmp0.z = tmp0.z * tmp1.w;
                tmp0.y = tmp0.y * tmp8.x + -0.5;
                tmp0.z = tmp0.y * tmp0.z + 1.0;
                tmp0.y = tmp0.y * tmp0.w + 1.0;
                tmp0.y = tmp0.y * tmp0.z;
                tmp0.y = tmp2.w * tmp0.y;
                tmp0.yzw = tmp6.xyz * tmp0.yyy;
                tmp0.x = 1.0 - tmp0.x;
                tmp2.xyz = tmp0.xxx * tmp4.xyz;
                o.sv_target.xyz = tmp0.yzw * tmp2.xyz + tmp1.xyz;
                o.sv_target.w = 1.0;
                return o;
			}
			ENDCG
		}
		Pass {
			Name "FORWARD_DELTA"
			Tags { "LIGHTMODE" = "FORWARDADD" "RenderType" = "Opaque" "SHADOWSUPPORT" = "true" }
			Blend One One, One One
			GpuProgramID 113584
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			
			#include "UnityCG.cginc"
			struct v2f
			{
				float4 position : SV_POSITION0;
				float2 texcoord : TEXCOORD0;
				float2 texcoord1 : TEXCOORD1;
				float2 texcoord2 : TEXCOORD2;
				float4 texcoord3 : TEXCOORD3;
				float3 texcoord4 : TEXCOORD4;
				float3 texcoord5 : TEXCOORD5;
				float3 texcoord6 : TEXCOORD6;
				float4 color : COLOR0;
				float3 texcoord7 : TEXCOORD7;
			};
			struct fout
			{
				float4 sv_target : SV_Target0;
			};
			// $Globals ConstantBuffers for Vertex Shader
			float4x4 unity_WorldToLight;
			// $Globals ConstantBuffers for Fragment Shader
			float4 _LightColor0;
			float4 _Color;
			float4 _MainTex_ST;
			float4 _BumpMap_ST;
			float4 _Specular_ST;
			float4 _Detail_ST;
			float4 _MossAlbedo_ST;
			float4 _MossNormalMap_ST;
			float4 _MossColor;
			float4 _SpecularMoss_ST;
			float _upnormalmoss;
			float _colourasnormal;
			float _paintedmoss;
			// Custom ConstantBuffers for Vertex Shader
			// Custom ConstantBuffers for Fragment Shader
			// Texture params for Vertex Shader
			// Texture params for Fragment Shader
			sampler2D _BumpMap;
			sampler2D _MossNormalMap;
			sampler2D _MainTex;
			sampler2D _LightTexture0;
			sampler2D _Specular;
			sampler2D _SpecularMoss;
			sampler2D _MossAlbedo;
			sampler2D _Detail;
			
			// Keywords: POINT LIGHTMAP_OFF DIRLIGHTMAP_OFF DYNAMICLIGHTMAP_OFF
			v2f vert(appdata_full v)
			{
                v2f o;
                float4 tmp0;
                float4 tmp1;
                float4 tmp2;
                float4 tmp3;
                tmp0 = v.vertex.yyyy * unity_ObjectToWorld._m01_m11_m21_m31;
                tmp0 = unity_ObjectToWorld._m00_m10_m20_m30 * v.vertex.xxxx + tmp0;
                tmp0 = unity_ObjectToWorld._m02_m12_m22_m32 * v.vertex.zzzz + tmp0;
                tmp1 = tmp0 + unity_ObjectToWorld._m03_m13_m23_m33;
                tmp0 = unity_ObjectToWorld._m03_m13_m23_m33 * v.vertex.wwww + tmp0;
                tmp2 = tmp1.yyyy * unity_MatrixVP._m01_m11_m21_m31;
                tmp2 = unity_MatrixVP._m00_m10_m20_m30 * tmp1.xxxx + tmp2;
                tmp2 = unity_MatrixVP._m02_m12_m22_m32 * tmp1.zzzz + tmp2;
                o.position = unity_MatrixVP._m03_m13_m23_m33 * tmp1.wwww + tmp2;
                o.texcoord.xy = v.texcoord.xy;
                o.texcoord1.xy = v.texcoord1.xy;
                o.texcoord2.xy = v.texcoord2.xy;
                o.texcoord3 = tmp0;
                tmp1.x = dot(v.normal.xyz, unity_WorldToObject._m00_m10_m20);
                tmp1.y = dot(v.normal.xyz, unity_WorldToObject._m01_m11_m21);
                tmp1.z = dot(v.normal.xyz, unity_WorldToObject._m02_m12_m22);
                tmp1.w = dot(tmp1.xyz, tmp1.xyz);
                tmp1.w = rsqrt(tmp1.w);
                tmp1.xyz = tmp1.www * tmp1.xyz;
                o.texcoord4.xyz = tmp1.xyz;
                tmp2.xyz = v.tangent.yyy * unity_ObjectToWorld._m01_m11_m21;
                tmp2.xyz = unity_ObjectToWorld._m00_m10_m20 * v.tangent.xxx + tmp2.xyz;
                tmp2.xyz = unity_ObjectToWorld._m02_m12_m22 * v.tangent.zzz + tmp2.xyz;
                tmp1.w = dot(tmp2.xyz, tmp2.xyz);
                tmp1.w = rsqrt(tmp1.w);
                tmp2.xyz = tmp1.www * tmp2.xyz;
                o.texcoord5.xyz = tmp2.xyz;
                tmp3.xyz = tmp1.zxy * tmp2.yzx;
                tmp1.xyz = tmp1.yzx * tmp2.zxy + -tmp3.xyz;
                tmp1.xyz = tmp1.xyz * v.tangent.www;
                tmp1.w = dot(tmp1.xyz, tmp1.xyz);
                tmp1.w = rsqrt(tmp1.w);
                o.texcoord6.xyz = tmp1.www * tmp1.xyz;
                o.color = v.color;
                tmp1.xyz = tmp0.yyy * unity_WorldToLight._m01_m11_m21;
                tmp1.xyz = unity_WorldToLight._m00_m10_m20 * tmp0.xxx + tmp1.xyz;
                tmp0.xyz = unity_WorldToLight._m02_m12_m22 * tmp0.zzz + tmp1.xyz;
                o.texcoord7.xyz = unity_WorldToLight._m03_m13_m23 * tmp0.www + tmp0.xyz;
                return o;
			}
			// Keywords: POINT LIGHTMAP_OFF DIRLIGHTMAP_OFF DYNAMICLIGHTMAP_OFF
			fout frag(v2f inp)
			{
                fout o;
                float4 tmp0;
                float4 tmp1;
                float4 tmp2;
                float4 tmp3;
                float4 tmp4;
                float4 tmp5;
                float4 tmp6;
                float4 tmp7;
                float4 tmp8;
                float4 tmp9;
                tmp0.x = dot(inp.texcoord4.xyz, inp.texcoord4.xyz);
                tmp0.x = rsqrt(tmp0.x);
                tmp0.yzw = tmp0.xxx * inp.texcoord4.xyz;
                tmp1.xyz = _WorldSpaceCameraPos - inp.texcoord3.xyz;
                tmp1.w = dot(tmp1.xyz, tmp1.xyz);
                tmp1.w = rsqrt(tmp1.w);
                tmp2.xyz = tmp1.www * tmp1.xyz;
                tmp3.xy = inp.texcoord.xy * _BumpMap_ST.xy + _BumpMap_ST.zw;
                tmp3 = tex2D(_BumpMap, tmp3.xy);
                tmp3.x = tmp3.w * tmp3.x;
                tmp3.xy = tmp3.xy * float2(2.0, 2.0) + float2(-1.0, -1.0);
                tmp2.w = dot(tmp3.xy, tmp3.xy);
                tmp2.w = min(tmp2.w, 1.0);
                tmp2.w = 1.0 - tmp2.w;
                tmp3.z = sqrt(tmp2.w);
                tmp4.xy = inp.texcoord.xy * _MossNormalMap_ST.xy + _MossNormalMap_ST.zw;
                tmp4 = tex2D(_MossNormalMap, tmp4.xy);
                tmp4.x = tmp4.w * tmp4.x;
                tmp4.xy = tmp4.xy * float2(2.0, 2.0) + float2(-1.0, -1.0);
                tmp2.w = dot(tmp4.xy, tmp4.xy);
                tmp2.w = min(tmp2.w, 1.0);
                tmp2.w = 1.0 - tmp2.w;
                tmp4.z = sqrt(tmp2.w);
                tmp5.xy = inp.texcoord.xy * _MainTex_ST.xy + _MainTex_ST.zw;
                tmp5 = tex2D(_MainTex, tmp5.xy);
                tmp2.w = 1.0 - inp.color.x;
                tmp6.xyz = inp.color.xyz * float3(2.0, 2.0, 2.0) + float3(-1.0, -1.0, -1.0);
                tmp3.w = tmp6.y * unity_ObjectToWorld._m11;
                tmp3.w = unity_ObjectToWorld._m10 * tmp6.x + tmp3.w;
                tmp3.w = unity_ObjectToWorld._m12 * tmp6.z + tmp3.w;
                tmp0.x = -inp.texcoord4.y * tmp0.x + tmp3.w;
                tmp0.x = saturate(_colourasnormal * tmp0.x + tmp0.z);
                tmp0.x = tmp0.x * _upnormalmoss;
                tmp0.x = tmp2.w * _paintedmoss + tmp0.x;
                tmp0.x = tmp0.x >= tmp5.w;
                tmp0.x = tmp0.x ? 1.0 : 0.0;
                tmp4.xyz = tmp4.xyz - tmp3.xyz;
                tmp3.xyz = tmp0.xxx * tmp4.xyz + tmp3.xyz;
                tmp4.xyz = tmp3.yyy * inp.texcoord6.xyz;
                tmp3.xyw = tmp3.xxx * inp.texcoord5.xyz + tmp4.xyz;
                tmp0.yzw = tmp3.zzz * tmp0.yzw + tmp3.xyw;
                tmp2.w = dot(tmp0.xyz, tmp0.xyz);
                tmp2.w = rsqrt(tmp2.w);
                tmp0.yzw = tmp0.yzw * tmp2.www;
                tmp3.xyz = _WorldSpaceLightPos0.www * -inp.texcoord3.xyz + _WorldSpaceLightPos0.xyz;
                tmp2.w = dot(tmp3.xyz, tmp3.xyz);
                tmp2.w = rsqrt(tmp2.w);
                tmp3.xyz = tmp2.www * tmp3.xyz;
                tmp1.xyz = tmp1.xyz * tmp1.www + tmp3.xyz;
                tmp1.w = dot(tmp1.xyz, tmp1.xyz);
                tmp1.w = rsqrt(tmp1.w);
                tmp1.xyz = tmp1.www * tmp1.xyz;
                tmp4.xyz = inp.texcoord3.yyy * unity_WorldToLight._m01_m11_m21;
                tmp4.xyz = unity_WorldToLight._m00_m10_m20 * inp.texcoord3.xxx + tmp4.xyz;
                tmp4.xyz = unity_WorldToLight._m02_m12_m22 * inp.texcoord3.zzz + tmp4.xyz;
                tmp4.xyz = tmp4.xyz + unity_WorldToLight._m03_m13_m23;
                tmp1.w = unity_ProbeVolumeParams.x == 1.0;
                if (tmp1.w) {
                    tmp1.w = unity_ProbeVolumeParams.y == 1.0;
                    tmp6.xyz = inp.texcoord3.yyy * unity_ProbeVolumeWorldToObject._m01_m11_m21;
                    tmp6.xyz = unity_ProbeVolumeWorldToObject._m00_m10_m20 * inp.texcoord3.xxx + tmp6.xyz;
                    tmp6.xyz = unity_ProbeVolumeWorldToObject._m02_m12_m22 * inp.texcoord3.zzz + tmp6.xyz;
                    tmp6.xyz = tmp6.xyz + unity_ProbeVolumeWorldToObject._m03_m13_m23;
                    tmp6.xyz = tmp1.www ? tmp6.xyz : inp.texcoord3.xyz;
                    tmp6.xyz = tmp6.xyz - unity_ProbeVolumeMin;
                    tmp6.yzw = tmp6.xyz * unity_ProbeVolumeSizeInv;
                    tmp1.w = tmp6.y * 0.25 + 0.75;
                    tmp2.w = unity_ProbeVolumeParams.z * 0.5 + 0.75;
                    tmp6.x = max(tmp1.w, tmp2.w);
                    tmp6 = UNITY_SAMPLE_TEX3D_SAMPLER(unity_ProbeVolumeSH, unity_ProbeVolumeSH, tmp6.xzw);
                } else {
                    tmp6 = float4(1.0, 1.0, 1.0, 1.0);
                }
                tmp1.w = saturate(dot(tmp6, unity_OcclusionMaskSelector));
                tmp2.w = dot(tmp4.xyz, tmp4.xyz);
                tmp4 = tex2D(_LightTexture0, tmp2.ww);
                tmp1.w = tmp1.w * tmp4.x;
                tmp4.xyz = tmp1.www * _LightColor0.xyz;
                tmp6.xy = inp.texcoord.xy * _Specular_ST.xy + _Specular_ST.zw;
                tmp6 = tex2D(_Specular, tmp6.xy);
                tmp7.xy = inp.texcoord.xy * _SpecularMoss_ST.xy + _SpecularMoss_ST.zw;
                tmp7 = tex2D(_SpecularMoss, tmp7.xy);
                tmp7 = tmp7.wxyz - tmp6.wxyz;
                tmp6 = tmp0.xxxx * tmp7 + tmp6.wxyz;
                tmp7 = float4(1.0, 1.0, 1.0, 1.0) - tmp6;
                tmp1.w = tmp7.x * tmp7.x;
                tmp2.w = dot(tmp0.xyz, tmp3.xyz);
                tmp2.w = max(tmp2.w, 0.0);
                tmp3.w = min(tmp2.w, 1.0);
                tmp3.x = saturate(dot(tmp3.xyz, tmp1.xyz));
                tmp3.yz = inp.texcoord.xy * _MossAlbedo_ST.xy + _MossAlbedo_ST.zw;
                tmp8 = tex2D(_MossAlbedo, tmp3.yz);
                tmp3.yz = inp.texcoord.xy * _Detail_ST.xy + _Detail_ST.zw;
                tmp9 = tex2D(_Detail, tmp3.yz);
                tmp5.xyz = tmp5.xyz * _Color.xyz;
                tmp8.xyz = tmp8.xyz * _MossColor.xyz + -tmp5.xyz;
                tmp5.xyz = tmp0.xxx * tmp8.xyz + tmp5.xyz;
                tmp5.xyz = tmp9.xyz * tmp5.xyz;
                tmp5.xyz = tmp5.xyz + tmp5.xyz;
                tmp0.x = max(tmp6.z, tmp6.y);
                tmp0.x = max(tmp6.w, tmp0.x);
                tmp0.x = 1.0 - tmp0.x;
                tmp5.xyz = tmp0.xxx * tmp5.xyz;
                tmp0.x = 1.0 - tmp0.x;
                tmp2.x = dot(tmp0.xyz, tmp2.xyz);
                tmp0.y = saturate(dot(tmp0.xyz, tmp1.xyz));
                tmp0.z = -tmp7.x * tmp7.x + 1.0;
                tmp0.w = abs(tmp2.x) * tmp0.z + tmp1.w;
                tmp0.z = tmp3.w * tmp0.z + tmp1.w;
                tmp0.z = tmp0.z * abs(tmp2.x);
                tmp0.z = tmp3.w * tmp0.w + tmp0.z;
                tmp0.z = tmp0.z + 0.00001;
                tmp0.z = 0.5 / tmp0.z;
                tmp0.w = tmp1.w * tmp1.w;
                tmp1.x = tmp0.y * tmp0.w + -tmp0.y;
                tmp0.y = tmp1.x * tmp0.y + 1.0;
                tmp0.w = tmp0.w * 0.3183099;
                tmp0.y = tmp0.y * tmp0.y + 0.0000001;
                tmp0.y = tmp0.w / tmp0.y;
                tmp0.y = tmp0.y * tmp0.z;
                tmp0.y = tmp0.y * 3.141593;
                tmp0.y = max(tmp0.y, 0.0001);
                tmp0.y = sqrt(tmp0.y);
                tmp0.y = tmp3.w * tmp0.y;
                tmp0.z = dot(tmp6.xyz, tmp6.xyz);
                tmp0.z = tmp0.z != 0.0;
                tmp0.z = tmp0.z ? 1.0 : 0.0;
                tmp0.y = tmp0.z * tmp0.y;
                tmp0.yzw = tmp0.yyy * tmp4.xyz;
                tmp1.x = 1.0 - tmp3.x;
                tmp1.y = tmp1.x * tmp1.x;
                tmp1.y = tmp1.y * tmp1.y;
                tmp1.x = tmp1.x * tmp1.y;
                tmp1.xyz = tmp7.yzw * tmp1.xxx + tmp6.yzw;
                tmp0.yzw = tmp0.yzw * tmp1.xyz;
                tmp1.x = tmp3.x + tmp3.x;
                tmp1.x = tmp3.x * tmp1.x;
                tmp1.y = 1.0 - tmp2.w;
                tmp1.z = tmp1.y * tmp1.y;
                tmp1.z = tmp1.z * tmp1.z;
                tmp1.y = tmp1.y * tmp1.z;
                tmp1.z = 1.0 - abs(tmp2.x);
                tmp1.w = tmp1.z * tmp1.z;
                tmp1.w = tmp1.w * tmp1.w;
                tmp1.z = tmp1.z * tmp1.w;
                tmp1.x = tmp1.x * tmp7.x + -0.5;
                tmp1.y = tmp1.x * tmp1.y + 1.0;
                tmp1.x = tmp1.x * tmp1.z + 1.0;
                tmp1.x = tmp1.x * tmp1.y;
                tmp1.x = tmp2.w * tmp1.x;
                tmp1.xyz = tmp4.xyz * tmp1.xxx;
                tmp0.x = 1.0 - tmp0.x;
                tmp2.xyz = tmp0.xxx * tmp5.xyz;
                o.sv_target.xyz = tmp1.xyz * tmp2.xyz + tmp0.yzw;
                o.sv_target.w = 0.0;
                return o;
			}
			ENDCG
		}
		Pass {
			Name "Meta"
			Tags { "LIGHTMODE" = "META" "RenderType" = "Opaque" "SHADOWSUPPORT" = "true" }
			Cull Off
			GpuProgramID 169547
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			
			#include "UnityCG.cginc"
			struct v2f
			{
				float4 position : SV_POSITION0;
				float2 texcoord : TEXCOORD0;
				float2 texcoord1 : TEXCOORD1;
				float2 texcoord2 : TEXCOORD2;
				float4 texcoord3 : TEXCOORD3;
				float3 texcoord4 : TEXCOORD4;
				float4 color : COLOR0;
			};
			struct fout
			{
				float4 sv_target : SV_Target0;
			};
			// $Globals ConstantBuffers for Vertex Shader
			// $Globals ConstantBuffers for Fragment Shader
			float unity_OneOverOutputBoost;
			float unity_MaxOutputValue;
			float4 _Color;
			float4 _MainTex_ST;
			float4 _Specular_ST;
			float4 _Detail_ST;
			float4 _MossAlbedo_ST;
			float4 _MossColor;
			float4 _SpecularMoss_ST;
			float _upnormalmoss;
			float _colourasnormal;
			float _paintedmoss;
			// Custom ConstantBuffers for Vertex Shader
			CBUFFER_START(UnityMetaPass)
				bool4 unity_MetaVertexControl;
			CBUFFER_END
			// Custom ConstantBuffers for Fragment Shader
			CBUFFER_START(UnityMetaPass)
				bool4 unity_MetaFragmentControl;
			CBUFFER_END
			// Texture params for Vertex Shader
			// Texture params for Fragment Shader
			sampler2D _MainTex;
			sampler2D _MossAlbedo;
			sampler2D _Detail;
			sampler2D _Specular;
			sampler2D _SpecularMoss;
			
			// Keywords: SHADOWS_DEPTH LIGHTMAP_OFF DIRLIGHTMAP_OFF DYNAMICLIGHTMAP_OFF
			v2f vert(appdata_full v)
			{
                v2f o;
                float4 tmp0;
                float4 tmp1;
                tmp0.x = v.vertex.z > 0.0;
                tmp0.z = tmp0.x ? 0.0001 : 0.0;
                tmp0.xy = v.texcoord1.xy * unity_LightmapST.xy + unity_LightmapST.zw;
                tmp0.xyz = unity_MetaVertexControl.xxx ? tmp0.xyz : v.vertex.xyz;
                tmp0.w = tmp0.z > 0.0;
                tmp1.z = tmp0.w ? 0.0001 : 0.0;
                tmp1.xy = v.texcoord2.xy * unity_DynamicLightmapST.xy + unity_DynamicLightmapST.zw;
                tmp0.xyz = unity_MetaVertexControl.yyy ? tmp1.xyz : tmp0.xyz;
                tmp1 = tmp0.yyyy * unity_MatrixVP._m01_m11_m21_m31;
                tmp1 = unity_MatrixVP._m00_m10_m20_m30 * tmp0.xxxx + tmp1;
                tmp0 = unity_MatrixVP._m02_m12_m22_m32 * tmp0.zzzz + tmp1;
                o.position = tmp0 + unity_MatrixVP._m03_m13_m23_m33;
                o.texcoord.xy = v.texcoord.xy;
                o.texcoord1.xy = v.texcoord1.xy;
                o.texcoord2.xy = v.texcoord2.xy;
                tmp0 = v.vertex.yyyy * unity_ObjectToWorld._m01_m11_m21_m31;
                tmp0 = unity_ObjectToWorld._m00_m10_m20_m30 * v.vertex.xxxx + tmp0;
                tmp0 = unity_ObjectToWorld._m02_m12_m22_m32 * v.vertex.zzzz + tmp0;
                o.texcoord3 = unity_ObjectToWorld._m03_m13_m23_m33 * v.vertex.wwww + tmp0;
                tmp0.x = dot(v.normal.xyz, unity_WorldToObject._m00_m10_m20);
                tmp0.y = dot(v.normal.xyz, unity_WorldToObject._m01_m11_m21);
                tmp0.z = dot(v.normal.xyz, unity_WorldToObject._m02_m12_m22);
                tmp0.w = dot(tmp0.xyz, tmp0.xyz);
                tmp0.w = rsqrt(tmp0.w);
                o.texcoord4.xyz = tmp0.www * tmp0.xyz;
                o.color = v.color;
                return o;
			}
			// Keywords: SHADOWS_DEPTH LIGHTMAP_OFF DIRLIGHTMAP_OFF DYNAMICLIGHTMAP_OFF
			fout frag(v2f inp)
			{
                fout o;
                float4 tmp0;
                float4 tmp1;
                float4 tmp2;
                tmp0.xyz = inp.color.xyz * float3(2.0, 2.0, 2.0) + float3(-1.0, -1.0, -1.0);
                tmp0.y = tmp0.y * unity_ObjectToWorld._m11;
                tmp0.x = unity_ObjectToWorld._m10 * tmp0.x + tmp0.y;
                tmp0.x = unity_ObjectToWorld._m12 * tmp0.z + tmp0.x;
                tmp0.y = dot(inp.texcoord4.xyz, inp.texcoord4.xyz);
                tmp0.y = rsqrt(tmp0.y);
                tmp0.x = -inp.texcoord4.y * tmp0.y + tmp0.x;
                tmp0.y = tmp0.y * inp.texcoord4.y;
                tmp0.x = saturate(_colourasnormal * tmp0.x + tmp0.y);
                tmp0.x = tmp0.x * _upnormalmoss;
                tmp0.y = 1.0 - inp.color.x;
                tmp0.x = tmp0.y * _paintedmoss + tmp0.x;
                tmp0.yz = inp.texcoord.xy * _MainTex_ST.xy + _MainTex_ST.zw;
                tmp1 = tex2D(_MainTex, tmp0.yz);
                tmp0.x = tmp0.x >= tmp1.w;
                tmp0.yzw = tmp1.xyz * _Color.xyz;
                tmp0.x = tmp0.x ? 1.0 : 0.0;
                tmp1.xy = inp.texcoord.xy * _MossAlbedo_ST.xy + _MossAlbedo_ST.zw;
                tmp1 = tex2D(_MossAlbedo, tmp1.xy);
                tmp1.xyz = tmp1.xyz * _MossColor.xyz + -tmp0.yzw;
                tmp0.yzw = tmp0.xxx * tmp1.xyz + tmp0.yzw;
                tmp1.xy = inp.texcoord.xy * _Detail_ST.xy + _Detail_ST.zw;
                tmp1 = tex2D(_Detail, tmp1.xy);
                tmp0.yzw = tmp0.yzw * tmp1.xyz;
                tmp0.yzw = tmp0.yzw + tmp0.yzw;
                tmp1.xy = inp.texcoord.xy * _SpecularMoss_ST.xy + _SpecularMoss_ST.zw;
                tmp1 = tex2D(_SpecularMoss, tmp1.xy);
                tmp2.xy = inp.texcoord.xy * _Specular_ST.xy + _Specular_ST.zw;
                tmp2 = tex2D(_Specular, tmp2.xy);
                tmp1 = tmp1 - tmp2;
                tmp1 = tmp0.xxxx * tmp1 + tmp2;
                tmp0.x = 1.0 - tmp1.w;
                tmp0.x = tmp0.x * tmp0.x;
                tmp2.xyz = tmp1.xyz * tmp0.xxx;
                tmp2.xyz = tmp2.xyz * float3(0.5, 0.5, 0.5);
                tmp0.x = max(tmp1.y, tmp1.x);
                tmp0.x = max(tmp1.z, tmp0.x);
                tmp0.x = 1.0 - tmp0.x;
                tmp0.xyz = tmp0.yzw * tmp0.xxx + tmp2.xyz;
                tmp0.xyz = log(tmp0.xyz);
                tmp0.w = saturate(unity_OneOverOutputBoost);
                tmp0.xyz = tmp0.xyz * tmp0.www;
                tmp0.xyz = exp(tmp0.xyz);
                tmp0.xyz = min(tmp0.xyz, unity_MaxOutputValue.xxx);
                tmp0.w = 1.0;
                tmp0 = unity_MetaFragmentControl ? tmp0 : float4(0.0, 0.0, 0.0, 0.0);
                o.sv_target = unity_MetaFragmentControl ? float4(0.0, 0.0, 0.0, 1.0) : tmp0;
                return o;
			}
			ENDCG
		}
	}
	Fallback "Diffuse"
	CustomEditor "ShaderForgeMaterialInspector"
}
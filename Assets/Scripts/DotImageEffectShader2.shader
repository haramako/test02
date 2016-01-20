Shader "Hidden/DotImageEffectShader2" {
	Properties{
		_MainTex("", 2D) = "" {}
	}
		SubShader{

		ZTest Off
		ZWrite Off
		Cull Off
		Fog{ Mode Off }
		Blend Off

		CGINCLUDE
		#include "UnityCG.cginc"
		#pragma target 3.0
		struct v2f
		{
			float4 pos : SV_POSITION;
			float2 uv : TEXCOORD0;
			float2 uv2 : TEXCOORD1;
		};			

		sampler2D _MainTex;
		float4 _MainTex_ST;
		float4 _MainTex_TexelSize;
		sampler2D _CameraDepthNormalsTexture;
		float4 _CameraDepthNormalsTexture_ST;

		v2f vert(appdata_img v)
		{
			v2f o;
			o.pos = mul(UNITY_MATRIX_MVP, v.vertex);
			o.uv = TRANSFORM_TEX(v.texcoord, _MainTex);
			o.uv2 = TRANSFORM_TEX(v.texcoord, _CameraDepthNormalsTexture);

			if (_MainTex_TexelSize.y < 0)
				o.uv2.y = 1.0 - o.uv2.y;

			return o;

		}				
		

		float GetDepth(half2 uv)
		{
			float4 depthnormal = tex2D(_CameraDepthNormalsTexture, uv);
			float3 viewNorm;
			float depth;
			DecodeDepthNormal(depthnormal, depth, viewNorm);
			return depth;
		}
		#pragma vertex vert
		ENDCG

		// Pass0 RGB+D(Depth*(1-a))
		Pass{

			CGPROGRAM
			#pragma fragment frag

			fixed4 frag(v2f i) : SV_Target
			{
			float depth0 = GetDepth(i.uv2);
			float depth1 = GetDepth(i.uv2+float2(0,_MainTex_TexelSize.y));
			float depth2 = GetDepth(i.uv2+float2(_MainTex_TexelSize.x,0));
			float depth3 = GetDepth(i.uv2+float2(0,-_MainTex_TexelSize.y));
			float depth4 = GetDepth(i.uv2+float2(-_MainTex_TexelSize.x,0));

			fixed4 col = tex2D(_MainTex,i.uv);

			float diff = max(max(depth1-depth0, depth2-depth0), max(depth3-depth0, depth4-depth0));
			if( diff > 0.0005 ){
				diff = min(diff / 0.0005,1);
				//col = (col*4 + col1 + col2 + col3 + col4)/8*(1-diff);
				//col.a = 1;
				//col.rgb = col.rgb * 0.2;
			}

			if( diff > 0.0005 ){
				diff = min(diff / 0.0005,1);
				//col = (col*4 + col1 + col2 + col3 + col4)/8*(1-diff);
				//col.a = 1;
				//col.rgb = col.rgb * 0.2;
				//col.rgb = col.rgb * 0.8;
			}
			return col;
				//half4 centerCol = tex2D(_MainTex, i.uv);
				//centerCol.a = (1 - centerCol.a) * GetDepth(i.uv2);
				//return centerCol;
			}
				ENDCG
		} // Pass0


		// Pass1 DepthOnly
		Pass{

				CGPROGRAM
				#pragma fragment frag

				half4 frag(v2f i) : SV_Target
				{
					return tex2D(_MainTex,i.uv);
				}
				ENDCG
		} // Pass1
		
	}
}
Shader "Unlit/Project"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
	}
	SubShader
	{
		
		Pass {

			CGPROGRAM
			
			#pragma vertex ver
			#pragma fragment frag

			struct vin {
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
				float3 normal : NORMAL;
				float4 colour : COLOR;
			};
			
			struct vout {
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
				float3 normal : NORMAL;
				float4 colour : COLOR;
			};

			vout ver(vin i){
				vout o;
				o.vertex = mul(UNITY_MATRIX_MVP,i.vertex);
				o.uv = i.uv;
				o.normal = i.normal;
				o.colour = i.colour;
				return o;
			}

			float4 frag(vout i) : COLOR {
				return i.colour;
			}


				ENDCG
		}
		Pass{
			Blend SrcAlpha OneMinusSrcAlpha
			Cull Off

			Tags{"Queue" = "Overlay"}

			CGPROGRAM

			#pragma vertex ver
			#pragma fragment frag

			struct vin {
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
				float3 normal : NORMAL;
			};

			struct vout {
				float4 vertex : POSITION;
				float4 orig : TEXCOORD0;
				float2 uv : TEXCOORD1;
				float3 normal : NORMAL;
			};

			vout ver(vin i) {
				vout o;
				o.vertex = mul(UNITY_MATRIX_MVP, i.vertex + float4(i.normal,0));
				o.uv = i.uv;
				o.normal = i.normal;
				o.orig = i.vertex;
				return o;
			}

			float4 frag(vout i) : COLOR{
				float3 normalPlane = float3(1,1,1) - abs(float3(1,1,1)*normalize(i.normal));
				// The distance along the two dimensions in the tangent plane to the normal
				float tangentx;
				float binormalx;

				if (normalPlane.x != 0) {
					tangentx = i.orig.x;
					if (normalPlane.y != 0) {
						binormalx = i.orig.y;
					}
					else {
						binormalx = i.orig.z;
					}
				}
				else {
					tangentx = i.orig.y;
					binormalx = i.orig.z;
				}

				tangentx = saturate(abs(tangentx));
				binormalx = saturate(abs(binormalx));


				float factor = 0.4;
				if ((1 - abs(tangentx - 0.5)) < (1 - factor) && (1 - abs(binormalx - 0.5)) < (1 - factor))
					discard;

				return float4(1, 0, 0, 0.5);
			}


			ENDCG
		}
	}
}

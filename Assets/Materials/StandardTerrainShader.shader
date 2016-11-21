// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'

// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'

Shader "Unlit/TerrainShaderProject"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_DisplacementMap("DisplacementMap", 2D) = "white" {}
		_NormalMap("NormalMap",2D) = "black" {}
		// This will be shifted inside the shader as a vertex property a little later.
		_Color("Diffuse Material Color", Color) = (1,1,1,1)
		_SpecColor("Specular Material Color", Color) = (1,1,1,1)
		_Shininess("Shininess", Float) = 10
		_shaderMinX("Minimum X worldPos", Float) = 0
		_shaderMinZ("Minimum Z worldPos", Float) = 0
		_shaderWidth("Width of Display Block", Float) = 0
		_shaderLength("Length of Display Block", Float) = 0
		_textureInvMagnitude("Inverse texture size factor", Float) = 1
		_bumpMapInvMagnitude("Inverse bump map size factor", Float) = 1
	}
	SubShader
	{	
		Tags {"Queue" = "Geometry" "RenderType" = "Opaque"}
		// Light Shader is Phong with multiple light sources, taken from https://en.wikibooks.org/wiki/Cg_Programming/Unity/Multiple_Lights
		// as Phong wasn't sufficient light shading (and was the objective of the first assignment, so probably isn't a key for this
		// assignment). The shader has been modified to also handle shadows through the tutorial at 
		// http://kylehalladay.com/blog/tutorial/bestof/2013/10/13/Multi-Light-Diffuse.html.
		Pass {      
         Tags { "LightMode" = "ForwardBase" } // pass for 
            // 4 vertex lights, ambient light & first pixel light
 
         CGPROGRAM
         #pragma multi_compile_fwdbase 
         #pragma vertex vert
         #pragma fragment frag
 
         #include "UnityCG.cginc"
		 #include "Lighting.cginc"
		 #include "AutoLight.cginc"
         //fixed4 _LightColor0
            // color of light source (from "Lighting.cginc")
 
         // User-specified properties
         uniform float4 _Color; 
         //uniform float4 _SpecColor; 
         uniform float _Shininess;
		 uniform sampler2D _MainTex;
		 uniform sampler2D _DisplacementMap;
		 uniform sampler2D _NormalMap;
 
         struct vertexInput {
            float4 vertex : POSITION;
            float3 normal : NORMAL;
			float2 uv : TEXCOORD0;
         };
         struct vertexOutput {
            float4 pos : SV_POSITION;
			float2 uv : TEXCOORD0;
            float3 normalDir : TEXCOORD1;
            float3 vertexLighting : TEXCOORD2;
			float3 lightDir : TEXCOORD3;
			LIGHTING_COORDS(4, 5)
         };
 
         vertexOutput vert(vertexInput v)
         {          
            vertexOutput output;

			//output.tangentWorld = normalize(mul(_Object2World, float4(v.tangent.xyz, 0.0)).xyz);
            //output.normalDir = normalize(mul(float4(v.normal,0.0), _World2Object).xyz);
			//output.binormalWorld = normalize(cross)
			output.normalDir = v.normal;

			output.pos = mul(UNITY_MATRIX_MVP, v.vertex);
            
			output.lightDir = ObjSpaceLightDir(v.vertex);
			output.uv = v.uv;

			TRANSFER_VERTEX_TO_FRAGMENT(output);

			// Diffuse reflection by four "vertex lights"
			output.vertexLighting = float3(0.0, 0.0, 0.0);
            
			#ifdef VERTEXLIGHT_ON

			float3 worldN = mul((float3x3) unity_ObjectToWorld, SCALED_NORMAL);
			float4 worldPos = mul(unity_ObjectToWorld, v.vertex);

            for (int index = 0; index < 4; index++)
            {    
               float4 lightPosition = float4(unity_4LightPosX0[index], 
               unity_4LightPosY0[index], 
               unity_4LightPosZ0[index], 1.0);
 
               float3 vertexToLightSource = (lightPosition - worldPos).rgb;        
               float3 lightDir = normalize(vertexToLightSource);
               float squaredDistance = dot(vertexToLightSource, vertexToLightSource);
               float attenuation = 1.0 / (1.0 + 
                  unity_4LightAtten0[index] * squaredDistance);
               float3 diffuseReflection = attenuation 
                  * unity_LightColor[index].rgb * _Color.rgb 
                  * max(0.0, dot(output.normalDir, lightDir));

			   output.vertexLighting =
				   output.vertexLighting + diffuseReflection*2;
            }
            #endif
			
            return output;
         }
 
         float4 frag(vertexOutput input) : COLOR
         {
            input.lightDir = normalize(input.lightDir);
			//input.normalDir = tex2D(_NormalMap, input.uv * 10 % 1);
			float atten = LIGHT_ATTENUATION(input);
			float4 tex = tex2D(_MainTex, input.uv) * (_Color + float4(input.vertexLighting, 1.0));
			
 
            float3 diffuseReflection = 
               saturate(dot(input.normalDir,input.lightDir));

            return float4((UNITY_LIGHTMODEL_AMBIENT.rgb * tex.rgb * atten) + (tex.rgb * _LightColor0.rgb * diffuseReflection)*(atten),tex.a + _LightColor0.a*atten);
         }
         ENDCG
      }
 
      Pass {    
         Tags { "LightMode" = "ForwardAdd" } 
            // pass for additional light sources
         Blend One One // additive blending 
 
         CGPROGRAM
 
         #pragma vertex vert  
         #pragma fragment frag
		 #pragma multi_compile_fwdadd
 
         #include "UnityCG.cginc"
		 #include "Lighting.cginc"
		 #include "AutoLight.cginc"
         //uniform float4 _LightColor0; 
            // color of light source (from "Lighting.cginc")
 
         // User-specified properties
         uniform float4 _Color; 
         //uniform float4 _SpecColor; 
         uniform float _Shininess;
		 uniform sampler2D _MainTex;
 
         struct v2f {
            float4 pos : SV_POSITION;
            float3 normal : TEXCOORD0;
			float3 lightDir : TEXCOORD1;
			float2 uv : TEXCOORD2;
			LIGHTING_COORDS(3, 4)
         };
 
         v2f vert(appdata_tan v) 
         {
			v2f o;

            o.pos = mul(UNITY_MATRIX_MVP, v.vertex);
			o.uv = v.texcoord.xy;

			o.lightDir = ObjSpaceLightDir(v.vertex);

			o.normal = v.normal;

			TRANSFER_VERTEX_TO_FRAGMENT(o);
            
			return o;
         }
 
		 float4 frag(v2f i) : COLOR
		 {
			i.lightDir = normalize(i.lightDir);
			 
			float4 tex = tex2D(_MainTex, (i.uv) * 4 % 1);
			tex *= _Color;

			float atten = LIGHT_ATTENUATION(i);

			float3 normal = i.normal;
			float diff = saturate(dot(normal, i.lightDir));

			return float4((tex.rgb * _LightColor0.rgb * diff) * (atten * 2),
				tex.a);
         }
 
         ENDCG
		}


		// End Lighting Shader (below is terrain selection ring)
		Pass{
			Blend SrcAlpha OneMinusSrcAlpha
			Cull Off

			Tags{"Queue" = "Overlay"}

			CGPROGRAM

			// The bounds of the selection area to show.
			uniform float _shaderMinX;
			uniform float _shaderMinZ;
			uniform float _shaderWidth;
			uniform float _shaderLength;

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
				float4 origWorld : TEXCOORD2;
				float3 normal : NORMAL;
			};

			vout ver(vin i) {
				vout o;
				o.vertex = mul(UNITY_MATRIX_MVP, i.vertex + float4(i.normal, 0));
				o.origWorld = mul(unity_ObjectToWorld, i.vertex);
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

				// Enlargened due to normal addition
				// TODO: Make this radial (Future plan).
				if (!(i.origWorld.x >= (_shaderMinX - _shaderWidth/2 - 3.15) && i.origWorld.x <= (_shaderMinX + _shaderWidth/2 + 3.15)
					&& i.origWorld.z >= (_shaderMinZ - _shaderLength/2 - 3.15) && i.origWorld.z <= (_shaderMinZ + _shaderLength/2 + 3.15))) {
					discard;
				}

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
				// TODO: Make this radial. (Future plan)
				if ((1 - abs(tangentx - 0.5)) < (1 - factor) && (1 - abs(binormalx - 0.5)) < (1 - factor))
					discard;

				return float4(1, 0, 0, 0.5);
			}


			ENDCG
		}
	}
}
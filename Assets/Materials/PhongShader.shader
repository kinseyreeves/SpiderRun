﻿// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'
// Upgrade NOTE: replaced '_World2Object' with 'unity_WorldToObject'

// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'
// Upgrade NOTE: replaced '_World2Object' with 'unity_WorldToObject'

Shader "Unlit/PhongShader" {
	Properties{
		_Color("Diffuse Material Color", Color) = (1,1,1,1)
		_SpecColor("Specular Material Color", Color) = (1,1,1,1)
		_Shininess("Shininess", Float) = 10
		_MainTex("Texture", 2D) = "white" {}
	}
		SubShader{
		Pass{
		Tags{ "LightMode" = "ForwardBase" } // pass for 
											// 4 vertex lights, ambient light & first pixel light

		CGPROGRAM
#pragma multi_compile_fwdbase 
#pragma vertex vert
#pragma fragment frag

#include "UnityCG.cginc" 
		uniform float4 _LightColor0;
	// color of light source (from "Lighting.cginc")

	// User-specified properties
	uniform float4 _Color;
	uniform float4 _SpecColor;
	uniform float _Shininess;
	uniform sampler2D _MainTex;

	struct vertexInput {
		float4 vertex : POSITION;
		float3 normal : NORMAL;
		float2 uv : TEXCOORD0;
	};
	struct vertexOutput {
		float4 pos : SV_POSITION;
		float4 posWorld : TEXCOORD0;
		float3 normalDir : TEXCOORD1;
		float3 vertexLighting : TEXCOORD2;
		float2 uv : TEXCOORD3;
		
	};

	vertexOutput vert(vertexInput input)
	{
		vertexOutput output;

		float4x4 modelMatrix = unity_ObjectToWorld;
		float4x4 modelMatrixInverse = unity_WorldToObject;

		output.posWorld = mul(modelMatrix, input.vertex);
		output.normalDir = normalize(
			mul(float4(input.normal, 0.0), modelMatrixInverse).xyz);
		output.pos = mul(UNITY_MATRIX_MVP, input.vertex);

		output.uv = input.uv;

		// Diffuse reflection by four "vertex lights"            
		output.vertexLighting = float3(0.0, 0.0, 0.0);
		#ifdef VERTEXLIGHT_ON
		for (int index = 0; index < 4; index++)
		{
			float4 lightPosition = float4(unity_4LightPosX0[index],
				unity_4LightPosY0[index],
				unity_4LightPosZ0[index], 1.0);



			float3 vertexToLightSource =
				lightPosition.xyz - output.posWorld.xyz;
			float3 lightDirection = normalize(vertexToLightSource);
			float squaredDistance =
				dot(vertexToLightSource, vertexToLightSource);
			float attenuation = 1.0 / (1.0 +
				unity_4LightAtten0[index] * squaredDistance);
			float3 diffuseReflection = attenuation
				* unity_LightColor[index].rgb * _Color.rgb
				* max(0.0, dot(output.normalDir, lightDirection));

			output.vertexLighting =
				output.vertexLighting + diffuseReflection;
		}
#endif
		return output;
	}

	float4 frag(vertexOutput input) : COLOR
	{
		float3 normalDirection = normalize(input.normalDir);
		float3 viewDirection = normalize(
			_WorldSpaceCameraPos - input.posWorld.xyz);
		float3 lightDirection;
		float attenuation;
		float4 tex = tex2D(_MainTex, input.uv) * (_Color + float4(input.vertexLighting, 1.0));

		if (0.0 == _WorldSpaceLightPos0.w) // directional light?
		{
			attenuation = 1.0; // no attenuation
			lightDirection =
				normalize(_WorldSpaceLightPos0.xyz);
		}
		else // point or spot light
		{
			float3 vertexToLightSource =
				_WorldSpaceLightPos0.xyz - input.posWorld.xyz;
			float distance = length(vertexToLightSource);
			attenuation = 1.0 / distance; // linear attenuation 
			lightDirection = normalize(vertexToLightSource);
		}

		float3 ambientLighting =
			UNITY_LIGHTMODEL_AMBIENT.rgb * tex.rgb;

		float3 diffuseReflection =
			attenuation * _LightColor0.rgb * tex.rgb
			* max(0.0, dot(normalDirection, lightDirection));

		float3 specularReflection;
		if (dot(normalDirection, lightDirection) < 0.0)
			// light source on the wrong side?
		{
			specularReflection = float3(0.0, 0.0, 0.0);
			// no specular reflection
		}
		else // light source on the right side
		{
			specularReflection = attenuation * _LightColor0.rgb
				* _SpecColor.rgb * pow(max(0.0, dot(
					reflect(-lightDirection, normalDirection),
					viewDirection)), _Shininess);
		}

		

		return float4(input.vertexLighting + ambientLighting
			+ diffuseReflection + specularReflection, 1.0);
	}
		ENDCG
	}

		Pass{
		Tags{ "LightMode" = "ForwardAdd" }
		// pass for additional light sources
		Blend One One // additive blending 

		CGPROGRAM

#pragma vertex vert  
#pragma fragment frag 
#pragma multi_compile_fwdadd

#include "UnityCG.cginc" 
		uniform float4 _LightColor0;
	// color of light source (from "Lighting.cginc")

	// User-specified properties
	uniform float4 _Color;
	uniform float4 _SpecColor;
	uniform float _Shininess;

	struct vertexInput {
		float4 vertex : POSITION;
		float3 normal : NORMAL;
	};
	struct vertexOutput {
		float4 pos : SV_POSITION;
		float4 posWorld : TEXCOORD0;
		float3 normalDir : TEXCOORD1;
	};

	vertexOutput vert(vertexInput input)
	{
		vertexOutput output;

		float4x4 modelMatrix = unity_ObjectToWorld;
		float4x4 modelMatrixInverse = unity_WorldToObject;

		output.posWorld = mul(modelMatrix, input.vertex);
		output.normalDir = normalize(
			mul(float4(input.normal, 0.0), modelMatrixInverse).xyz);
		output.pos = mul(UNITY_MATRIX_MVP, input.vertex);
		return output;
	}

	float4 frag(vertexOutput input) : COLOR
	{
		float3 normalDirection = normalize(input.normalDir);

		float3 viewDirection = normalize(
			_WorldSpaceCameraPos.xyz - input.posWorld.xyz);
		float3 lightDirection;
		float attenuation;

		if (0.0 == _WorldSpaceLightPos0.w) // directional light?
		{
			attenuation = 1.0; // no attenuation
			lightDirection =
				normalize(_WorldSpaceLightPos0.xyz);
		}
		else // point or spot light
		{
			float3 vertexToLightSource =
				_WorldSpaceLightPos0.xyz - input.posWorld.xyz;
			float distance = length(vertexToLightSource);
			attenuation = 1.0 / distance; // linear attenuation 
			lightDirection = normalize(vertexToLightSource);
		}

		float3 diffuseReflection =
			attenuation * _LightColor0.rgb * _Color.rgb
			* max(0.0, dot(normalDirection, lightDirection));

		float3 specularReflection;
		if (dot(normalDirection, lightDirection) < 0.0)
			// light source on the wrong side?
		{
			specularReflection = float3(0.0, 0.0, 0.0);
			// no specular reflection
		}
		else // light source on the right side
		{
			specularReflection = attenuation * _LightColor0.rgb
				* _SpecColor.rgb * pow(max(0.0, dot(
					reflect(-lightDirection, normalDirection),
					viewDirection)), _Shininess);
		}

		return float4(diffuseReflection
			+ specularReflection, 1.0);
		// no ambient lighting in this pass
	}

		ENDCG
	}

	}
		Fallback "Specular"
}

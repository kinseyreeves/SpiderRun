// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'

// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'

Shader "Custom/FogOfWar" {
	Properties {
		_Color ("Main Color", Color) = (1,1,1,1)
		_MainTex ("Base (RGB) Trans (A)", 2D) = "white" {}
		_FogRadius ("_FogRadius", Float) = 1.0
		_FogMaxRadius("_FogMaxRadius", Float) = 0.5
		_Player_Pos ("_Player_Pos", Vector) = (0,0,0,1)
		
	}
	SubShader {
		Tags { "Queue"="Transparent" "IgnoreProjector" = "True" "RenderType" = "Transparent" }

		LOD 200
		Blend SrcAlpha OneMinusSrcAlpha
		Cull Off
		
		CGPROGRAM
		#pragma surface surf Lambert vertex:vert alpha:blend


		sampler2D _MainTex;
		fixed4 _Color;

		float _FogRadius;
		float _FogMaxRadius;

		float4 _Player_Pos;


		struct Input {
			float2 uv_MainTex;
			float2 location;

		};

		float powerForPos(float4 pos, float2 nearVertex);

		void vert(inout appdata_full vertexData, out Input outData) {

			float4 pos = mul(UNITY_MATRIX_MVP, vertexData.vertex);
			float4 posWorld = mul(unity_ObjectToWorld, vertexData.vertex);

			outData.uv_MainTex = vertexData.texcoord;
			outData.location = posWorld.xz;
		}

		

		void surf (Input input, inout SurfaceOutput o) {
			fixed4 baseColor = tex2D (_MainTex, input.uv_MainTex) * _Color;
			float alpha = (1.0 - powerForPos(_Player_Pos, input.location));

			o.Albedo = baseColor.rgb;
			o.Alpha = alpha;
		}

		float powerForPos(float4 pos, float2 nearVertex) {
			float atten = (_FogRadius - length(pos.xz - nearVertex.xy));

			return (1.0/_FogMaxRadius)*atten/ _FogRadius;
		}

		ENDCG
	}
	FallBack "Transparent/VertexLit"
}

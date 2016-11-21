// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'

// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'

Shader "Custom/FOW2Shader" {


	Properties{
	_Color("Main Color", Color) = (1,1,1,1)
	_MainTex("Base (RGB) Trans (A)", 2D) = "white" {}
	_FogRadius("_FogRadius", Float) = 1.0
	_FogMaxRadius("_FogMaxRadius", Float) = 0.5
	_Player_Pos("_Player_Pos", Vector) = (0,0,0,1)


	}


	SubShader{
		Tags{ "Queue" = "Transparent" }

		Pass{
			

			ZWrite Off
			Cull Off 



			Blend SrcAlpha OneMinusSrcAlpha

			CGPROGRAM
			
			// To read from position
			#pragma target 3.0
			
			#pragma vertex vert  
			#pragma fragment frag 

			float powerForPos(float4 pos, float2 nearVertex);

			sampler2D _MainTex;
			float _FogRadius;
			float _FogMaxRadius;
			fixed4 _Color;


			float4 _Player_Pos;
			
			float4 _Torch1Pos;
			float4 	_Torch2Pos;
			float4  _Torch3Pos;

			struct vertexInput {
				float4 vertex : POSITION;
			};

			struct vertexOutput {
				float4 pos : SV_POSITION;
				float4 posInObjectCoords : TEXCOORD0;
				float3 wpos : TEXCOORD1;
			};

	vertexOutput vert(vertexInput input)
	{
		vertexOutput output;
		output.pos = mul(UNITY_MATRIX_MVP, input.vertex);
		output.posInObjectCoords = input.vertex;
		output.wpos = mul(unity_ObjectToWorld, input.vertex).xyz;
		return output;
	}

	float4 frag(vertexOutput input) : COLOR
	{
		float alpha = (1 - (powerForPos(_Player_Pos, input.pos)));

		float4 output = float4(1.0, 1.0, 1.0, alpha);
		output.rgb = _Color;
		return output; // green
	}

	float powerForPos(float4 pos, float2 nearVertex) {
		float atten = (_FogRadius - length(pos.xz - nearVertex.xy));

		return (1.0 / _FogMaxRadius)*atten / _FogRadius;
	}

		ENDCG
	}
}
}
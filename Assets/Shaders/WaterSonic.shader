Shader "Custom/WaterSonic" {
	Properties {
		_Color ("Color", Color) = (1,1,1,1) 
		_MainTex ("Albedo (RGB)", 2D) = "white" {}
		_NormalTex ("Normal", 2D) = "white" {}
		_Glossiness ("Smoothness", Range(0,1)) = 0.5
		_Metallic ("Metallic", Range(0,1)) = 0.0
		_Ajust ("Ajuste",Range(-2,2))=1
	}
	SubShader {
		Tags { "RenderType"="Opaque" }
		LOD 200
		
		CGPROGRAM
		// Physically based Standard lighting model, and enable shadows on all light types
		#pragma surface surf Standard fullforwardshadows

		// Use shader model 3.0 target, to get nicer looking lighting
		#pragma target 3.0
		float _Ajust;
		sampler2D _MainTex;
		sampler2D _NormalTex;

		struct Input {
			float2 uv_MainTex;
			float2 uv_NormalTex;
		};

		half _Glossiness;
		half _Metallic;
		fixed4 _Color;

		void surf (Input IN, inout SurfaceOutputStandard o) {
			// Albedo comes from a texture tinted by color
			fixed4 c = tex2D (_MainTex, IN.uv_MainTex*_Ajust) * _Color;
			o.Albedo = c.rgb;

			float2 newuv= float2(IN.uv_NormalTex.x,IN.uv_NormalTex.y);
			fixed3 n = UnpackNormal(tex2D (_NormalTex, newuv)); 
			float2 newuv2= float2(IN.uv_NormalTex.x+_Time.x,IN.uv_NormalTex.y);
			fixed3 n2 = UnpackNormal(tex2D (_NormalTex, newuv2)); 
			o.Normal = n*n2;
			// Metallic and smoothness come from slider variables
			o.Metallic = _Metallic;
			o.Smoothness = _Glossiness;
			o.Alpha = c.a;
		}
		ENDCG
	}
	FallBack "Diffuse"
}

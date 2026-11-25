// Reescrito para Unity 2019. Equivale al "Mobile/Particles/VertexLit Blended" de Unity: partícula
// con transparencia, iluminada por vértice más un color emisivo.
Shader "Mobile/Particles/VertexLit Blended" {
Properties {
	_EmisColor ("Emissive Color", Color) = (0.2,0.2,0.2,0)
	_MainTex ("Particle Texture", 2D) = "white" {}
}
SubShader {
	Tags { "Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent" "PreviewType"="Plane" }
	Blend SrcAlpha OneMinusSrcAlpha
	Cull Off ZWrite Off
	Pass {
		Tags { "LightMode"="ForwardBase" }
		CGPROGRAM
		#pragma vertex vert
		#pragma fragment frag
		#include "UnityCG.cginc"
		#include "Lighting.cginc"
		struct appdata { float4 vertex : POSITION; float3 normal : NORMAL; fixed4 color : COLOR; float2 uv : TEXCOORD0; };
		struct v2f { float4 pos : SV_POSITION; fixed4 color : COLOR; float2 uv : TEXCOORD0; };
		sampler2D _MainTex; float4 _MainTex_ST; fixed4 _EmisColor;
		v2f vert (appdata v) {
			v2f o; o.pos = UnityObjectToClipPos(v.vertex); o.uv = TRANSFORM_TEX(v.uv, _MainTex);
			float3 n = UnityObjectToWorldNormal(v.normal);
			fixed3 light = _LightColor0.rgb * max(0, dot(n, _WorldSpaceLightPos0.xyz)) + ShadeSH9(float4(n, 1)) + _EmisColor.rgb;
			o.color = fixed4(v.color.rgb * light, v.color.a);
			return o;
		}
		fixed4 frag (v2f i) : SV_Target { return i.color * tex2D(_MainTex, i.uv); }
		ENDCG
	}
}
}

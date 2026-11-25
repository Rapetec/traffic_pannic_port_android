// Reescrito para Unity 2019. Equivale al "Mobile/Particles/Additive" de Unity: partícula aditiva, color del vértice
// por la textura.
Shader "Mobile/Particles/Additive" {
Properties {
	_MainTex ("Particle Texture", 2D) = "white" {}
}
SubShader {
	Tags { "Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent" "PreviewType"="Plane" }
	Blend SrcAlpha One
	Cull Off Lighting Off ZWrite Off
	Pass {
		CGPROGRAM
		#pragma vertex vert
		#pragma fragment frag
		#include "UnityCG.cginc"
		struct appdata { float4 vertex : POSITION; fixed4 color : COLOR; float2 uv : TEXCOORD0; };
		struct v2f { float4 pos : SV_POSITION; fixed4 color : COLOR; float2 uv : TEXCOORD0; };
		sampler2D _MainTex; float4 _MainTex_ST;
		v2f vert (appdata v) { v2f o; o.pos = UnityObjectToClipPos(v.vertex); o.color = v.color; o.uv = TRANSFORM_TEX(v.uv, _MainTex); return o; }
		fixed4 frag (v2f i) : SV_Target { return i.color * tex2D(_MainTex, i.uv); }
		ENDCG
	}
}
}

// Reescrito para Unity 2019. Equivale al "Mobile/Unlit (Supports Lightmap)" de Unity: textura sin
// luz, multiplicada por el mapa de luz si lo hay (este juego no usa mapas de luz).
Shader "Mobile/Unlit (Supports Lightmap)" {
Properties {
	_MainTex ("Base (RGB)", 2D) = "white" {}
}
SubShader {
	Tags { "RenderType"="Opaque" }
	LOD 100
	Pass {
		CGPROGRAM
		#pragma vertex vert
		#pragma fragment frag
		#pragma multi_compile _ LIGHTMAP_ON
		#include "UnityCG.cginc"
		struct appdata { float4 vertex : POSITION; float2 uv : TEXCOORD0; float2 uv1 : TEXCOORD1; };
		struct v2f { float4 pos : SV_POSITION; float2 uv : TEXCOORD0; float2 lm : TEXCOORD1; };
		sampler2D _MainTex; float4 _MainTex_ST;
		v2f vert (appdata v) { v2f o; o.pos = UnityObjectToClipPos(v.vertex); o.uv = TRANSFORM_TEX(v.uv, _MainTex); o.lm = v.uv1 * unity_LightmapST.xy + unity_LightmapST.zw; return o; }
		fixed4 frag (v2f i) : SV_Target {
			fixed4 c = tex2D(_MainTex, i.uv);
			#ifdef LIGHTMAP_ON
			c.rgb *= DecodeLightmap(UNITY_SAMPLE_TEX2D(unity_Lightmap, i.lm));
			#endif
			return c;
		}
		ENDCG
	}
}
}

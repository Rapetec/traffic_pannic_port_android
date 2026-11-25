// Reescrito para Unity 2019. Equivale al "Mobile/VertexLit" de Unity: luz calculada por vértice.
Shader "Mobile/VertexLit" {
Properties {
	_MainTex ("Base (RGB)", 2D) = "white" {}
}
SubShader {
	Tags { "RenderType"="Opaque" }
	LOD 80
	Pass {
		Tags { "LightMode"="ForwardBase" }
		CGPROGRAM
		#pragma vertex vert
		#pragma fragment frag
		#include "UnityCG.cginc"
		#include "Lighting.cginc"
		struct v2f { float4 pos : SV_POSITION; float2 uv : TEXCOORD0; fixed3 light : COLOR0; };
		sampler2D _MainTex; float4 _MainTex_ST;
		v2f vert (appdata_base v) {
			v2f o; o.pos = UnityObjectToClipPos(v.vertex); o.uv = TRANSFORM_TEX(v.texcoord, _MainTex);
			float3 n = UnityObjectToWorldNormal(v.normal);
			o.light = _LightColor0.rgb * max(0, dot(n, _WorldSpaceLightPos0.xyz)) + ShadeSH9(float4(n, 1));
			return o;
		}
		fixed4 frag (v2f i) : SV_Target { fixed4 c = tex2D(_MainTex, i.uv); c.rgb *= i.light; return c; }
		ENDCG
	}
}
}

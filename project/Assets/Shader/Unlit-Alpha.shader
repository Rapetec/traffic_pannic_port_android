// Reescrito para Unity 2019. Equivale al "Unlit/Transparent" de Unity: textura con transparencia
// (los menús y textos de EZ GUI / SpriteManager lo usan).
Shader "Unlit/Transparent" {
Properties {
	_MainTex ("Base (RGB) Trans (A)", 2D) = "white" {}
}
SubShader {
	Tags { "Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent" }
	LOD 100
	ZWrite Off
	Blend SrcAlpha OneMinusSrcAlpha
	Pass {
		CGPROGRAM
		#pragma vertex vert
		#pragma fragment frag
		#pragma multi_compile_fog
		#include "UnityCG.cginc"
		struct v2f { float4 pos : SV_POSITION; float2 uv : TEXCOORD0; UNITY_FOG_COORDS(1) };
		sampler2D _MainTex; float4 _MainTex_ST;
		v2f vert (appdata_base v) { v2f o; o.pos = UnityObjectToClipPos(v.vertex); o.uv = TRANSFORM_TEX(v.texcoord, _MainTex); UNITY_TRANSFER_FOG(o, o.pos); return o; }
		fixed4 frag (v2f i) : SV_Target { fixed4 c = tex2D(_MainTex, i.uv); UNITY_APPLY_FOG(i.fogCoord, c); return c; }
		ENDCG
	}
}
}

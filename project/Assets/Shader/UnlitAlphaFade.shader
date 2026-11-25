// Reescrito para Unity 2019. Shader propio del juego: textura con transparencia multiplicada por
// _Color, que los scripts cambian para hacer fundidos.
Shader "Custom/UnlitAlphaFade" {
Properties {
	_Color ("Color Tint", Color) = (1,1,1,1)
	_MainTex ("Base (RGB) Alpha (A)", 2D) = "white" {}
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
		#include "UnityCG.cginc"
		struct v2f { float4 pos : SV_POSITION; float2 uv : TEXCOORD0; };
		sampler2D _MainTex; float4 _MainTex_ST; fixed4 _Color;
		v2f vert (appdata_base v) { v2f o; o.pos = UnityObjectToClipPos(v.vertex); o.uv = TRANSFORM_TEX(v.texcoord, _MainTex); return o; }
		fixed4 frag (v2f i) : SV_Target { return tex2D(_MainTex, i.uv) * _Color; }
		ENDCG
	}
}
}

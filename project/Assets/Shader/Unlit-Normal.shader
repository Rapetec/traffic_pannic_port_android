// Reescrito para Unity 2019 (AssetRipper solo exporta un sustituto vacío de los shaders compilados).
// Equivale al "Unlit/Texture" de Unity: textura sin luz.
Shader "Unlit/Texture" {
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
		#pragma multi_compile_fog
		#include "UnityCG.cginc"
		struct v2f { float4 pos : SV_POSITION; float2 uv : TEXCOORD0; UNITY_FOG_COORDS(1) };
		sampler2D _MainTex; float4 _MainTex_ST;
		v2f vert (appdata_base v) { v2f o; o.pos = UnityObjectToClipPos(v.vertex); o.uv = TRANSFORM_TEX(v.texcoord, _MainTex); UNITY_TRANSFER_FOG(o, o.pos); return o; }
		fixed4 frag (v2f i) : SV_Target { fixed4 c = tex2D(_MainTex, i.uv); UNITY_APPLY_FOG(i.fogCoord, c); UNITY_OPAQUE_ALPHA(c.a); return c; }
		ENDCG
	}
}
}

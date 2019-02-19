Shader "Custom/Sprite/PixelPalette"
{
   Properties
	{
		[PerRendererData] _MainTex ("Sprite Texture", 2D) = "white" {}
		_Pallete("Pallete",2D) = "transparent"{} //调色板纹理
		_Color ("Tint", Color) = (1,1,1,1)
		[MaterialToggle] PixelSnap ("Pixel snap", Float) = 0
        _Speed("Speed", Range(1.0,30)) = 15
        _ActiveFlikcer("ActiveFlikcer",Int) = 0
		_MaxPalette("MaxPalette",Int) = 1  //最大调色板数量，根据调色板纹理来确定
		_CurrentPalette("CurrentPalette",Int) = 0 //当前调色板
	}

	SubShader
	{
		Tags
		{ 
			"Queue"="Transparent" 
			"IgnoreProjector"="True" 
			"RenderType"="Transparent" 
			"PreviewType"="Plane"
			"CanUseSpriteAtlas"="True"
		}

		Cull Off
		Lighting Off
		ZWrite Off
		Blend One OneMinusSrcAlpha

		Pass
		{
		CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma multi_compile _ PIXELSNAP_ON
			#include "UnityCG.cginc"
			
			struct appdata_t
			{
				float4 vertex   : POSITION;
				float4 color    : COLOR;
				float2 texcoord : TEXCOORD0;
			};

			struct v2f
			{
				float4 vertex   : SV_POSITION;
				fixed4 color    : COLOR;
				float2 texcoord  : TEXCOORD0;
			};
			
			fixed4 _Color;
            float _Speed;
            float _ActiveFlikcer;


			v2f vert(appdata_t IN)
			{
				v2f OUT;
				OUT.vertex = UnityObjectToClipPos(IN.vertex);
				OUT.texcoord = IN.texcoord;
				OUT.color = IN.color * _Color;
				#ifdef PIXELSNAP_ON
				OUT.vertex = UnityPixelSnap (OUT.vertex);
				#endif

				return OUT;
			}

			sampler2D _MainTex;
			sampler2D _AlphaTex;
			sampler2D _Pallete;
			float _AlphaSplitEnabled;
			half _MaxPalette;
			half _CurrentPalette;

			fixed4 SampleSpriteTexture (float2 uv)
			{
				fixed4 color = tex2D (_MainTex, uv);

#if UNITY_TEXTURE_ALPHASPLIT_ALLOWED
				if (_AlphaSplitEnabled)
					color.a = tex2D (_AlphaTex, uv).r;
#endif //UNITY_TEXTURE_ALPHASPLIT_ALLOWED

				return color;
			}

			fixed4 frag(v2f IN) : SV_Target
			{
				fixed4 c = SampleSpriteTexture(IN.texcoord);
				//切换调色板，这里用G通道的Index来制作的调色板
				float paletteIndex = 1 - (_CurrentPalette / _MaxPalette);
				fixed4 swapCol = tex2D(_Pallete,float2(c.g, paletteIndex));
				fixed4 final = lerp(c, swapCol, swapCol.a) * IN.color;
				if(_ActiveFlikcer == 1){
					half curSin = abs(sin(_Time.a * _Speed));
					half alpha;
					if(curSin > 0.5){
						alpha =  1 ;
					}else{
						alpha =  0 ;
					}
					if(final.a == 1){
						final.a = alpha;
					}
				}
				final.a = c.a;
				final.rgb *= c.a;
				return final;
             
			}
		ENDCG
		}
	}
}

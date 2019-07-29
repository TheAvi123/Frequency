Shader "Sprites/VerticalGradient"
{
    Properties
    {
		[PerRendererData] _MainTex("Sprite Texture", 2D) = "white" {}
		_ColorTop("Top Color", Color) = (1,1,1,1)
		_ColorBot("Bot Color", Color) = (1,1,1,1)
    }

    SubShader
    {
		Cull Off

		Tags
		{
			"Queue" = "Transparent"
		}

        Pass
        {
			Stencil {
				Ref 1  //Customize this value
				Comp Equal //Customize the compare function
				Pass Keep
			}

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

			fixed4 _ColorTop;
			fixed4 _ColorBot;

			struct v2f {
				float4 pos : SV_POSITION;
				fixed4 col : COLOR;
			};

			v2f vert(appdata_base v)
			{
				v2f o;
				o.pos = UnityObjectToClipPos(v.vertex);
				o.col = lerp(_ColorBot, _ColorTop, v.texcoord.y);
				return o;
			}

			float4 frag(v2f i) : COLOR{
				float4 c = i.col;
				c.a = 1;
				return c;
			}

			ENDCG
        }
    }
}

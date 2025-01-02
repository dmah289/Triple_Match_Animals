Shader "Rabbit/GlassOutline"
{
    Properties
    {
        _MainTex ("Sprite Texture", 2D) = "white" {}
        _BorderThickness ("Border Thickness", Range(0.001, 0.1)) = 0.02
        _ShineWidth ("Shine Width", Range(0.01, 0.2)) = 0.03
        _ShineSpeed ("Shine Speed", Range(0.1, 5.0)) = 1.0
        _ShineColor ("Shine Color", Color) = (1, 1, 1, 1)
    }
    SubShader
    {
        Tags { "Queue"="Transparent" "RenderType"="Transparent" }
        LOD 200
        Cull Off
        ZWrite Off
        Blend SrcAlpha OneMinusSrcAlpha

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            float _BorderThickness;
            float _ShineWidth;
            float _ShineSpeed;
            fixed4 _ShineColor;

            v2f vert(appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            float GetBorderMask(float2 uv)
            {
                // Tính khoảng cách từ pixel tới các viền
                float2 dist = abs(uv - 0.5) * 2.0;
                return 1.0 - max(dist.x, dist.y);
            }

            fixed4 frag(v2f i) : SV_Target
            {
                // Lấy màu gốc của texture
                fixed4 texColor = tex2D(_MainTex, i.uv);

                // Tạo viền dựa trên khoảng cách tới các cạnh
                float borderMask = smoothstep(_BorderThickness, _BorderThickness * 0.8, GetBorderMask(i.uv));

                // Vệt sáng chéo chạy xung quanh viền theo thời gian
                float diagonalLine = frac((_Time.y * _ShineSpeed) + (i.uv.x + i.uv.y) * 0.5);
                float shine = smoothstep(1.0 - _ShineWidth, 1.0, 1.0 - diagonalLine);

                // Kết hợp viền và vệt sáng
                fixed4 shineEffect = _ShineColor * shine * borderMask;

                // Pha trộn với texture gốc
                return texColor + shineEffect;
            }
            ENDCG
        }
    }
}




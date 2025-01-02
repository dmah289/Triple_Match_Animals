Shader "Rabbit/MotionBackground"
{
    Properties
    {
        _MainTex("Texture", 2D) = "white" {}               // Texture chính
        _Opacity("Opacity", Range(0, 1)) = 0.5            // Độ trong suốt
        _Speed("Speed", Range(-2, 2)) = 0.5               // Tốc độ di chuyển
        _Scale("Scale", float) = 1                        // Độ phóng to/thu nhỏ texture
        _Color1("Gradient Color 1", Color) = (1, 1, 0, 1) // Màu bắt đầu (vàng)
        _Color2("Gradient Color 2", Color) = (1, 1, 1, 1) // Màu kết thúc (trắng)
        _GradientScale("Gradient Scale", Range(0.1, 10)) = 1.0 // Độ dài của gradient
    }

    SubShader
    {
        Tags
        {
            "Queue" = "Transparent"
            "IgnoreProjector" = "True"
            "RenderType" = "Transparent"
        }
        Blend SrcAlpha OneMinusSrcAlpha
        Cull Off
        Lighting Off
        ZWrite Off

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata_t
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
            float4 _MainTex_ST;
            float4 _Color1;
            float4 _Color2;
            float _Opacity;
            float _Speed;
            float _Scale;
            float _GradientScale;

            v2f vert(appdata_t v)
            {
                v2f o;

                // Chuyển vị trí vào clip space
                o.vertex = UnityObjectToClipPos(v.vertex);

                // Lấy tỷ lệ khung hình để tránh giãn hình
                float aspectRatio = _ScreenParams.x / _ScreenParams.y;

                // Điều chỉnh UV theo tỷ lệ và scale
                o.uv = v.uv;
                o.uv.x *= aspectRatio;
                o.uv *= _Scale;

                // Offset UV để tạo chuyển động vô tận theo góc 30 độ
                o.uv += frac(float2(_Time.y * _Speed * 0.866, _Time.y * _Speed * 0.5));

                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                // Gradient dựa trên trục Y và GradientScale để điều chỉnh độ dài
                float gradientFactor = saturate(i.uv.y * _GradientScale);
                fixed4 gradientColor = lerp(_Color1, _Color2, gradientFactor);

                // Đọc texture trắng đen
                fixed4 texColor = tex2D(_MainTex, frac(i.uv));

                // Kết hợp texture với gradient
                fixed4 finalColor = texColor * gradientColor;

                // Điều chỉnh độ trong suốt
                finalColor.a *= texColor.a * _Opacity;

                return finalColor;
            }
            ENDCG
        }
    }
}

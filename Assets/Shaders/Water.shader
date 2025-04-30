Shader "Unlit/Water"
{
    Properties
    {
        [HDR] _Color("Color", Color) = (1,1,1,1)
        _DepthFactor("Depth Factor", float) = 1.0
        //_DepthPow("Depth Pow", float) = 1.0
        
        _NoiseTex("Noise Texture", 2D) = "white" {}
        _WaveSpeed("Wave Speed", float) = 1
        _WaveAmp("Wave Amp", float) = 0.2
        _ExtraHeight("Extra Height", float) = 0.0
    }
    SubShader
    {
        Tags { "RenderType"="Transparent" "IgnoreProjector"="True" "Queue" = "Transparent" }
        Blend SrcAlpha OneMinusSrcAlpha
        LOD 100

        Pass
        {
            Tags { "LightMode" = "ForwardBase" }

            CGPROGRAM
            
            #pragma vertex vert
            #pragma fragment frag
            #pragma multi_compile_fwdbase nolightmap nodirlightmap nodynlightmap novertexlight


            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float4 texCoord : TEXCOORD0;
                float3 normal : NORMAL;
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;
                float4 texCoord : TEXCOORD0;
                float normal : TEXCOORD1;
            };

            float4 _Color;
            float _DepthFactor;
            //fixed _DepthPow;

            sampler2D _NoiseTex;
            float _WaveSpeed;
            float _WaveAmp;
            float _ExtraHeight;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);

                float noiseSample = tex2Dlod(_NoiseTex, float4(v.texCoord.xy, 0, 0));
                o.vertex.y += sin(_Time * _WaveSpeed * noiseSample) * _WaveAmp + _ExtraHeight;

                o.texCoord = v.texCoord;

                o.normal = dot(normalize(v.normal), normalize(ObjSpaceViewDir(v.vertex)));

                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 col = _Color;
                col.w = _Color.w-(i.normal*_DepthFactor);

                return col;
            }
            ENDCG
        }
    }
}

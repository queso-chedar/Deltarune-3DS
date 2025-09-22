Shader "Custom/EmissionGlobalIlluminationShader"
{
    Properties
    {
        _EmissionColor ("Emission Color", Color) = (0,0,0,1)
        _EmissionMap ("Emission Map", 2D) = "white" {}
        _EmissionIntensity ("Emission Intensity", Range(0, 10)) = 1.0

        _GlobalIlluminationMode ("Global Illumination", Int) = 0 // 0 for Realtime, 1 for Baked
    }

    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 200

        Pass {
            BindChannels {
                Bind "Vertex", vertex
                Bind "normal", normal
                Bind "texcoord1", texcoord0 // lightmap uses 2nd uv
                Bind "texcoord", texcoord1 // main uses 1st uv
            }

            SetTexture [unity_Lightmap] {
                matrix [unity_LightmapMatrix]
                combine texture * texture alpha DOUBLE
            }

            SetTexture [_MainTex] {
                combine texture * primary
            }

			SetTexture [_EmissionMap] {
				constantColor [_EmissionColor]
    			combine texture * constant
			}
        }
        
        CGPROGRAM
        #pragma surface surf Lambert addshadow fullforwardshadows

        sampler2D _MainTex;
        fixed4 _Color;

        sampler2D _EmissionMap;
        fixed4 _EmissionColor;
        float _EmissionIntensity;
        int _GlobalIlluminationMode;

        struct Input
        {
            float2 uv_MainTex;
            float2 uv_EmissionMap;
        };

        void surf(Input IN, inout SurfaceOutput o)
        {
            // Emission with adjustable intensity
            fixed4 emission = tex2D(_EmissionMap, IN.uv_EmissionMap) * _EmissionColor;
            o.Emission = emission.rgb * _EmissionIntensity;

            // Global Illumination handling
            if (_GlobalIlluminationMode == 0)
            {
                o.Emission *= UNITY_LIGHTMODEL_AMBIENT.rgb;
            }
        }
        ENDCG
    }

    Fallback "Diffuse"
}

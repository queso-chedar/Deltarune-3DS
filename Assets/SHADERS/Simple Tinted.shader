Shader "3DS Shaders/Simple Tinted" {
    Properties {
        _MainTex ("Base (RGB)", 2D) = "white" {}
        _Color ("Tint Color", Color) = (1,1,1,1)
    }

    SubShader {
        Tags { "RenderType"="Opaque" }
        LOD 80

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
                combine texture * previous QUAD
            }
            SetTexture [_MainTex] {
                constantColor [_Color]
                combine constant * previous
            }
        }
    }
}

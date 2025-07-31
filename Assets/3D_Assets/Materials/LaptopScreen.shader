Shader "Custom/LaptopScreen"
{
    Properties
    {
        _MainTex ("Game Texture", 2D) = "black" {}
        _EmissionStrength ("Emission Strength", Range(0, 2)) = 1
        _Brightness ("Brightness", Range(0, 2)) = 1
        _Contrast ("Contrast", Range(0, 2)) = 1
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 200

        CGPROGRAM
        #pragma surface surf Standard fullforwardshadows

        sampler2D _MainTex;
        float _EmissionStrength;
        float _Brightness;
        float _Contrast;

        struct Input
        {
            float2 uv_MainTex;
        };

        void surf (Input IN, inout SurfaceOutputStandard o)
        {
            // Sample the texture
            fixed4 c = tex2D(_MainTex, IN.uv_MainTex);
            
            // Apply brightness and contrast
            c.rgb = ((c.rgb - 0.5) * _Contrast + 0.5) * _Brightness;
            
            // Set albedo and emission for screen effect
            o.Albedo = c.rgb * 0.2; // Dim albedo for screen effect
            o.Emission = c.rgb * _EmissionStrength; // Make it glow like a screen
            o.Metallic = 0;
            o.Smoothness = 0.8;
            o.Alpha = c.a;
        }
        ENDCG
    }
    FallBack "Diffuse"
}

Shader "Custom/FullbrightTransparent" 
{
    Properties 
    {
        // This gets you a color picker in the inspector
        _Color ("Main Color", Color) = (1,1,1,1)

        // This gets you a texture picker in the inspector. Supports textures
        // with alpha channel.
        _MainTex ("Base (RGB) Trans (A)", 2D) = "white" {}
	}
    SubShader 
    {
        // These tags do:
        // Queue=Transparent: Render this object as part of the transparent queue, after all opaque objects
        // have been rendered first, and renders in back to front order.
        // IgnoreProjector: This object is ignored by projectors, useful for semi-transparent objects.
        // Rendertype: Enables alpha transparency rendering for this object.
        Tags { "Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent" }
        
        // The Level of Detail property defines what hardware will be enabled to render this material. Lower
        // LODs enable cheaper/less-capable hardware. More complex shaders get higher LOD values.
        LOD 200

        // Disable z-writing, so rendered pixels are not occluded by this object.
        ZWrite Off
        
        // Set the specific alpha blending state. There are several states for different effects 
        // (http://docs.unity3d.com/Documentation/Components/SL-Blend.html)
        Blend SrcAlpha OneMinusSrcAlpha 
                                        
        CGPROGRAM

        // This pragma tells the shader to execute the 'NoLighting' function as the lighting model,
        // instead of the default 'Lambert' or 'Phong'. Use the 'NoLighting' function for full-bright
        // objects, or replace it with 'Lambert' for more common lighting.
        #pragma surface surf NoLighting

        // These declarations enable us to use the inspector's color an texture as properties in the 
        // shader functions.
        float4 _Color;
        sampler2D _MainTex;

        // The input structure defines that we will use the texture-coordinate properties of every
        // vertex that is passed into the shader (you can enable position and per-vertex color data as
        // well, if your shader uses that.)
        struct Input 
        {
                float2 uv_MainTex;
        };

        // This function is the surface shader. It computes the surface properties for every surface
        // that is rendered with this shader. This function operates at vertex level.
        // @param IN The Input information for every vertex passed to the shader. Contains the texture coordinates
        // @param o The output structure that contains the surface's final color information.
        void surf (Input IN, inout SurfaceOutput o) 
        {
            // We retrieve a texel (using the tex2D function) from the texture (_MainTex) at the passed UV coordinates
            // (IN.uv_MainTex) and store the color in c.
            half4 c = tex2D (_MainTex, IN.uv_MainTex);

            // Multiply the texel color (c.rgb) by the inspector color (_Color) and store as the diffuse component of
            // the final surface color (o.Albedo).
            o.Albedo = c.rgb * _Color.rgb;

            // Copy the texture's alpha color (from the alpha channel) and store it as the surface's final surface 
            // alpha.
            o.Alpha = c.a;                  
        }

        // This function defines the lighting model. This specific implementation strips all lighting information
        // to make all rendered pixels full-bright (useful for in-game pickups/glows and the like). This function
        // operates at pixel level.
        // @param s The SurfaceOutput information from the 'surf' function.
        // @param lightDir The light's direction we can use to calculate light-surface interaction with.
        // @param atten The light's attenuation factors (attenuation is complex, outside the scope of this shader. Lots
        // of information on the webs).
        fixed4 LightingNoLighting(SurfaceOutput s, fixed3 lightDir, fixed atten)
        {
            // Declare the variable that will store the final pixel color,
            fixed4 c;
            // Copy the diffuse color component from the SurfaceOutput to the final pixel.
            c.rgb = s.Albedo; 
            // Copy the alpha component from the SurfaceOutput to the final pixel.
            c.a = s.Alpha;
            return c;
        }
        
        ENDCG
    } 
    FallBack "Diffuse"
}
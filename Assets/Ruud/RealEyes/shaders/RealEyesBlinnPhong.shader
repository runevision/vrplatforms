Shader "RealEyes/RealEyesBlinnPhong" {
	Properties {
		_IrisScale ("Scale", Range (.5, 1.5)) = 1
		_Dilation ("Dilation", Range (-.5, 2)) = 0
		_Parallax ("Eye Depth", Range (0, .2)) = 0.05
		_SpecStr ("Glint Strength", Range (0, 10)) = 5
		_SpecPower ("Glint Size", Range (0, 3)) = 1
		_Color ("Iris Tint", Color) = (1,1,1,1)
		_Color2 ("Eyeball Tint", Color) = (.8,.8,.8,1)
		_MainTex ("Eye Texture", 2D) = "white" {}
		_BumpMap ("Iris Normal Map", 2D) = "bump" {}
		_BumpMap2 ("Cornia Normal Map", 2D) = "bump" {}
		_Masks ("IrisSpecMask(R)Iris Heightmap(G)CorniaSpecMask(B)", 2D) = "black" {}
		_Cube ("Cubemap Reflection", CUBE) = "black" {}
		_Samples("Depth Samples", Int) = 10
	}
	SubShader {
		Tags { "RenderType"="Opaque" }

	

		CGPROGRAM
	    #pragma surface surf BlinnPhongCustom fullforwardshadows vertex:vert
		#pragma target 3.0
		
		half4 LightingBlinnPhongCustom (SurfaceOutput s, half3 lightDir, half3 viewDir, half atten) {
			half3 h = normalize (lightDir + viewDir);
			half diff = max (0, dot (s.Normal, lightDir));

			float nh = max (0, dot (s.Normal, h));
			float spec = pow (nh, s.Specular*128.0) * s.Gloss;

			half4 c;
			c.rgb = (s.Albedo * _LightColor0.rgb * diff + _LightColor0.rgb * spec) * (atten);
			c.a = s.Alpha;
			return c;
		}
		
		
	    struct Input {
	        float2 uv_MainTex;
	        float2 uv_BumpMap;
	        float2 uv_Masks;
	        float3 viewDir;
	    };
	    
	    sampler2D _MainTex;
	    sampler2D _BumpMap;
	    sampler2D _Masks;
	    half _Parallax;
	    half _IrisScale;
	    fixed _Dilation;
	    fixed4 _Color;
	    fixed4 _Color2;
	    int _Samples;
	    
		void vert (inout appdata_full v) {
			float2 uv = v.texcoord.xy - .5;
			uv *= _IrisScale;
			v.texcoord.xy = uv + .5;
		}
	    
	    void surf (Input IN, inout SurfaceOutput o) {
	    	
	    	//Do Parallax
			half parallaxLimit = -length( IN.viewDir.xy ) / IN.viewDir.z;
			parallaxLimit *= _Parallax;

			half2 vOffsetDir = normalize( IN.viewDir.xy );
			half2 vMaxOffset = vOffsetDir * parallaxLimit;
			fixed stepSize = 1.0 / (float)_Samples;

			half2 dx = ddx( IN.uv_MainTex);
			half2 dy = ddy( IN.uv_MainTex);

			half fCurrRayHeight = 1.0;
			half2 vCurrOffset = float2( 0, 0 );
			half2 vLastOffset = float2( 0, 0 );

			half fLastSampledHeight = 1;
			half fCurrSampledHeight = 1;

			int nCurrSample = 0;
			while ( nCurrSample < _Samples )
			{
				fCurrSampledHeight = tex2D( _Masks, IN.uv_MainTex + vCurrOffset, dx, dy ).g;

				if ( fCurrSampledHeight > fCurrRayHeight )
				{
					float delta1 = fCurrSampledHeight - fCurrRayHeight;
					float delta2 = ( fCurrRayHeight + stepSize ) - fLastSampledHeight;

					float ratio = delta1/(delta1+delta2);

					vCurrOffset = (ratio) * vLastOffset + (1.0-ratio) * vCurrOffset;
					nCurrSample = _Samples + 1;
				}
				else
				{
					nCurrSample++;
					fCurrRayHeight -= stepSize;
					vLastOffset = vCurrOffset;
					vCurrOffset += stepSize * vMaxOffset;
					fLastSampledHeight = fCurrSampledHeight;
				}
			}
	    	
			half2 uv = IN.uv_MainTex + vCurrOffset - .5;
	        half pupil = saturate(length(uv)/ 0.14);
			uv *= lerp(1.0, pupil, _Dilation);
	        uv += .5;
	    	
	    	half4 masks = tex2D( _Masks, uv);
	    	half4 albido = tex2D (_MainTex, uv);
	    	albido.rgb *= (_Color * albido.a) + (_Color2 * (1-albido.a));
	    	
	        o.Albedo = albido.rgb;
	        o.Specular = .25;
	        o.Gloss = .5 * masks.g;
	        //o.Smoothness = .5 * masks.r; 
	        o.Normal = UnpackNormal (tex2D (_BumpMap, uv));
	    }
	    ENDCG
	    
		
		Blend One One
        ZWrite Off
        
	    CGPROGRAM
	    #pragma surface surf BlinnPhongCustom noambient novertexlights
		#pragma target 3.0

		half4 LightingBlinnPhongCustom (SurfaceOutput s, half3 lightDir, half3 viewDir, half atten) {
			half3 h = normalize (lightDir + viewDir);
			half diff = max (0, dot (s.Normal, lightDir));

			float nh = max (0, dot (s.Normal, h));
			float spec = pow (nh, s.Specular*128.0) * s.Gloss;
	
			half4 c;
			c.rgb = (s.Albedo * _LightColor0.rgb * diff + _LightColor0.rgb * spec) * (atten);
			c.a = s.Alpha;
			return c;
		}
		
	
	    struct Input {
	        float2 uv_BumpMap2;
	        float3 viewDir;
	        float3 worldRefl;
	        INTERNAL_DATA
	    };
	    
	    //sampler2D _MainTex;
	    sampler2D _BumpMap2;
	    half _SpecStr;
	    half _SpecPower;
	    samplerCUBE _Cube;
	    
	    void surf (Input IN, inout SurfaceOutput o) {
	        o.Albedo = 0;
	        o.Specular = _SpecPower;
	        o.Gloss = _SpecStr;
	        o.Normal = UnpackNormal (tex2D (_BumpMap2, IN.uv_BumpMap2));
	        
	        half rim = 1.0 - saturate(dot (normalize(IN.viewDir), o.Normal));
	        o.Emission = (pow (rim, 2.5) * .4) + (texCUBE (_Cube, WorldReflectionVector (IN, o.Normal)).rgb * rim);
	    }
	    ENDCG

	
	}

	
	FallBack "Diffuse"
}

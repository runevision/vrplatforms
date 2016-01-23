Shader "RealEyes/RealEyesLegacy" {
    Properties {
      _IrisScale ("Scale", Range (.5, 1.5)) = 1
      _Dilation ("Dilation", Range (-.5, 2)) = 0
      _Parallax ("Eye Depth", Range (0, .2)) = 0.05
      _SpecStr ("Glint Strength", Range (0, 10)) = 2
      _SpecPower ("Glint Size", Range (32, 1024)) = 256
      _Color ("Iris Tint", Color) = (1,1,1,1)
      _Color2 ("Eyeball Tint", Color) = (.8,.8,.8,1)
      _MainTex ("Eye Texture", 2D) = "white" {}
	  _BumpMap ("Iris Normal Map", 2D) = "bump" {}
	  _BumpMap2 ("Cornia Normal Map", 2D) = "bump" {}
	  _Masks ("IrisSpecMask(R)Iris Heightmap(G)CorniaSpecMask(B)", 2D) = "black" {}
	  _Cube ("Cubemap Reflection", CUBE) = "black" {}
      _MinSamples("Min Samples", Int) = 6
      _MaxSamples("Max Samples", Int) = 12
    }

    SubShader {
   		Tags { "RenderType"="Opaque" "Queue"="Geometry" }
    	Pass {
		
	    	Tags {"LightMode" = "ForwardBase"}
            CGPROGRAM
                #pragma vertex vert
                #pragma fragment frag
                #include "HLSLSupport.cginc"
				#include "UnityShaderVariables.cginc"
                #pragma multi_compile_fwdbase
                #pragma exclude_renderers flash
                #pragma target 3.0
                #pragma glsl
                #include "UnityCG.cginc"
                #include "AutoLight.cginc"

                struct v2f
                {
                    float4 pos : SV_POSITION;
					float2 uv : TEXCOORD0;
					float3 normal : TEXCOORD1;
					float3 tangent : TEXCOORD2;
					float3 binormal : TEXCOORD3;
					fixed3 lightDir : TEXCOORD4;
					fixed3 vlight : TEXCOORD5;
					float3 viewDir : TEXCOORD6;
					float3 viewDirW : TEXCOORD7;
                    LIGHTING_COORDS(8,9) 
                }; 
				
				fixed _IrisScale;
				
                v2f vert (appdata_tan v)
                {
                    v2f o;
                    
    				o.pos = mul(UNITY_MATRIX_MVP, v.vertex);
    				
   				    float2 uv = v.texcoord.xy - .5;
    				uv *= _IrisScale;
					o.uv = uv + .5;
    				
					float3 viewDir = ObjSpaceViewDir(v.vertex);
					float3 worldRefl = mul ((float3x3)_Object2World, viewDir);
					
					TANGENT_SPACE_ROTATION;
					
  					o.lightDir = mul (rotation, ObjSpaceLightDir(v.vertex));
  					o.viewDir = mul(rotation, ObjSpaceViewDir(v.vertex));
  					
  					o.viewDirW = WorldSpaceViewDir(v.vertex);
  					o.tangent = normalize( mul( _Object2World, v.tangent ).xyz );
  					o.normal = normalize( mul( float4( v.normal, 0.0 ), _World2Object ).xyz );
  					o.binormal = cross(o.normal, o.tangent) * v.tangent.w;
  					float3 worldN = mul((float3x3)_Object2World, SCALED_NORMAL);
				    float3 shlight = ShadeSH9 (float4(worldN,1.0));
				  	o.vlight = shlight;
  					
                    TRANSFER_VERTEX_TO_FRAGMENT(o);
                    return o;
                }

                sampler2D _MainTex;
                sampler2D _BumpMap;
                sampler2D _BumpMap2;
                sampler2D _Masks;
                samplerCUBE _Cube;
                fixed4 _Color;
                fixed4 _Color2;
                fixed4 _LightColor0;
                half _SpecStr;
                half _SpecPower;
                half _Parallax;
                fixed _Dilation;
                int _MinSamples;
                int _MaxSamples;
               	
                
                float4 frag(v2f i) : COLOR
                {

                	//half3 nViewDirT = normalize(i.viewDir);
                    half3 h = normalize(i.lightDir + i.viewDir);
                	
                	half parallaxLimit = -length( i.viewDir.xy ) / i.viewDir.z;
					parallaxLimit *= _Parallax;
					
					half2 vOffsetDir = normalize( i.viewDir.xyz );
					half2 vMaxOffset = vOffsetDir * parallaxLimit;
					
					int numSamples = (int)lerp( _MaxSamples, _MinSamples, dot( normalize(i.viewDir), i.normal ) );
					fixed stepSize = 1.0 / (float)numSamples;
					
					half2 dx = ddx( i.uv );
					half2 dy = ddy( i.uv );
					
					half fCurrRayHeight = 1.0;
					half2 vCurrOffset = float2( 0, 0 );
					half2 vLastOffset = float2( 0, 0 );
					
					half fLastSampledHeight = 1;
					half fCurrSampledHeight = 1;
			
					int nCurrSample = 0;
					while ( nCurrSample < numSamples )
					{
					  fCurrSampledHeight = tex2D( _Masks, i.uv + vCurrOffset, dx, dy ).g;
					  
					  if ( fCurrSampledHeight > fCurrRayHeight )
					  {
					    float delta1 = fCurrSampledHeight - fCurrRayHeight;
					    float delta2 = ( fCurrRayHeight + stepSize ) - fLastSampledHeight;
					
					    float ratio = delta1/(delta1+delta2);
					
					    vCurrOffset = (ratio) * vLastOffset + (1.0-ratio) * vCurrOffset;
					    nCurrSample = numSamples + 1;
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
           
					half2 uv = i.uv + vCurrOffset - .5;
                    half pupil = saturate(length(uv)/ 0.14);
					uv *= lerp(1.0, pupil, _Dilation);
                    uv += .5;
	
  					fixed4 albido = tex2D(_MainTex, uv);
  					half4 masks = tex2D( _Masks, uv);
  					
  					albido.rgb *= (_Color * albido.a) + (_Color2 * (1-albido.a));
                	half3 n = normalize(UnpackNormal( tex2D(_BumpMap, uv)));
                	half3 n2 = UnpackNormal( tex2D(_BumpMap2, i.uv));
  					
                    float NdotL = max(0,dot(n, i.lightDir));
                    float NdotH = max(0,dot(n, h));
                    float N2dotH = max(0,dot(n2, h));
                    float N2dotV = 1.0 - saturate(dot (normalize(i.viewDir), n2));
                    
                    half rim = pow(N2dotV, 4);
                    
                    float3 spec = pow(NdotH, 40) * masks.r * _LightColor0.rgb;
					spec += pow(N2dotH, _SpecPower) * _SpecStr * _LightColor0.rgb;
                    
                    float atten = LIGHT_ATTENUATION(i);
                    
					float3 normalW = (i.tangent * n2.x) + (i.binormal * n2.y) + (i.normal * n2.z);
					float3 reflection = texCUBE(_Cube, reflect(-i.viewDirW, normalW)) * N2dotV;
					
                    fixed4 c;
                    c.rgb = ((albido * _LightColor0.rgb * NdotL)  + spec) * (atten) + ((reflection + rim) * masks.b);
					c.rgb += albido * i.vlight;
                    c.a = 1;
                
                    return c;
                }
            ENDCG
        }
        
		Pass {
	    	Tags {"LightMode" = "ForwardAdd"}
	    	ZWrite Off Blend One One Fog { Color (0,0,0,0) }
            CGPROGRAM
                #pragma vertex vert
                #pragma fragment frag
                #include "HLSLSupport.cginc"
				#include "UnityShaderVariables.cginc"
                #pragma multi_compile_fwdadd
                #pragma exclude_renderers flash
                #pragma target 3.0
                #pragma glsl
                #define UNITY_PASS_FORWARDADD
                #include "UnityCG.cginc"
                #include "AutoLight.cginc"

                struct v2f
                {
                    float4 pos : SV_POSITION;
					float2 uv : TEXCOORD0;
					fixed3 lightDir : TEXCOORD1;
					float3 viewDir : TEXCOORD2;
                    float3 normal : TEXCOORD3;
                    LIGHTING_COORDS(4,5) 
                }; 
				
				fixed _IrisScale;
				
                v2f vert (appdata_tan v)
                {
                    v2f o;
                    
    				o.pos = mul(UNITY_MATRIX_MVP, v.vertex);
    				
   				    float2 uv = v.texcoord.xy - .5;
    				uv *= _IrisScale;
					o.uv = uv + .5;
    				
					float3 viewDir = -ObjSpaceViewDir(v.vertex);
					float3 worldRefl = mul ((float3x3)_Object2World, viewDir);
					TANGENT_SPACE_ROTATION;
  					o.lightDir = normalize(mul (rotation, ObjSpaceLightDir(v.vertex)));
  					o.viewDir = mul(rotation, ObjSpaceViewDir(v.vertex));
                    o.normal = normalize( mul( float4( v.normal, 0.0 ), _World2Object ).xyz );
                      
  					
                    TRANSFER_VERTEX_TO_FRAGMENT(o);
                    return o;
                }
                sampler2D _MainTex;
                sampler2D _BumpMap;
                sampler2D _BumpMap2;
                sampler2D _Masks;
                fixed4 _Color;
                fixed4 _Color2;
                fixed4 _LightColor0;
                half _SpecStr;
                half _SpecPower;
                half _Parallax;
                fixed _Dilation;
                int _MinSamples;
                int _MaxSamples;
               	
                
                float4 frag(v2f i) : COLOR
                {
					#ifndef USING_DIRECTIONAL_LIGHT
  					fixed3 lightDir = normalize(i.lightDir);
  					#else
  					fixed3 lightDir = i.lightDir;
  					#endif
  					
                	half3 nViewDirT = normalize(i.viewDir);
                    half3 h = normalize(lightDir + nViewDirT);
                	
                	half parallaxLimit = -length( i.viewDir.xy ) / i.viewDir.z;
					parallaxLimit *= _Parallax;
					
					half2 vOffsetDir = normalize( i.viewDir.xy );
					half2 vMaxOffset = vOffsetDir * parallaxLimit;
					
					int numSamples = (int)lerp( _MaxSamples, _MinSamples, dot( normalize(i.viewDir), i.normal ) );
					fixed stepSize = 1.0 / (float)numSamples;
					
					half2 dx = ddx( i.uv );
					half2 dy = ddy( i.uv );
					
					half fCurrRayHeight = 1.0;
					half2 vCurrOffset = float2( 0, 0 );
					half2 vLastOffset = float2( 0, 0 );
					
					half fLastSampledHeight = 1;
					half fCurrSampledHeight = 1;
			
					int nCurrSample = 0;
					while ( nCurrSample < numSamples )
					{
					  fCurrSampledHeight = tex2D( _Masks, i.uv + vCurrOffset, dx, dy ).g;
					  
					  if ( fCurrSampledHeight > fCurrRayHeight )
					  {
					    float delta1 = fCurrSampledHeight - fCurrRayHeight;
					    float delta2 = ( fCurrRayHeight + stepSize ) - fLastSampledHeight;
					
					    float ratio = delta1/(delta1+delta2);
					
					    vCurrOffset = (ratio) * vLastOffset + (1.0-ratio) * vCurrOffset;
					    nCurrSample = numSamples + 1;
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
					         
					half2 uv = i.uv + vCurrOffset - .5;
                    half pupil = saturate(length(uv)/ 0.14);
					uv *= lerp(1.0, pupil, _Dilation);
                    uv += .5;
					
					fixed4 albido = tex2D(_MainTex, uv);
  					fixed4 masks = tex2D( _Masks, uv);

                	half3 n = UnpackNormal( tex2D(_BumpMap, uv));
                	half3 n2 = UnpackNormal( tex2D(_BumpMap2, i.uv));
  					
                    float NdotL = max(0,dot(n, lightDir));
                    float NdotH = max(0,dot(n, h));
                    float N2dotH = max(0,dot(n2, h));
                    
                    float3 spec = pow(NdotH, 40) * masks.r * _LightColor0.rgb;
                    spec += pow(N2dotH, _SpecPower) * _SpecStr * _LightColor0.rgb;
                  
                    float atten = LIGHT_ATTENUATION(i);

                    fixed4 c;
                    c.rgb = ((albido * _LightColor0.rgb * NdotL) + spec) * (atten);
                    c.a = 1;
                    return c;
                }
            ENDCG
        }
    }
    Fallback "RealEyes/RealEyes_Simple"
}
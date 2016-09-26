// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'

Shader "RealEyes/RealEyesLegacySimple" {
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
	  _Masks ("IrisSpecMask(R)Iris Heightmap(G)CorniaSpecMask(B)CorniaHeightmap(a)", 2D) = "black" {}
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
		        #include "UnityCG.cginc"
		        #include "AutoLight.cginc"
		
		        struct v2f
		        {
		            float4 pos : SV_POSITION;
					float2 uv : TEXCOORD0;
					fixed3 lightDir : TEXCOORD1;
					fixed3 vlight : TEXCOORD2;
					float3 viewDir : TEXCOORD3;
		
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
					
					TANGENT_SPACE_ROTATION;
						o.lightDir = normalize(mul (rotation, ObjSpaceLightDir(v.vertex)));
						o.viewDir = mul(rotation, ObjSpaceViewDir(v.vertex));
						float3 worldN = mul((float3x3)unity_ObjectToWorld, SCALED_NORMAL);
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
		       	
		        
		        float4 frag(v2f i) : COLOR
		        {
		        	half3 nViewDirT = normalize(i.viewDir);
		            half3 h = normalize(i.lightDir + nViewDirT);
		
					half2 uv = i.uv - .5;
		            half pupil = saturate(length(uv)/ 0.14);
					uv *= lerp(1.0, pupil, _Dilation);
		            uv += .5;
		
						fixed4 albido = tex2D(_MainTex, uv);
						fixed4 masks = tex2D( _Masks, uv);
						
						albido.rgb *= (_Color * albido.a) + (_Color2 * (1-albido.a));
		        	half3 n = UnpackNormal( tex2D(_BumpMap, uv));
		        	half3 n2 = UnpackNormal( tex2D(_BumpMap2, i.uv));
						
		            float NdotL = max(0,dot(n, i.lightDir));
		            float NdotH = max(0,dot(n, h));
		            float N2dotH = max(0,dot(n2, h));
		            
		            float3 spec = pow(NdotH, 40) * masks.r * _LightColor0.rgb;
		            spec += pow(N2dotH, _SpecPower) * _SpecStr * _LightColor0.rgb;
		          
		            float atten = LIGHT_ATTENUATION(i);
					
		            fixed4 c;
		            c.rgb = ((albido * _LightColor0.rgb * NdotL)  + spec) * (atten);
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
				#include "UnityShaderVariables.cginc"
                #pragma multi_compile_fwdadd
                #define UNITY_PASS_FORWARDADD
                #include "UnityCG.cginc"
                #include "AutoLight.cginc"

                struct v2f
                {
                    float4 pos : SV_POSITION;
					float2 uv : TEXCOORD0;
					fixed3 lightDir : TEXCOORD1;
					float3 viewDir : TEXCOORD2;
                    LIGHTING_COORDS(3,4) 
                }; 
				
				fixed _IrisScale;
				
                v2f vert (appdata_tan v)
                {
                    v2f o;
                    
    				o.pos = mul(UNITY_MATRIX_MVP, v.vertex);
    				
   				    float2 uv = v.texcoord.xy - .5;
    				uv *= _IrisScale;
					o.uv = uv + .5;

					TANGENT_SPACE_ROTATION;
  					o.lightDir = mul (rotation, ObjSpaceLightDir(v.vertex));
  					o.viewDir = mul(rotation, ObjSpaceViewDir(v.vertex));
  					
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
               	
                
                float4 frag(v2f i) : COLOR
                {
					#ifndef USING_DIRECTIONAL_LIGHT
  					fixed3 lightDir = normalize(i.lightDir);
  					#else
  					fixed3 lightDir = i.lightDir;
  					#endif
  					
                	half3 nViewDirT = normalize(i.viewDir);
                    half3 h = normalize(lightDir + nViewDirT);
                	
                
					half2 uv = i.uv - .5;
                    half pupil = saturate(length(uv)/ 0.14);
					uv *= lerp(1.0, pupil, _Dilation);
                    uv += .5;
					
					fixed4 albido = tex2D(_MainTex, uv);
  					fixed4 masks = tex2D( _Masks, uv);

                	half3 n = UnpackNormal( tex2D(_BumpMap, uv));
                	half3 n2 = UnpackNormal( tex2D(_BumpMap2, i.uv));
  					
                    fixed NdotL = max(0,dot(n, lightDir));
                    fixed NdotH = max(0,dot(n2, h));

                    half3 spec = pow(NdotH, _SpecPower) * _SpecStr * _LightColor0.rgb;
                  
                    float atten = LIGHT_ATTENUATION(i);

                    fixed4 c;
                    c.rgb = ((albido * _LightColor0.rgb * NdotL) + spec) * (atten);
                    c.a = 1;
                    return c;
                }
            ENDCG
        }
    }
    Fallback "Diffuse"
}
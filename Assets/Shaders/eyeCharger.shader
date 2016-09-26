// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'

// Shader created with Shader Forge v1.13 
// Shader Forge (c) Neat Corporation / Joachim Holmer - http://www.acegikmo.com/shaderforge/
// Note: Manually altering this data may prevent you from opening it in Shader Forge
/*SF_DATA;ver:1.13;sub:START;pass:START;ps:flbk:,lico:1,lgpr:1,nrmq:1,nrsp:0,limd:1,spmd:1,trmd:0,grmd:0,uamb:True,mssp:True,bkdf:False,rprd:False,enco:False,rmgx:True,rpth:0,hqsc:True,hqlp:False,tesm:0,bsrc:0,bdst:1,culm:0,dpts:2,wrdp:True,dith:0,ufog:True,aust:True,igpj:False,qofs:0,qpre:1,rntp:1,fgom:False,fgoc:False,fgod:False,fgor:False,fgmd:0,fgcr:0.5,fgcg:0.5,fgcb:0.5,fgca:1,fgde:0.01,fgrn:0,fgrf:300,ofsf:0,ofsu:0,f2p0:False;n:type:ShaderForge.SFN_Final,id:7737,x:34405,y:33643,varname:node_7737,prsc:2|diff-9978-OUT,spec-7922-OUT,normal-917-RGB,emission-3189-OUT;n:type:ShaderForge.SFN_TexCoord,id:2022,x:32924,y:33262,varname:node_2022,prsc:2,uv:0;n:type:ShaderForge.SFN_ValueProperty,id:4656,x:32819,y:33449,ptovrint:False,ptlb:panning,ptin:_panning,varname:node_4656,prsc:2,glob:False,v1:0;n:type:ShaderForge.SFN_Tex2d,id:5094,x:33338,y:33429,ptovrint:False,ptlb:panning texture,ptin:_panningtexture,varname:node_5094,prsc:2,ntxv:0,isnm:False|UVIN-2947-UVOUT;n:type:ShaderForge.SFN_Panner,id:2947,x:33104,y:33392,varname:node_2947,prsc:2,spu:0,spv:1|UVIN-2022-UVOUT,DIST-9416-OUT;n:type:ShaderForge.SFN_ValueProperty,id:7922,x:33944,y:33707,ptovrint:False,ptlb:metallic,ptin:_metallic,varname:node_7922,prsc:2,glob:False,v1:0;n:type:ShaderForge.SFN_Color,id:2965,x:33668,y:33525,ptovrint:False,ptlb:color2,ptin:_color2,varname:node_2965,prsc:2,glob:False,c1:0.5,c2:0.5,c3:0.5,c4:1;n:type:ShaderForge.SFN_Color,id:8154,x:33709,y:33113,ptovrint:False,ptlb:color1,ptin:_color1,varname:node_8154,prsc:2,glob:False,c1:0.5,c2:0.5,c3:0.5,c4:1;n:type:ShaderForge.SFN_Lerp,id:7411,x:33897,y:33803,varname:node_7411,prsc:2|A-4644-OUT,B-2965-RGB,T-5094-R;n:type:ShaderForge.SFN_Vector3,id:4644,x:33668,y:33724,varname:node_4644,prsc:2,v1:0,v2:0,v3:0;n:type:ShaderForge.SFN_Fresnel,id:5736,x:33743,y:33287,varname:node_5736,prsc:2|EXP-5642-OUT;n:type:ShaderForge.SFN_Multiply,id:9978,x:33928,y:33258,varname:node_9978,prsc:2|A-8154-RGB,B-5736-OUT;n:type:ShaderForge.SFN_Vector1,id:5642,x:33492,y:33321,varname:node_5642,prsc:2,v1:-1;n:type:ShaderForge.SFN_RemapRange,id:3189,x:34082,y:33803,varname:node_3189,prsc:2,frmn:0,frmx:1,tomn:0,tomx:0.6|IN-7411-OUT;n:type:ShaderForge.SFN_RemapRange,id:9416,x:32912,y:33530,varname:node_9416,prsc:2,frmn:0,frmx:1,tomn:0,tomx:-3.22|IN-4656-OUT;n:type:ShaderForge.SFN_Tex2d,id:917,x:33944,y:33484,ptovrint:False,ptlb:normal map,ptin:_normalmap,varname:node_917,prsc:2,ntxv:3,isnm:False;proporder:8154-2965-4656-7922-5094-917;pass:END;sub:END;*/

Shader "vrplatformer/eyeCharger" {
    Properties {
        _color1 ("color1", Color) = (0.5,0.5,0.5,1)
        _color2 ("color2", Color) = (0.5,0.5,0.5,1)
        _panning ("panning", Float ) = 0
        _metallic ("metallic", Float ) = 0
        _panningtexture ("panning texture", 2D) = "white" {}
        _normalmap ("normal map", 2D) = "bump" {}
    }
    SubShader {
        Tags {
            "RenderType"="Opaque"
        }
        Pass {
            Name "FORWARD"
            Tags {
                "LightMode"="ForwardBase"
            }
            
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #define UNITY_PASS_FORWARDBASE
            #include "UnityCG.cginc"
            #include "AutoLight.cginc"
            #pragma multi_compile_fwdbase_fullshadows
            #pragma multi_compile_fog
            #pragma exclude_renderers gles3 metal d3d11_9x xbox360 xboxone ps3 ps4 psp2 
            #pragma target 3.0
            uniform float4 _LightColor0;
            uniform float _panning;
            uniform sampler2D _panningtexture; uniform float4 _panningtexture_ST;
            uniform float _metallic;
            uniform float4 _color2;
            uniform float4 _color1;
            uniform sampler2D _normalmap; uniform float4 _normalmap_ST;
            struct VertexInput {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float4 tangent : TANGENT;
                float2 texcoord0 : TEXCOORD0;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                float2 uv0 : TEXCOORD0;
                float4 posWorld : TEXCOORD1;
                float3 normalDir : TEXCOORD2;
                float3 tangentDir : TEXCOORD3;
                float3 bitangentDir : TEXCOORD4;
                LIGHTING_COORDS(5,6)
                UNITY_FOG_COORDS(7)
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.uv0 = v.texcoord0;
                o.normalDir = UnityObjectToWorldNormal(v.normal);
                o.tangentDir = normalize( mul( unity_ObjectToWorld, float4( v.tangent.xyz, 0.0 ) ).xyz );
                o.bitangentDir = normalize(cross(o.normalDir, o.tangentDir) * v.tangent.w);
                o.posWorld = mul(unity_ObjectToWorld, v.vertex);
                float3 lightColor = _LightColor0.rgb;
                o.pos = mul(UNITY_MATRIX_MVP, v.vertex);
                UNITY_TRANSFER_FOG(o,o.pos);
                TRANSFER_VERTEX_TO_FRAGMENT(o)
                return o;
            }
            float4 frag(VertexOutput i) : COLOR {
                i.normalDir = normalize(i.normalDir);
                float3x3 tangentTransform = float3x3( i.tangentDir, i.bitangentDir, i.normalDir);
/////// Vectors:
                float3 viewDirection = normalize(_WorldSpaceCameraPos.xyz - i.posWorld.xyz);
                float4 _normalmap_var = tex2D(_normalmap,TRANSFORM_TEX(i.uv0, _normalmap));
                float3 normalLocal = _normalmap_var.rgb;
                float3 normalDirection = normalize(mul( normalLocal, tangentTransform )); // Perturbed normals
                float3 lightDirection = normalize(_WorldSpaceLightPos0.xyz);
                float3 lightColor = _LightColor0.rgb;
                float3 halfDirection = normalize(viewDirection+lightDirection);
////// Lighting:
                float attenuation = LIGHT_ATTENUATION(i);
                float3 attenColor = attenuation * _LightColor0.xyz;
///////// Gloss:
                float gloss = 0.5;
                float specPow = exp2( gloss * 10.0+1.0);
////// Specular:
                float NdotL = max(0, dot( normalDirection, lightDirection ));
                float3 specularColor = float3(_metallic,_metallic,_metallic);
                float3 directSpecular = (floor(attenuation) * _LightColor0.xyz) * pow(max(0,dot(halfDirection,normalDirection)),specPow)*specularColor;
                float3 specular = directSpecular;
/////// Diffuse:
                NdotL = max(0.0,dot( normalDirection, lightDirection ));
                float3 directDiffuse = max( 0.0, NdotL) * attenColor;
                float3 indirectDiffuse = float3(0,0,0);
                indirectDiffuse += UNITY_LIGHTMODEL_AMBIENT.rgb; // Ambient Light
                float3 node_9978 = (_color1.rgb*pow(1.0-max(0,dot(normalDirection, viewDirection)),(-1.0)));
                float3 diffuseColor = node_9978;
                float3 diffuse = (directDiffuse + indirectDiffuse) * diffuseColor;
////// Emissive:
                float2 node_2947 = (i.uv0+(_panning*-3.22+0.0)*float2(0,1));
                float4 _panningtexture_var = tex2D(_panningtexture,TRANSFORM_TEX(node_2947, _panningtexture));
                float3 emissive = (lerp(float3(0,0,0),_color2.rgb,_panningtexture_var.r)*0.6+0.0);
/// Final Color:
                float3 finalColor = diffuse + specular + emissive;
                fixed4 finalRGBA = fixed4(finalColor,1);
                UNITY_APPLY_FOG(i.fogCoord, finalRGBA);
                return finalRGBA;
            }
            ENDCG
        }
        Pass {
            Name "FORWARD_DELTA"
            Tags {
                "LightMode"="ForwardAdd"
            }
            Blend One One
            
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #define UNITY_PASS_FORWARDADD
            #include "UnityCG.cginc"
            #include "AutoLight.cginc"
            #pragma multi_compile_fwdadd_fullshadows
            #pragma multi_compile_fog
            #pragma exclude_renderers gles3 metal d3d11_9x xbox360 xboxone ps3 ps4 psp2 
            #pragma target 3.0
            uniform float4 _LightColor0;
            uniform float _panning;
            uniform sampler2D _panningtexture; uniform float4 _panningtexture_ST;
            uniform float _metallic;
            uniform float4 _color2;
            uniform float4 _color1;
            uniform sampler2D _normalmap; uniform float4 _normalmap_ST;
            struct VertexInput {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float4 tangent : TANGENT;
                float2 texcoord0 : TEXCOORD0;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                float2 uv0 : TEXCOORD0;
                float4 posWorld : TEXCOORD1;
                float3 normalDir : TEXCOORD2;
                float3 tangentDir : TEXCOORD3;
                float3 bitangentDir : TEXCOORD4;
                LIGHTING_COORDS(5,6)
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.uv0 = v.texcoord0;
                o.normalDir = UnityObjectToWorldNormal(v.normal);
                o.tangentDir = normalize( mul( unity_ObjectToWorld, float4( v.tangent.xyz, 0.0 ) ).xyz );
                o.bitangentDir = normalize(cross(o.normalDir, o.tangentDir) * v.tangent.w);
                o.posWorld = mul(unity_ObjectToWorld, v.vertex);
                float3 lightColor = _LightColor0.rgb;
                o.pos = mul(UNITY_MATRIX_MVP, v.vertex);
                TRANSFER_VERTEX_TO_FRAGMENT(o)
                return o;
            }
            float4 frag(VertexOutput i) : COLOR {
                i.normalDir = normalize(i.normalDir);
                float3x3 tangentTransform = float3x3( i.tangentDir, i.bitangentDir, i.normalDir);
/////// Vectors:
                float3 viewDirection = normalize(_WorldSpaceCameraPos.xyz - i.posWorld.xyz);
                float4 _normalmap_var = tex2D(_normalmap,TRANSFORM_TEX(i.uv0, _normalmap));
                float3 normalLocal = _normalmap_var.rgb;
                float3 normalDirection = normalize(mul( normalLocal, tangentTransform )); // Perturbed normals
                float3 lightDirection = normalize(lerp(_WorldSpaceLightPos0.xyz, _WorldSpaceLightPos0.xyz - i.posWorld.xyz,_WorldSpaceLightPos0.w));
                float3 lightColor = _LightColor0.rgb;
                float3 halfDirection = normalize(viewDirection+lightDirection);
////// Lighting:
                float attenuation = LIGHT_ATTENUATION(i);
                float3 attenColor = attenuation * _LightColor0.xyz;
///////// Gloss:
                float gloss = 0.5;
                float specPow = exp2( gloss * 10.0+1.0);
////// Specular:
                float NdotL = max(0, dot( normalDirection, lightDirection ));
                float3 specularColor = float3(_metallic,_metallic,_metallic);
                float3 directSpecular = attenColor * pow(max(0,dot(halfDirection,normalDirection)),specPow)*specularColor;
                float3 specular = directSpecular;
/////// Diffuse:
                NdotL = max(0.0,dot( normalDirection, lightDirection ));
                float3 directDiffuse = max( 0.0, NdotL) * attenColor;
                float3 node_9978 = (_color1.rgb*pow(1.0-max(0,dot(normalDirection, viewDirection)),(-1.0)));
                float3 diffuseColor = node_9978;
                float3 diffuse = directDiffuse * diffuseColor;
/// Final Color:
                float3 finalColor = diffuse + specular;
                return fixed4(finalColor * 1,0);
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
    CustomEditor "ShaderForgeMaterialInspector"
}

float3 DLdirection;
float3 DLambient;
float3 DLdiffuse;
float3 DLspecular;

float3 PLposition;
float PLconstant;
float PLlinear;
float PLquadratic;
float3 PLambient;
float3 PLdiffuse;
float3 PLspecular;

float3 SLposition;
float3 SLdirection;
float SLcutOff;
float SLouterCutOff;
float SLconstant;
float SLlinear;
float SLquadratic;
float3 SLambient;
float3 SLdiffuse;
float3 SLspecular;

float4x4 xView;
float4x4 xProjection;
float4x4 xWorld;
float3 xCamPos;
float gamma;

bool red;
bool isInfected;

float4 DiffuseColor;

float4 ClipPlane;
bool ClipPlaneEnabled = false;

bool DoShadowMapping = true;
float4x4 ShadowView;
float4x4 ShadowProjection;
float3 ShadowLightPosition;
float ShadowFarPlane;
float ShadowMult = 0.3f;
float ShadowBias = 1.0f / 50.0f;
texture2D ShadowMap;
sampler2D shadowSampler = sampler_state {
	texture = <ShadowMap>;
	minfilter = point;
	magfilter = point;
	mipfilter = point;
};
#include "PPShared.vsi"
float sampleShadowMap(float2 UV)
{
	if (UV.x < 0 || UV.x > 1 || UV.y < 0 || UV.y > 1)
		return 1;
	return tex2D(shadowSampler, UV).r;
}

texture2D BasicTexture;
sampler2D basicTextureSampler = sampler_state {
	texture = <BasicTexture>;
	addressU = wrap;
	addressV = wrap;
	minfilter = anisotropic;
	magfilter = anisotropic;
	mipfilter = linear;
};
bool TextureEnabled = true;

texture2D NormalMap;
sampler2D NormalMapSampler = sampler_state
{
	Texture = <NormalMap>;
	MinFilter = linear;
	MagFilter = linear;
	MipFilter = linear;
};
bool NormalMapEnabled = false;

struct VSI {
	float4 Position : POSITION0;
	float2 UV : TEXCOORD0;
	float4 Color : COLOR0;
	float3 Normal : NORMAL0;
};

struct VSO {
	float4 Position : POSITION;
	float4 Color : COLOR;
	float3 Normal : TEXCOORD0;
	float4 WorldPosition : TEXCOORD1;
	float2 UV : TEXCOORD2;
	float4 ShadowScreenPosition : TEXCOORD3;
};


VSO ColoredVS(VSI input) {

	VSO output;

	output.WorldPosition = mul(input.Position, xWorld);
	float4x4 wvp = mul(mul(xWorld, xView), xProjection);
	output.Position = mul(input.Position, wvp);
	output.Color = input.Color;
	output.Normal = mul(input.Normal, xWorld);
	output.UV = input.UV;
	output.ShadowScreenPosition = mul(mul(input.Position, xWorld),
		mul(ShadowView, ShadowProjection));
	return output;
}

float4 ColoredPS(VSO input) : COLOR{
	if (ClipPlaneEnabled) {
		clip(dot(float4(input.WorldPosition), ClipPlane));
	}
	float3 norm = normalize(input.Normal);
	if (NormalMapEnabled == true)
		norm = tex2D(NormalMapSampler, input.UV);
	
	float3 viewDir = normalize(xCamPos - input.WorldPosition.xyz);
	float3 basicTexture = tex2D(basicTextureSampler, input.UV);

	float2 shadowTexCoord = postProjToScreen(input.ShadowScreenPosition)
		+ halfPixel();
	float mapDepth = sampleShadowMap(shadowTexCoord);

	float realDepth = input.ShadowScreenPosition.z / input.ShadowScreenPosition.w;
	float shadow = 1;
	float bias = 1.0f / 100.0f;
	if (realDepth < 1 && realDepth - bias > mapDepth)
		shadow = 0.5f;

	if (!TextureEnabled)
		basicTexture = DiffuseColor;
	if (isInfected == true) {
		basicTexture = float3(0.3f, 0.28f, 0.25f);
	}
	//kierunkowe
	float3 lightDir = normalize(-DLdirection);
	float diff = max(dot(norm, lightDir), 0.0);
	float3 reflectDir = reflect(-lightDir, norm);
	float spec = pow(max(dot(viewDir, reflectDir), 0.0), 32);
	float3 ambient = DLambient * basicTexture;
	float3 diffuse = DLdiffuse * diff * basicTexture;
	float3 specular = DLspecular * spec * basicTexture;


	float3 result = (ambient + diffuse + specular);



	//punktowe
	lightDir = normalize(PLposition - input.WorldPosition.xyz);
	diff = max(dot(norm, lightDir), 0.0);
	reflectDir = reflect(-lightDir, norm);
	spec = pow(max(dot(viewDir, reflectDir), 0.0), 32);
	float distance = length(PLposition - input.WorldPosition.xyz);
	float attenuation = 1.0 / (PLconstant + PLlinear * distance + PLquadratic * (distance * distance));
	ambient = PLambient * basicTexture;
	diffuse = PLdiffuse * diff * basicTexture;
	specular = PLspecular * spec * basicTexture;
	ambient *= attenuation;
	diffuse *= attenuation;
	specular *= attenuation;
	result += (ambient + diffuse + specular);

	//spot light
	lightDir = normalize(SLposition - input.WorldPosition.xyz);
	diff = max(dot(norm, lightDir), 0.0);
	reflectDir = reflect(-lightDir, norm);
	spec = pow(max(dot(viewDir, reflectDir), 0.0), 32);
	distance = length(SLposition - input.WorldPosition.xyz);
	attenuation = 1.0 / (SLconstant + SLlinear * distance + SLquadratic * (distance * distance));
	float theta = dot(lightDir, normalize(-SLdirection));
	float epsilon = SLcutOff - SLouterCutOff;
	float intensity = clamp((theta - SLouterCutOff) / epsilon, 0.0, 1.0);
	ambient = SLambient * basicTexture;
	diffuse = SLdiffuse * diff * basicTexture;
	specular = SLspecular * spec * basicTexture;
	ambient *= attenuation * intensity;
	diffuse *= attenuation * intensity;
	specular *= attenuation * intensity;
	result += (ambient + diffuse + specular);

	//result = result / (result+1.0f);
	result = result * shadow;
	result = float3(1.0f, 1.0f, 1.0f) - exp(-result * 1.0f);
	
	result = pow(result, 1.0f / gamma);
	/*if (isInfected == true) {
		result = float3(0.3f, 0.28f, 0.25f);
	}*/
	if (red == true) {
		result = float3(1.0f, 0.0f, 0.0f) - exp(-result * 1.0f);
		result = pow(result, 1.0f / 5.0f);
	}
	return float4(result, 1.0);
}


technique Colored
{
	pass Pass0
	{
#if SM4
		VertexShader = compile vs_4_0_level_9_3 ColoredVS();
		PixelShader = compile ps_4_0_level_9_3 ColoredPS();
#else
		VertexShader = compile vs_2_0 ColoredVS();
		PixelShader = compile ps_3_0 ColoredPS();
#endif				
	}
}
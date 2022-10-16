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
//ProjectedTexture
float4x4 ProjectorViewProjection;
texture2D ProjectedTexture;
sampler2D projectorSampler = sampler_state
{
	texture = <ProjectedTexture>;
};
bool ProjectorEnabled = false;
#include "PPShared.vsi"
float sampleShadowMap(float2 UV)
{
	if (UV.x < 0 || UV.x > 1 || UV.y < 0 || UV.y > 1)
		return 1;
	return tex2D(shadowSampler, UV).r;
}

float3 sampleProjector(float2 UV)
{
	if (UV.x < 0 || UV.x > 1 || UV.y < 0 || UV.y > 1)
		return float3(0, 0, 0);
	return tex2D(projectorSampler, UV);
}

struct VSO {
	float4 Position : POSITION;
	float4 Color : COLOR;
	float3 Normal : TEXCOORD0;
	float4 WorldPosition : TEXCOORD1;
	float4 ShadowScreenPosition : TEXCOORD2;
	float4 ProjectorScreenPosition : TEXCOORD3;
	//float4 Positioncopy : TEXCOORD3;
};


VSO ColoredVS(float4 inPos : POSITION, float4 inColor : COLOR, float3 inNormal : NORMAL) {

	VSO output;

	output.WorldPosition = mul(inPos, xWorld);
	float4x4 wvp = mul(mul(xWorld, xView), xProjection);
	output.Position = mul(inPos, wvp);
	//output.Positioncopy = inPos;
	output.Color = inColor;
	output.Normal = mul(float4(inNormal, 1.0f), xWorld).xyz;
	output.ShadowScreenPosition = mul(mul(inPos, xWorld),
		mul(ShadowView, ShadowProjection));
	output.ProjectorScreenPosition = mul(mul(inPos, xWorld), ProjectorViewProjection);
	return output;
}

float4 ColoredPS(VSO input) : COLOR{
	if (ClipPlaneEnabled)
		clip(dot(float4(input.WorldPosition), ClipPlane));

	float3 norm = normalize(input.Normal);
	float3 viewDir = normalize(xCamPos - input.WorldPosition.xyz);

	float2 shadowTexCoord = postProjToScreen(input.ShadowScreenPosition)
		+ halfPixel();
	float mapDepth = sampleShadowMap(shadowTexCoord);
	float realDepth = input.ShadowScreenPosition.z / input.ShadowScreenPosition.w;
	float shadow = 1;
	float bias = 1.0f / 100.0f;
	if (realDepth < 1 && realDepth - bias > mapDepth)
		shadow = 0.5f;

	//kierunkowe
	float3 lightDir = normalize(-DLdirection);
	float diff = max(dot(norm, lightDir), 0.0);
	float3 reflectDir = reflect(-lightDir, norm);
	float spec = pow(max(dot(viewDir, reflectDir), 0.0), 32);
	float3 ambient = DLambient * input.Color.xyz;
	float3 diffuse = DLdiffuse * diff * input.Color.xyz;
	float3 specular = DLspecular * spec * input.Color.xyz;


	float3 result = (ambient + diffuse + specular);



	//punktowe
	lightDir = normalize(PLposition - input.WorldPosition.xyz);
	diff = max(dot(norm, lightDir), 0.0);
	reflectDir = reflect(-lightDir, norm);
	spec = pow(max(dot(viewDir, reflectDir), 0.0), 32);
	float distance = length(PLposition - input.WorldPosition.xyz);
	float attenuation = 1.0 / (PLconstant + PLlinear * distance + PLquadratic * (distance * distance));
	ambient = PLambient * input.Color.xyz;
	diffuse = PLdiffuse * diff * input.Color.xyz;
	specular = PLspecular * spec * input.Color.xyz;
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
	ambient = SLambient * input.Color.xyz;
	diffuse = SLdiffuse * diff * input.Color.xyz;
	specular = SLspecular * spec * input.Color.xyz;
	ambient *= attenuation * intensity;
	diffuse *= attenuation * intensity;
	specular *= attenuation * intensity;
	result += (ambient + diffuse + specular);

	result = result * shadow;
	//result = result / (result+1.0f);
	result = float3(1.0f, 1.0f, 1.0f) - exp(-result * 1.0f);
	result = pow(result, 1.0f / gamma);



	result += sampleProjector(postProjToScreen(
		input.ProjectorScreenPosition) + halfPixel());
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
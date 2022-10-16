float4x4 xWorld;
float4x4 xView;
float4x4 xProjection;
float3 xCamPos;
float4x4 ReflectedView;
texture ReflectionMap;
float WaveLength = 0.6f;
float WaveHeight = 0.2f;
float Time = 0.0f;
float WaveSpeed = 0.04f;

sampler2D reflectionSampler = sampler_state {
	texture = <ReflectionMap>;
	MinFilter = Anisotropic;
	MagFilter = Anisotropic;
	AddressU = Wrap;
	AddressV = Wrap;
};
texture WaterNormalMap;
sampler2D waterNormalSampler = sampler_state {
	texture = <WaterNormalMap>;
};


#include "PPShared.vsi"

struct VertexShaderInput
{
	float4 Position : POSITION0;
	float2 UV : TEXCOORD0;
};
struct VertexShaderOutput
{
	float4 Position : POSITION0;
	float4 ReflectionPosition : TEXCOORD1;
	float2 NormalMapPosition : TEXCOORD2;
};
VertexShaderOutput VertexShaderFunction(VertexShaderInput input)
{
	VertexShaderOutput output;
	float4x4 wvp = mul(xWorld, mul(xView, xProjection));
	output.Position = mul(input.Position, wvp);
	float4x4 rwvp = mul(xWorld, mul(ReflectedView, xProjection));
	output.ReflectionPosition = mul(input.Position, rwvp);
	output.NormalMapPosition = input.UV / 0.4f;
	output.NormalMapPosition.y -= Time * 0.04f;
	return output;
}

float4 PixelShaderFunction(VertexShaderOutput input) : COLOR0
{
	float4 normal = tex2D(waterNormalSampler, input.NormalMapPosition) * 2 - 1;
	float2 UVOffset = 0.01f * normal.rg;

	float2 reflectionUV = postProjToScreen(input.ReflectionPosition) +
	halfPixel();
	float3 reflection = tex2D(reflectionSampler, reflectionUV + UVOffset);
	return float4(reflection, 1);
}

technique Technique1
{
	pass Pass1
	{
		VertexShader = compile vs_1_1 VertexShaderFunction();
		PixelShader = compile ps_2_0 PixelShaderFunction();
	}
}
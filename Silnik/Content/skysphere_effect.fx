﻿/*float4x4 World;
float4x4 View;
float4x4 Projection;
float3 CameraPosition;
texture CubeMap;
samplerCUBE CubeMapSampler = sampler_state {
	texture = <CubeMap>;
	minfilter = anisotropic;
	magfilter = anisotropic;
};
struct VertexShaderInput
{
	float4 Position : POSITION0;
};
struct VertexShaderOutput {
	float4 Position : POSITION0;
	float3 WorldPosition : TEXCOORD0;
};
VertexShaderOutput VertexShaderFunction(VertexShaderInput input) {
	VertexShaderOutput output;
	float4 worldPosition = mul(input.Position, World);
	output.WorldPosition = worldPosition;
	output.Position = mul(worldPosition, mul(View, Projection));
	return output;
}

float4 PixelShaderFunction(VertexShaderOutput input) : COLOR0{
	float3 viewDirection = normalize(input.WorldPosition - CameraPosition);
	return texCUBE(CubeMapSampler, viewDirection);
}

technique Technique1 {
	pass Pass1
	{
		VertexShader = compile vs_2_0 VertexShaderFunction();
		PixelShader = compile ps_2_0 PixelShaderFunction();
	}
};*/

float4x4 World;
float4x4 View;
float4x4 Projection;

float gamma;

float3 CameraPosition;

float4 ClipPlane;
bool ClipPlaneEnabled = false;

Texture SkyBoxTexture;
samplerCUBE SkyBoxSampler = sampler_state
{
	texture = <SkyBoxTexture>;
	magfilter = LINEAR;
	minfilter = LINEAR;
	mipfilter = LINEAR;
	AddressU = Mirror;
	AddressV = Mirror;
};

struct VertexShaderInput
{
	float4 Position : POSITION0;
};

struct VertexShaderOutput
{
	float4 Position : POSITION0;
	float3 TextureCoordinate : TEXCOORD0;
};

VertexShaderOutput VertexShaderFunction(VertexShaderInput input)
{
	VertexShaderOutput output;

	float4 worldPosition = mul(input.Position, World);
	float4 viewPosition = mul(worldPosition, View);
	output.Position = mul(viewPosition, Projection);

	float4 VertexPosition = mul(input.Position, World);
	output.TextureCoordinate = VertexPosition - CameraPosition;

	return output;
}

float4 PixelShaderFunction(VertexShaderOutput input) : COLOR0
{
	float3 result = texCUBE(SkyBoxSampler, normalize(input.TextureCoordinate))*0.04f;
	result = pow(result, 1.0f / gamma);
	return float4(result, 1.0);
}

technique Skybox
{
	pass Pass1
	{
		VertexShader = compile vs_2_0 VertexShaderFunction();
		PixelShader = compile ps_2_0 PixelShaderFunction();
	}
}
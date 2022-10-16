float4x4 xWorld;
float4x4 xView;
float4x4 xProjection;
float FarPlane = 500.0f;


struct VertexShaderInput
{
	float4 Position : POSITION0;
};
struct VertexShaderOutput
{
	float4 Position : POSITION0;
	float4 ScreenPosition : TEXCOORD0;
};
VertexShaderOutput VertexShaderFunction(VertexShaderInput input)
{
	VertexShaderOutput output;

	// Calculate the screen space position
	float4x4 wvp = mul(xWorld, mul(xView, xProjection));
	float4 position = mul(input.Position, wvp);

	output.Position = position;
	output.ScreenPosition = position;

	return output;
}
float4 PixelShaderFunction(VertexShaderOutput input) : COLOR0
{
	// Determine the depth of this vertex / by the far plane distance,
	// limited to [0, 1]
	//float depth = clamp(input.ScreenPosition.z / 500.0f, 0, 1);
	float depth = (input.ScreenPosition.z-0.0065f) / input.ScreenPosition.w;
// Return only the depth value
return float4(depth, 0, 0, 1);
}
technique Technique1
{
	pass Pass1
	{
		VertexShader = compile vs_1_1 VertexShaderFunction();
		PixelShader = compile ps_2_0 PixelShaderFunction();
	}
}
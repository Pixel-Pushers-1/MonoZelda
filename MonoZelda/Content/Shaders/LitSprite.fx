#pragma optimize(off)

#if OPENGL
#define VS_SHADERMODEL vs_3_0
#define PS_SHADERMODEL ps_3_0
#else
#define VS_SHADERMODEL vs_4_0
#define PS_SHADERMODEL ps_4_0
#endif

#define MAX_LIGHTS 10
float3 light_positions[MAX_LIGHTS];
float4 light_colors[MAX_LIGHTS];
float light_active[MAX_LIGHTS]; // Use float instead of bool for multiplier
int num_lights;

#define MAX_SEGMENTS 80
float4 line_segments[MAX_SEGMENTS];
int num_line_segments;

float4x4 view_projection;
float2 player_position;
sampler TextureSampler : register(s0);

struct VertexInput
{
    float4 Position : POSITION0;
    float4 Color : COLOR0;
    float4 TexCoord : TEXCOORD0;
};
struct PixelInput
{
    float4 Position : SV_Position0;
    float4 Color : COLOR0;
    float4 TexCoord : TEXCOORD0;
};

PixelInput SpriteVertexShader(VertexInput v)
{
    PixelInput output;

    output.Position = mul(v.Position, view_projection);
    output.Color = v.Color;
    output.TexCoord = v.TexCoord;
    return output;
}

float testVisibility(float2 rayOrigin, float2 ray, float2 pointA, float2 pointB)
{
    float2 v1 = rayOrigin - pointA;
    float2 v2 = pointB - pointA;
    float2 v3 = float2(-ray.y, ray.x);

    float dotProduct = dot(v2, v3);
    if (abs(dotProduct) < 1e-6) // Parallel lines will never intersect
        return -1.0;

    float t1 = (v2.x * v1.y - v2.y * v1.x) / dotProduct; // v2 x v1
    float t2 = dot(v1, v3) / dotProduct;

    if (t1 >= 0.0 && t1 <= 1.0 && t2 >= 0.0 && t2 <= 1.0)
    {
        return t2;
    }

    return -1.0;
}

float4 SpritePixelShader(PixelInput p) : SV_TARGET
{
    float4 diffuse = tex2D(TextureSampler, p.TexCoord.xy);

    if(num_line_segments <= 0)
    {
        return diffuse * p.Color;
    }

    float2 ray = player_position - p.Position.xy;
    float4 color = float4(0, 1, 0, 1);
    float occlusionCount = 0.0;
    float sumT = 0.0;
    for (int i = 0; i < num_line_segments; ++i)
    {
        float2 topLeft = line_segments[i].xy;
        float2 bottomRight = line_segments[i].zw;

        float width = bottomRight.x - topLeft.x;
        float height = bottomRight.y - topLeft.y;

        float2 topRight = topLeft + float2(width, 0);
        float2 bottomLeft = topLeft + float2(0, height);

        float2 edgeNormal = float2(0, -1);

        float2 pointA = topLeft;
        float2 pointB = topRight;

        float2 incidentVector = normalize(ray);
        float angle = atan2(incidentVector.x, incidentVector.y);
        float normalAngle = (angle + 3.14159265359) / (2 * 3.14159265359);
        if (angle < -3.14159265359 / 2)
        {
            pointA = topRight;
            pointB = topLeft;
        }
        else if (angle < 0)
        {
            pointA = topLeft;
            pointB = topRight;
        }
        else if (angle < 3.14159265359 / 2)
        {
            pointA = topRight;
            pointB = topLeft;
        }
        else
        {
            pointA = topLeft;
            pointB = topRight;
        }

        float t1 = testVisibility(p.Position.xy, ray, pointA, pointB);
        if(t1 >= 0)
        {
            occlusionCount += 1.0;
            sumT += 1 - (t1 * t1 * t1);
        }

        if (angle < -3.14159265359 / 2)
        {
            pointA = topRight;
            pointB = bottomRight;
        }
        else if (angle < 0)
        {
            pointA = topLeft;
            pointB = bottomLeft;
        }
        else if (angle < 3.14159265359 / 2)
        {
            pointA = topRight;
            pointB = bottomRight;
        }
        else
        {
            pointA = topLeft;
            pointB = bottomLeft;
        }

        t1 = testVisibility(p.Position.xy, ray, pointA, pointB);
        if(t1 >= 0)
        {
            occlusionCount += 1.0;
            sumT += 1 - (t1 * t1 * t1);
        }
    }

    float val = 1 - sumT;
    float4 occlusionValue = float4(val, val, val, 1);
    
    float distanceToPlayer = length(p.Position.xy - player_position) / 400;
    float3 playerInfluence = float3(1 - distanceToPlayer, 1 - distanceToPlayer, 1 - distanceToPlayer);
    float3 value = saturate(playerInfluence);

    for (int light = 0; light < num_lights; ++light)
    {
        float distanceToLight = length(p.Position.xy - light_positions[light].xy) / 200;
        float3 lightInfluence = float3(1 - distanceToLight, 1 - distanceToLight, 1 - distanceToLight);
        value += saturate(lightInfluence) * light_colors[light].rgb * light_active[light];
    }

    value = saturate(value); // Clamp the final value between 0 and 1
    value = max(value * occlusionValue.rgb, float3(0.10, 0.10, 0.10)); // Ensure minimum value of 0.10

    return float4(value, 1) * diffuse * p.Color;
}

technique SpriteBatch
{
    pass
    {
        VertexShader = compile VS_SHADERMODEL SpriteVertexShader();
        PixelShader = compile PS_SHADERMODEL SpritePixelShader();
    }
}
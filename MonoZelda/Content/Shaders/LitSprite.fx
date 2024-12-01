﻿//#pragma optimize(off)

#if OPENGL
#define VS_SHADERMODEL vs_3_0
#define PS_SHADERMODEL ps_3_0
#else
#define VS_SHADERMODEL vs_4_0
#define PS_SHADERMODEL ps_4_0
#endif

#define MAX_LIGHTS 6
float4 lights[MAX_LIGHTS]; // x, y, radius, intensity
float4 lights_colors[MAX_LIGHTS]; // r, g, b, a
int num_lights;

#define MAX_SEGMENTS 80
float4 line_segments[MAX_SEGMENTS];
int num_line_segments;

float4x4 view_projection;
float2 player_position;
sampler TextureSampler : register(s0);

float menu_position;

static const float PI = 3.14159265359;
static const float PI_I = 1.0 / PI;

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

// https://en.wikipedia.org/wiki/Line%E2%80%93line_intersection
float4 testVisibility(float2 rayOrigin, float2 ray, float2 pointA, float2 pointB)
{
    float2 v1 = rayOrigin - pointA;
    float2 v2 = pointB - pointA;
    float2 v3 = float2(-ray.y, ray.x);

    float dotProduct = dot(v2, v3);
    float normalDot = dot(normalize(v2), normalize(v3));

    float t1 = (v2.x * v1.y - v2.y * v1.x) / dotProduct; // v2 x v1
    float t2 = dot(v1, v3) / dotProduct;
;
    float2 angleFromTo = normalize(v2) - normalize(ray);
    float angle = atan2(angleFromTo.x, angleFromTo.y) * PI_I;

    return float4(normalDot, t1, t2, angle);
}

float4 SpritePixelShader(PixelInput p) : SV_TARGET
{
    float4 diffuse = tex2D(TextureSampler, p.TexCoord.xy);

    if(num_line_segments <= 0 || p.Position.y > menu_position)
    {
        return diffuse * p.Color;
    }

    float4 obstructionValue = float4(1, 1, 1, 1);

    // Ray test all lights
    for (int pointIndex = 0; pointIndex < num_lights; ++pointIndex)
    {
        float2 ray = lights[pointIndex].xy - p.Position.xy;
        for(int rectIndex = 0; rectIndex < num_line_segments; ++rectIndex)
        {
            float4 rect = line_segments[rectIndex];

            float2 topLeft = rect.xy;
            float2 topRight = rect.xy + float2(rect.z, 0);
            float2 bottomLeft = rect.xy - float2(0, rect.w);
            float2 bottomRight = rect.xy + float2(rect.z, -rect.w);

            if(p.Position.x >= topLeft.x && p.Position.x <= topRight.x &&
               p.Position.y >= bottomRight.y && p.Position.y <= topRight.y)
            {
                if(lights[pointIndex].y < rect.y)
                    continue;

                float percentX = (topRight.x - p.Position.x) / rect.z; 
                float valX = -pow(2 * percentX - 1, 2) + 1;

                float percentY = (topRight.y - p.Position.y) / rect.w;
                float val = valX * percentY;

                obstructionValue -= float4(val, val, val, 0);
                continue;
            }

            // Checked edges are an X and the bottom of the rect collider
            float2 edges[4] = {
                topLeft, bottomRight,
                bottomLeft, topRight,
            };

            for(int edgeIndex = 0; edgeIndex < 2; ++edgeIndex)
            {
                float2 pointA = edges[2 * edgeIndex];
                float2 pointB = edges[2 * edgeIndex + 1];

                float4 obs = testVisibility(p.Position.xy, ray, pointA, pointB);

                if(obs.y > 0 && obs.y < 1 && obs.z > 0 && obs.z < 1)
                {
                    float nAngle = obs.w;
                    float percent = obs.z;

                    float obsValue = -pow(2 * percent - 1, 2) + 1;

                    if(edgeIndex > 0)
                    {
                        obsValue *= abs(obs.x);
                    }

                    float value = obsValue;

                    obstructionValue -= float4(value, value, value, 0);
                }
            }
        }
    }

    float3 value = float3(0, 0, 0);
    for (int lightIndex = 0; lightIndex < num_lights; ++lightIndex)
    {
        float distanceToPoint = length(p.Position.xy - lights[lightIndex].xy) / lights[lightIndex].z;
        float3 pointInfluence = float3(1 - distanceToPoint, 1 - distanceToPoint, 1 - distanceToPoint);
        value += saturate(pointInfluence) * lights_colors[lightIndex].rgb * lights[lightIndex].w;
    }

    value = saturate(value);
    value = max(value * obstructionValue.rgb, float3(0.10, 0.10, 0.10));

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
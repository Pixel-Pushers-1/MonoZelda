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

// t1, t2, inside, angle
// int getObstrutors(float2 pixelPosition, float2 lightPos, out float4 obstructors[4])
// {
//     // init the obstructors
//     for (int t = 0; t < 4; ++t)
//     {
//         obstructors[t] = float4(1, 1, 1, 1);
//     }

//     // TODO: Opporunity to create a quadtree to optimize this
//     int numObstructors = 0;

//     float2 ray = lightPos - pixelPosition;
//     float2 rayNormal = normalize(ray);

//     for (int i = 0; i < num_line_segments && numObstructors < 4; ++i)
//     {
//         float2 topLeft = line_segments[i].xy;
//         float2 bottomRight = line_segments[i].zw;

//         float width = bottomRight.x - topLeft.x;
//         float height = bottomRight.y - topLeft.y;

//         float2 topRight = topLeft + float2(width, 0);
//         float2 bottomLeft = topLeft + float2(0, height);

//         float2 edgeNormals[4] = {
//             float2(0, 1),
//             float2(1, 0),
//             float2(0, -1),
//             float2(-1, 0)
//         };

//         float4 edges[4] = {
//             float4(topLeft.x, topLeft.y, topRight.x, topRight.y),
//             float4(topRight.x, topRight.y, bottomRight.x, bottomRight.y),
//             float4(bottomLeft.x, bottomLeft.y, bottomRight.x, bottomRight.y),
//             float4(topLeft.x, topLeft.y, bottomLeft.x, bottomLeft.y)
//         };

//         for (int edgeIndex = 0; edgeIndex < 4; ++edgeIndex)
//         {
//             float4 edge = edges[edgeIndex];
//             float2 normal = edgeNormals[edgeIndex];

//             float2 pointA = float2(edge.x, edge.y);
//             float2 pointB = float2(edge.z, edge.w);

//             // 0/1, t1, t2, dotProduct
//             float4 test = testVisibility(pixelPosition, ray, pointA, pointB, normal);

//             // t1[0] = 1 if the ray intersects the edge
//             if(test[0] > 0 && test[1] >= 0 && test[1] <= 1)
//             {
//                 float4 obstructor = obstructors[numObstructors];
//                 obstructor = test.yzww;

//                 numObstructors++;
//             }
//         }
//     }

//     return numObstructors;
// }

float4 SpritePixelShader(PixelInput p) : SV_TARGET
{
    float4 diffuse = tex2D(TextureSampler, p.TexCoord.xy);

    if(num_line_segments <= 0 || p.Position.y > menu_position)
    {
        return diffuse * p.Color;
    }

    float4 obstructionValue = float4(1, 1, 1, 1);
    
    float2 ray = player_position - p.Position.xy;
    for(int rectIndex = 0; rectIndex < num_line_segments; ++rectIndex)
    {
        float4 rect = line_segments[rectIndex];


        float2 topLeft = rect.xy;
        float2 topRight = rect.xy + float2(rect.z, 0);
        float2 bottomLeft = rect.xy - float2(0, rect.w);
        float2 bottomRight = rect.xy + float2(rect.z, -rect.w);

        // if the pixel is inside the rect and the ray points up
        if(p.Position.x >= topLeft.x && p.Position.x <= topRight.x &&
           p.Position.y >= bottomRight.y && p.Position.y <= topRight.y)
        {
            if(player_position.y < rect.y)
                continue;

            float percentX = (topRight.x - p.Position.x) / rect.z; 
            float valX = -pow(2 * percentX - 1, 2) + 1;

            // fade it in as the player moves from the bottom edge to the top edge
            float percentY = (topRight.y - p.Position.y) / rect.w;

            float val = valX * percentY; 

            obstructionValue -= float4(val, val, val, 0);
            continue;
        }

        float2 edges[4] = {
            topLeft, bottomRight,
            bottomLeft, topRight
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

                // -\left(2x-1\right)^{2}+1
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
    value = max(value * obstructionValue.rgb, float3(0.10, 0.10, 0.10)); // Ensure minimum value of 0.10

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
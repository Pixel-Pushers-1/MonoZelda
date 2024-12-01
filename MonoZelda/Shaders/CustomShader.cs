

using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace MonoZelda.Shaders
{
    internal class CustomShader
    {
        public const int MAX_LIGHT_COLLIDERS = 75;
        public const int MAX_LIGHTS = 6;

        private Effect effect;
        public Effect Effect => effect;

        private EffectParameter menuPositionParameter;
        private EffectParameter viewProjectionParameter;

        // Collider parameters
        private EffectParameter numLineSegmentsParameter;
        private EffectParameter lineSegmentsParameter;

        // Light parameters
        // x, y, z, w = x, y, radius, intensity
        private EffectParameter lightsParameter;
        private EffectParameter colorsParameter;
        private EffectParameter numLightsParameter;


        private GraphicsDevice graphicsDevice;

        public CustomShader(GraphicsDevice graphicsDevice)
        {
            this.graphicsDevice = graphicsDevice;
        }

        public void LoadShader(ContentManager content)
        {
            effect = content.Load<Effect>("Shaders/LitSprite");

            numLineSegmentsParameter = effect.Parameters["num_line_segments"];
            lineSegmentsParameter = effect.Parameters["line_segments"];
            menuPositionParameter = effect.Parameters["menu_position"];
            viewProjectionParameter = effect.Parameters["view_projection"];
            lightsParameter = effect.Parameters["lights"];
            colorsParameter = effect.Parameters["lights_colors"];
            numLightsParameter = effect.Parameters["num_lights"];            

            Initialize();
        }

        private void Initialize()
        {
            SetLineSegments(new Vector4[0]);
            SetMenuPosition(192);
            SetViewProjection();
            SetDynamicLights(Array.Empty<Vector4>(), Array.Empty<Vector4>());
        }

        public void Reset()
        {
            Initialize();
        }

        public void SetDynamicLights(Vector4[] lights, Vector4[] colors)
        {
            if (lightsParameter != null)
            {
                // Invert all the y values
                for (int i = 0; i < lights.Length; i++)
                {
                    lights[i].Y = graphicsDevice.Viewport.Height - lights[i].Y;
                }

                lightsParameter.SetValue(lights);
            }

            if (colorsParameter != null)
            {
                colorsParameter.SetValue(colors);
            }

            if (numLightsParameter != null)
            {
                numLightsParameter.SetValue(lights.Length);
            }
        }

        public void SetViewProjection()
        {
            if (viewProjectionParameter != null)
            {
                int right = graphicsDevice.Viewport.Width;
                int bottom = graphicsDevice.Viewport.Height;
                var projection = Matrix.CreateOrthographicOffCenter(0, right, bottom, 0, 0, 1);
                viewProjectionParameter.SetValue(Matrix.Identity * projection);
            }
        }

        public void SetLineSegments(Vector4[] lineSegments)
        {
            if (lineSegmentsParameter != null)
            {
                lineSegmentsParameter.SetValue(lineSegments);
            }

            SetNumLineSegments(lineSegments.Length);
        }

        public void SetNumLineSegments(int numLineSegments)
        {
            if (numLineSegmentsParameter != null)
            {
                numLineSegmentsParameter.SetValue(numLineSegments);
            }
        }

        public void SetMenuPosition(float menuPosition)
        {
            if (menuPositionParameter != null)
            {
                // Y is inverted in the shader
                menuPositionParameter.SetValue(graphicsDevice.Viewport.Height - menuPosition);
            }
        }
    }
}

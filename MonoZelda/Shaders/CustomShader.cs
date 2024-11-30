

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace MonoZelda.Shaders
{
    internal class CustomShader
    {
        private Effect effect;
        public Effect Effect => effect;

        private EffectParameter numLineSegmentsParameter;
        private EffectParameter lineSegmentsParameter;
        private EffectParameter menuPositionParameter;
        private EffectParameter viewProjectionParameter;
        private EffectParameter playerPositionParameter;

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
            playerPositionParameter = effect.Parameters["player_position"];

            Initialize();
        }

        private void Initialize()
        {
            SetLineSegments(new Vector4[0]);
            SetMenuPosition(192);
            SetViewProjection();
        }

        public void Reset()
        {
            Initialize();
        }

        public void SetPlayerPosition(Point playerPosition)
        {
            if (playerPositionParameter != null)
            {
                var vector2 = new Vector2(playerPosition.X, graphicsDevice.Viewport.Height - playerPosition.Y);
                playerPositionParameter.SetValue(vector2);
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

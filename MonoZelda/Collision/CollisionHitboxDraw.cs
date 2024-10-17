using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using MonoZelda.Sprites;

namespace MonoZelda.Collision
{
    public class CollisionHitboxDraw : IDrawable
    {
        public Color GizmoColor { get; set; } = Color.White;
        public int Thickness { get; set; } = 1;

        private Collidable collidable;
        private Texture2D texture;

        public CollisionHitboxDraw(Collidable collidable, GraphicsDevice graphicsDevice)
        {
            this.collidable = collidable;
            CreateTexture(graphicsDevice);
            SpriteDrawer.RegisterDrawable(this, int.MaxValue, true);
        }

        ~CollisionHitboxDraw()
        {
            SpriteDrawer.UnregisterDrawable(this);
        }

        private void CreateTexture(GraphicsDevice graphicsDevice)
        {
            texture = new Texture2D(graphicsDevice, 1, 1);
            texture.SetData(new Color[] { Color.White });
        }

        public void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            Rectangle Bounds = collidable.Bounds;
            spriteBatch.Draw(texture, new Rectangle(Bounds.Left, Bounds.Top, Bounds.Width, Thickness), GizmoColor);
            spriteBatch.Draw(texture, new Rectangle(Bounds.Left, Bounds.Bottom - Thickness, Bounds.Width, Thickness), GizmoColor);
            spriteBatch.Draw(texture, new Rectangle(Bounds.Left, Bounds.Top, Thickness, Bounds.Height), GizmoColor);
            spriteBatch.Draw(texture, new Rectangle(Bounds.Right - Thickness, Bounds.Top, Thickness, Bounds.Height), GizmoColor);
        }
    }
}

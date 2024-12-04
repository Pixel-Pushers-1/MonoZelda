using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using MonoZelda.Sprites;

namespace MonoZelda.Collision
{
    public class CollisionHitboxDraw : IDrawable
    {
        public bool Enabled { get; set; }
        public Color GizmoColor { get; set; } = Color.White;
        public int Thickness { get; set; } = 1;

        private ICollidable collidable;
        private Texture2D texture;

        public CollisionHitboxDraw(ICollidable collidable)
        {
            this.collidable = collidable;
            texture = TextureData.GetTexture(SpriteType.Blank);
            SpriteDrawer.RegisterDrawable(this, SpriteLayer.Gizmos, true);
        }

        ~CollisionHitboxDraw() {
            Unregister();
        }

        public void Unregister()
        {
            SpriteDrawer.UnregisterDrawable(this);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            Rectangle Bounds = collidable.Bounds;
            spriteBatch.Draw(texture, new Rectangle(Bounds.Left, Bounds.Top, Bounds.Width, Thickness), GizmoColor);
            spriteBatch.Draw(texture, new Rectangle(Bounds.Left, Bounds.Bottom - Thickness, Bounds.Width, Thickness), GizmoColor);
            spriteBatch.Draw(texture, new Rectangle(Bounds.Left, Bounds.Top, Thickness, Bounds.Height), GizmoColor);
            spriteBatch.Draw(texture, new Rectangle(Bounds.Right - Thickness, Bounds.Top, Thickness, Bounds.Height), GizmoColor);
        }
    }
}

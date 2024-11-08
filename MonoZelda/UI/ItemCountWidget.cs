using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MonoZelda.Link;
using MonoZelda.Link.Projectiles;
using MonoZelda.Sprites;
using System.Collections.Generic;

namespace MonoZelda.UI
{
    internal class ItemCountWidget : ScreenWidget
    {
        private SpriteFont font;
        private Point margin = new Point(10, 10);

        private Point rupeeCountPosition = new Point(0, 22);
        private Point keyCountPosition = new Point(0, 86);
        private Point bombCountPosition = new Point(0, 118);

        public ItemCountWidget(SpriteFont spriteFont, Screen screen, Point position, ContentManager contentManager) : base(screen, position)
        {
            font = spriteFont;

            // TODO: Projectile manager needs to update PlayerState

            //show equipped projecetilie
            //ProjectileType projectileType = playerState.EquippedProjectile;
            //Point projPosition = new Point(450, 75);
            //var proj = new SpriteDict(contentManager.Load<Texture2D>("Sprites/items"), SpriteCSVData.Items, 0, projPosition);
            //string spriteName = projectileSpriteMap.TryGetValue(projectileType, out var name) ? name : "arrow";
            //proj.SetSprite(spriteName);
        }

        public override void Draw(SpriteBatch sb)
        {
            // Draw inventory counts in the correct location
            sb.DrawString(font, "00", (WidgetLocation + rupeeCountPosition).ToVector2(), Color.White);
            sb.DrawString(font, "00", (WidgetLocation + keyCountPosition).ToVector2(), Color.White);
            sb.DrawString(font, "00", (WidgetLocation + bombCountPosition).ToVector2(), Color.White);

        }

        public override void Load(ContentManager content)
        {

        }

        public override void Update()
        {

        }
    }
}

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

        private SpriteDict rupee;
        private SpriteDict key;
        private SpriteDict bomb;

        private Point rupeePosition = new Point(0, 10);
        private Point rupeeCountPosition = new Point(50, 20);
        private Point keyPosition = new Point(0, 80);
        private Point bombPosition = new Point(0, 150);

        public ItemCountWidget(SpriteFont spriteFont, Screen screen, Point position, ContentManager contentManager) : base(screen, position)
        {
            font = spriteFont;

            int numberOfRupees = PlayerState.Rupees;
            rupee = new SpriteDict(SpriteType.Items, 0, rupeePosition);
            rupee.SetSprite("rupee");
            //set key
            int numberOfKeys = PlayerState.Keys;
            key = new SpriteDict(SpriteType.Items, 0, keyPosition);
            key.SetSprite("key_0");
            //set bomb
            int numberOfBombs = PlayerState.Bombs;
            bomb = new SpriteDict(SpriteType.Items, 0, bombPosition);
            bomb.SetSprite("bomb");

            ProjectileType projectileType = PlayerState.EquippedProjectile;
            Point projPosition = new Point(450, 75);
            var proj = new SpriteDict(SpriteType.Items,SpriteLayer.Items,projPosition);
            //string spriteName = projectileSpriteMap.TryGetValue(projectileType, out var name) ? name : "arrow";
            //proj.SetSprite(spriteName);
        }

        public override void Draw(SpriteBatch sb)
        {
            // Daw inventory counts in the correct location
            sb.DrawString(font, "0", (WidgetLocation + rupeeCountPosition).ToVector2(), Color.White);
        }

        public override void Load(ContentManager content)
        {

        }

        public override void Update()
        {
            rupee.Position = WidgetLocation + rupeePosition;
            key.Position = WidgetLocation + keyPosition;
            bomb.Position = WidgetLocation + bombPosition;
        }
    }
}

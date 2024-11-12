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

        private Point rupeeCountPosition = new Point(0, -12);
        private Point keyCountPosition = new Point(0, 54);
        private Point bombCountPosition = new Point(0, 86);

        private SpriteDict equippedProjectileSprite;
        private SpriteDict swordSprite;
        private readonly Dictionary<ProjectileType, string> projectileSpriteMap = new Dictionary<ProjectileType, string>
        {
            { ProjectileType.Arrow, "arrow" },
            { ProjectileType.ArrowBlue, "arrow_blue" },
            { ProjectileType.Boomerang, "boomerang" },
            { ProjectileType.BoomerangBlue, "boomerang_blue" },
            { ProjectileType.Bomb, "bomb" },
            { ProjectileType.CandleBlue, "candle_blue" },
        };
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
            //set ProjectileSprite
            Point projPosition = WidgetLocation + margin + new Point(88, 12);
            equippedProjectileSprite = new SpriteDict(SpriteType.HUD, SpriteLayer.HUD, projPosition);

            //set SwordSprite
            Point swordPosition = WidgetLocation + margin + new Point(184, 12);
            swordSprite = new SpriteDict(SpriteType.HUD, SpriteLayer.HUD, swordPosition);
            swordSprite.SetSprite("woodensword");
            UpdateProjectileSprite();




        }

        public override void Draw(SpriteBatch sb)
        {
            //format ruppees
            string rupeeCount = PlayerState.Rupees.ToString("D2");
            string keyCount = PlayerState.Keys.ToString("D2");
            string bombCount = PlayerState.Bombs.ToString("D2");
            //draw rupppessw
            sb.DrawString(font, rupeeCount, (WidgetLocation + rupeeCountPosition).ToVector2(), Color.White);
            sb.DrawString(font, keyCount, (WidgetLocation + keyCountPosition).ToVector2(), Color.White);
            sb.DrawString(font, bombCount, (WidgetLocation + bombCountPosition).ToVector2(), Color.White);
        }

        public override void Load(ContentManager content)
        {

        }

        public override void Update()
        {
            UpdateProjectileSprite();
            UpdateSwordSprite();
        }

        private void UpdateProjectileSprite()
        {
            ProjectileType currentProjectile = PlayerState.EquippedProjectile;
            equippedProjectileSprite.Position = WidgetLocation + margin + new Point(88, 12);

            if (projectileSpriteMap.TryGetValue(currentProjectile, out string spriteName))
            {
                equippedProjectileSprite.Enabled = true;
                equippedProjectileSprite.SetSprite(spriteName);
            }
            else
            {
                equippedProjectileSprite.Enabled = false;
            }
        }

        private void UpdateSwordSprite()
        {
            swordSprite.Position = WidgetLocation + margin + new Point(184, 12);



        }
    }
}

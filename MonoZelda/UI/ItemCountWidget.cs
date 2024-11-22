using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MonoZelda.Link;
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
        private SpriteDict equippedWeaponSprite;
        private SpriteDict swordSprite;

        private readonly Dictionary<WeaponType, string> weaponSpriteMap = new()
        {
            { WeaponType.Bow, "bow" },
            { WeaponType.Boomerang, "boomerang" },
            { WeaponType.CandleBlue, "candle_blue" },
            { WeaponType.Bomb, "bomb" },
        };

        public ItemCountWidget(SpriteFont spriteFont, Screen screen, Point position) : base(screen, position)
        {
            font = spriteFont;

            Point projPosition = WidgetLocation + margin + new Point(88, 12);
            equippedWeaponSprite = new SpriteDict(SpriteType.HUD, SpriteLayer.HUD, projPosition);

            //set SwordSprite
            Point swordPosition = WidgetLocation + margin + new Point(184, 12);
            swordSprite = new SpriteDict(SpriteType.HUD, SpriteLayer.HUD, swordPosition);
            swordSprite.SetSprite("woodensword");

            //update Weapon sprite
            UpdateWeaponSprite();
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
            UpdateWeaponSprite();
            UpdateSwordSprite();
        }

        private void UpdateWeaponSprite()
        {
            WeaponType currentWeapon = PlayerState.EquippedWeapon;
            equippedWeaponSprite.Position = WidgetLocation + margin + new Point(88, 12);

            if (weaponSpriteMap.TryGetValue(currentWeapon, out string spriteName))
            {
                equippedWeaponSprite.Enabled = true;
                equippedWeaponSprite.SetSprite(spriteName);
            }
            else
            {
                equippedWeaponSprite.Enabled = false;
            }
        }

        private void UpdateSwordSprite()
        {
            swordSprite.Position = WidgetLocation + margin + new Point(184, 12);
        }
    }
}

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MonoZelda.Link;
using MonoZelda.Sprites;
using System.Collections.Generic;
using System.Diagnostics;

namespace MonoZelda.UI
{
    internal class HUDBackgroundWidget : ScreenWidget
    {
        private SpriteDict spriteDict;

        public HUDBackgroundWidget(Screen screen, Point position, ContentManager cm) : base(screen, position)
        {
            spriteDict = new(SpriteType.HUD, SpriteLayer.HUD - 1, position);
            spriteDict.SetSprite("hud_background");
        }

        public override void Draw(SpriteBatch sb)
        {
        }

        public override void Load(ContentManager content)
        {
        }

        public override void Update()
        {
        }
    }
}

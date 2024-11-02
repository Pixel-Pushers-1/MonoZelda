using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace MonoZelda.UI
{
    internal class LevelTextWidget : ScreenWidget
    {
        public string LevelName { get; set; }
        
        private SpriteFont font;
        private Point margin = new Point(10, 10);

        public LevelTextWidget(SpriteFont spriteFont, Screen screen, Point position) : base(screen, position)
        {
            font = spriteFont;
            LevelName = "Level 1";
        }

        public override void Draw(SpriteBatch sb)
        {
            // Are text origins in the upper left?
            sb.DrawString(font, LevelName, (WidgetLocation + margin).ToVector2(), Color.White);
        }

        public override void Load(ContentManager content)
        {
        }

        public override void Update()
        {
        }
    }
}

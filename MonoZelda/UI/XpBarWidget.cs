using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MonoZelda.Link;
using MonoZelda.Sprites;
using System.Collections.Generic;
using System.Diagnostics;

namespace MonoZelda.UI;

internal class XpBarWidget : ScreenWidget
{
    private static readonly Point barPosition = Point.Zero;

    private ProgressBarSprite fullXpBarSprite;

    public XpBarWidget(Screen screen, Point position) : base(screen, position)
    {
        fullXpBarSprite = new(SpriteType.HUD, SpriteLayer.HUD, barPosition);
        fullXpBarSprite.SetSprite("xpbar_full");
    }

    public override void Draw(SpriteBatch sb) {
    }

    public override void Load(ContentManager content) {

    }

    public override void Update()
    {
        //REPLACE THIS LINE WITH ACTUAL XP LOGIC FROM PLAYERSTATE\/ \/ \/
        SetXp((float) MonoZeldaGame.GameTime.TotalGameTime.TotalSeconds % 1f);
        //REPLACE THIS LINE WITH ACTUAL XP LOGIC FROM PLAYERSTATE /\ /\ /\ 

        fullXpBarSprite.Position = WidgetLocation + barPosition;
    }

    private void SetXp(float xpNormalized)
    {
        fullXpBarSprite.ProgressNormalized = xpNormalized;
    }
}


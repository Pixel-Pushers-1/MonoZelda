using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.Diagnostics;

namespace MonoZelda.Sprites;

public class BlankSprite : IDrawable
{
    private enum FadeMode {
        In,
        Out,
        None,
    }

    /// <summary>
    /// The screen position to draw this SpriteDict at.
    /// </summary>
    public Point Position { get; set; }
    /// <summary>
    /// Determines whether the sprite is drawn or not.
    /// </summary>
    public bool Enabled { get; set; }
    /// <summary>
    /// Pixel size of this BlankSprite.
    /// </summary>
    public Point Size { get; set; }
    /// <summary>
    /// Color of this BlankSprite.
    /// </summary>
    public Color BaseColor { get; set; }

    private readonly Texture2D texture;
    private FadeMode fadeMode = FadeMode.None;
    private float fadeTimer;
    private float totalFadeTime;

    public BlankSprite(int priority, Point position, Point size)
    {
        this.texture = TextureData.GetTexture(SpriteType.Blank);
        Position = position;
        Size = size;
        this.Enabled = true;
        this.BaseColor = Color.White;
        SpriteDrawer.RegisterDrawable(this, priority);
    }

    public BlankSprite(int priority, Point position, Point size, Color baseColor) {
        this.texture = TextureData.GetTexture(SpriteType.Blank);
        Position = position;
        Size = size;
        this.Enabled = true;
        this.BaseColor = baseColor;
        SpriteDrawer.RegisterDrawable(this, priority);
    }

    ~BlankSprite() {
        Unregister();
    }

    public void Unregister() {
        SpriteDrawer.UnregisterDrawable(this);
    }

    public void FadeOut(float time) {
        fadeMode = FadeMode.Out;
        totalFadeTime = time;
        fadeTimer = time;
    }

    public void FadeIn(float time) {
        Enabled = true;
        fadeMode = FadeMode.In;
        totalFadeTime = time;
        fadeTimer = time;
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        //do nothing if disabled
        if (!Enabled) {
            return;
        }

        //calculate color
        Vector4 drawColor = BaseColor.ToVector4();
        switch (fadeMode) {
            case FadeMode.In: {
                float t = 1f - fadeTimer / totalFadeTime;
                drawColor = Vector4.Lerp(Vector4.Zero, BaseColor.ToVector4(), t);
                break;
            }
            case FadeMode.Out: {
                float t = 1f - fadeTimer / totalFadeTime;
                drawColor = Vector4.Lerp(BaseColor.ToVector4(), Vector4.Zero, t);
                break;
            }
        }

        //create destination rectangle
        Rectangle destRect = new(Position, Size);

        //draw current sprite
        spriteBatch.Draw(texture, destRect, new Color(drawColor));

        if (fadeTimer > 0f) {
            fadeTimer -= (float) MonoZeldaGame.GameTime.ElapsedGameTime.TotalSeconds;
        }
        else {
            if (fadeMode == FadeMode.Out) {
                Enabled = false;
            }
            fadeMode = FadeMode.None;
        }
    }
}

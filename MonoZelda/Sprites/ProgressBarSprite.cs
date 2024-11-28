using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace MonoZelda.Sprites;

public class ProgressBarSprite : IDrawable
{
    /// <summary>
    /// The screen position to draw this SpriteDict at.
    /// </summary>
    public Point Position { get; set; }
    /// <summary>
    /// Determines whether the sprite is drawn or not.
    /// </summary>
    public bool Enabled { get; set; }
    /// <summary>
    /// The normalized progress which determines how much of the sprite to draw.
    /// </summary>
    public float ProgressNormalized { get; set; }

    private readonly Texture2D texture;
    private readonly Dictionary<string, Sprite> dict = new();
    private string currentSprite = "";

    public ProgressBarSprite(SpriteType spriteType, int priority, Point position) {
        this.texture = TextureData.GetTexture(spriteType);
        Position = position;
        this.Enabled = true;
        dict = SpriteSheetParser.Parse(SpriteCSVData.GetFileName(spriteType));
        currentSprite = dict.ElementAt(0).Key;
        SpriteDrawer.RegisterDrawable(this, priority);
    }

    public void Unregister() {
        SpriteDrawer.UnregisterDrawable(this);
    }

    public void SetSprite(string name) {
        currentSprite = name;
    }

    public void Draw(SpriteBatch spriteBatch) {
        //do nothing if disabled
        if (!Enabled) {
            return;
        }

        //draw current sprite
        dict[currentSprite].Draw(spriteBatch, texture, Position, ProgressNormalized);
    }
}

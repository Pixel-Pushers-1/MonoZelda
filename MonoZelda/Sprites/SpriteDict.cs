using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.Diagnostics;

namespace MonoZelda.Sprites;

public class SpriteDict : IDrawable
{
    public Point Position { get; set; }
    public bool Enabled { get; set; }

    private readonly Texture2D texture;
    private readonly Dictionary<string, Sprite> dict = new();
    private string currentSprite = "";

    public SpriteDict(SpriteType spriteType, int priority, Point position)
    {
        this.texture = TextureData.GetTexture(spriteType);
        Position = position;
        this.Enabled = true;
        SpriteSheetParser.Parse(this, SpriteCSVData.GetFileName(spriteType));
        SpriteDrawer.RegisterDrawable(this, priority);
    }

    public void Unregister() {
        SpriteDrawer.UnregisterDrawable(this);
    }

    public void Add(Sprite sprite, string name)
    {
        dict.Add(name, sprite);

        //set current sprite to sprite being added if no sprite is currently set
        if (currentSprite == "")
        {
            currentSprite = name;
        }
    }

    public void SetSprite(string name)
    {
        currentSprite = name;
    }

    public double SetSpriteOneshot(string name)
    {
        currentSprite = name;
        return dict[currentSprite].SetOneshot(true);
    }

    public void Draw(SpriteBatch spriteBatch, GameTime gameTime)
    {
        //do nothing if disabled
        if (!Enabled) {
            return;
        }

        //draw current sprite
        dict[currentSprite].Draw(spriteBatch, gameTime, texture, Position);
    }
}

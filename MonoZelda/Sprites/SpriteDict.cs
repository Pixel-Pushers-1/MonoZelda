﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.Diagnostics;

namespace MonoZelda.Sprites;

public class SpriteDict : IDrawable
{
    public enum FlashingType
    {
        OnOff,
        Red,
        Colorful,
    }

    private const float FLASHING_RATE = 20f;
    private static Dictionary<FlashingType, List<Color>> flashingColors = new Dictionary<FlashingType, List<Color>> {
        { FlashingType.OnOff, new List<Color> { ColorData.White, ColorData.Transparent } },
        { FlashingType.Red, new List<Color> { ColorData.White, ColorData.Red } },
        { FlashingType.Colorful, new List<Color> { ColorData.Blue, ColorData.Red, ColorData.Green, ColorData.White } },
    };

    public Point Position { get; set; }
    public bool Enabled { get; set; }

    private readonly Texture2D texture;
    private readonly Dictionary<string, Sprite> dict = new();
    private string currentSprite = "";
    private FlashingType flashingType;
    private float flashingTimer;

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

    public void SetFlashing(FlashingType type, float seconds)
    {
        flashingType = type;
        flashingTimer = seconds;
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        //do nothing if disabled
        if (!Enabled) {
            return;
        }

        //draw current sprite
        dict[currentSprite].Draw(spriteBatch, texture, Position, GetColor());

        flashingTimer -= (float) MonoZeldaGame.GameTime.ElapsedGameTime.TotalSeconds;
    }

    private Color GetColor() {
        if (flashingTimer <= 0f) {
            return ColorData.White;
        }
        return flashingColors[flashingType][(int) (flashingTimer * FLASHING_RATE % flashingColors[flashingType].Count)];
    }
}

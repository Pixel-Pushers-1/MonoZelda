﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace MonoZelda.Sprites;

public class SpriteDict : IDrawable
{
    public enum FlashingType
    {
        OnOff,
        Red,
        Colorful,
        LevelUp,
    }

    private static Dictionary<FlashingType, List<Color>> flashingColors = new() {
        { FlashingType.OnOff, new List<Color> { ColorData.White, ColorData.Transparent } },
        { FlashingType.Red, new List<Color> { ColorData.White, ColorData.Red } },
        { FlashingType.Colorful, new List<Color> { ColorData.Blue, ColorData.Red, ColorData.Green, ColorData.White } },
        { FlashingType.LevelUp, new List<Color> { ColorData.Yellow, ColorData.Green, ColorData.White } }
    };

    /// <summary>
    /// The screen position to draw this SpriteDict at.
    /// </summary>
    public Point Position { get; set; }
    /// <summary>
    /// Determines whether the sprite is drawn or not.
    /// </summary>
    public bool Enabled { get; set; }
    /// <summary>
    /// Rate that colors change when flashing (per second).
    /// </summary>
    public float FlashingRate { get; set; } = 20f;

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
        dict = SpriteSheetParser.Parse(SpriteCSVData.GetFileName(spriteType));
        currentSprite = dict.ElementAt(0).Key;
        SpriteDrawer.RegisterDrawable(this, priority);
    }

    public void Unregister() {
        SpriteDrawer.UnregisterDrawable(this);
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

        if (flashingTimer > 0f) {
            flashingTimer -= (float) MonoZeldaGame.GameTime.ElapsedGameTime.TotalSeconds;
        }
    }

    private Color GetColor() {
        if (flashingTimer <= 0f) {
            return ColorData.White;
        }
        return flashingColors[flashingType][(int) (flashingTimer * FlashingRate % flashingColors[flashingType].Count)];
    }
}

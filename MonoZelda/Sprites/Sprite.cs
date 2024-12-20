﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Diagnostics;

namespace MonoZelda.Sprites;

public class Sprite
{
    public enum AnchorType
    {
        TopLeft,
        TopRight,
        BottomLeft,
        BottomRight,
        Center,
        TopQuarter,
        LeftQuarter,
        BottomQuarter,
        RightQuarter
    }

    public Rectangle SourceRect { get; set; }
    public int FrameCount { get; set; }
    public float Size { get; set; }
    public AnchorType Anchor { get; set; }
    public bool Animating { get; set; }
    public float Fps { get; set; }

    private bool oneshotPlaying = false;
    private double oneshotTimer;

    public Sprite(Rectangle sourceRect, AnchorType anchor = AnchorType.Center, int frameCount = 1, float fps = 10, float size = 4f, bool animating = true)
    {
        SourceRect = sourceRect;
        Anchor = anchor;
        FrameCount = frameCount;
        Size = size;
        Animating = animating;
        Fps = fps;
    }

    public double SetOneshot(bool value)
    {
        if (value) {
            oneshotTimer = 0;
            oneshotPlaying = true;
        }
        else {
            oneshotPlaying = false;
        }
        //buffer to try and prevent the first frame being shown again briefly before the animation ends
        double buffer = 0.05;
        return FrameCount / Fps - buffer;
    }

    //supports changing the color of the sprite based on color param
    public void Draw(SpriteBatch spriteBatch, Texture2D texture, Point position, Color color)
    {
        Rectangle currentSource = GetCurrentSourceRect();

        //calculate destination rect based on position, size, and anchor
        Vector2 destRectSize = new(SourceRect.Width * Size, SourceRect.Height * Size);
        Point destRectPos = position - (destRectSize * GetNormalizedAnchorOffset(Anchor)).ToPoint();
        Rectangle destinationRect = new(destRectPos, destRectSize.ToPoint());

        //draw the sprite
        spriteBatch.Draw(texture, destinationRect, currentSource, color);
    }

    //supports slicing the sprite horizontally based on progressNormalized param
    public void Draw(SpriteBatch spriteBatch, Texture2D texture, Point position, float progressNormalized)
    {
        Rectangle currentSource = GetCurrentSourceRect();
        currentSource.Width = (int) (currentSource.Width * progressNormalized);

        //calculate destination rect based on position, size, anchor, and progress
        Vector2 destRectSize = new(SourceRect.Width * Size * progressNormalized, SourceRect.Height * Size);
        Point destRectPos = position - (destRectSize * GetNormalizedAnchorOffset(Anchor)).ToPoint();
        Rectangle destinationRect = new(destRectPos, destRectSize.ToPoint());

        //draw the sprite
        spriteBatch.Draw(texture, destinationRect, currentSource, Color.White);
    }

    private Rectangle GetCurrentSourceRect() {
        //calculate source rect based on gameTime to support animation
        double animationTime;
        if (oneshotPlaying) {
            //oneshot timer
            animationTime = oneshotTimer;
            oneshotTimer += MonoZeldaGame.GameTime.ElapsedGameTime.TotalSeconds;
            //end oneshot animation 
            if (oneshotTimer >= FrameCount / Fps) {
                oneshotPlaying = false;
            }
        }
        else if (Animating) {
            //global timer
            animationTime = MonoZeldaGame.GameTime.TotalGameTime.TotalSeconds;
        }
        else {
            //not animating
            animationTime = 0;
        }
        int sourceX = SourceRect.X + (int) (animationTime * Fps % FrameCount) * SourceRect.Width;
        return new(sourceX, SourceRect.Y, SourceRect.Width, SourceRect.Height);
    }

    private static Vector2 GetNormalizedAnchorOffset(AnchorType anchorType)
    {
        return anchorType switch
        {
            AnchorType.TopLeft => Vector2.Zero,
            AnchorType.TopRight => new Vector2(1f, 0f),
            AnchorType.BottomLeft => new Vector2(0f, 1f),
            AnchorType.BottomRight => new Vector2(1f, 1f),
            AnchorType.Center => new Vector2(.5f, .5f),
            AnchorType.TopQuarter => new Vector2(.5f, .25f),
            AnchorType.LeftQuarter => new Vector2(.25f, .5f),
            AnchorType.BottomQuarter => new Vector2(.5f, .75f),
            AnchorType.RightQuarter => new Vector2(.75f, .5f),
            _ => Vector2.Zero,
        };
    }

    public static AnchorType StringToAnchorType(string s)
    {
        string lower = s.ToLower();
        return lower switch
        {
            "top left" => AnchorType.TopLeft,
            "top right" => AnchorType.TopRight,
            "bottom left" => AnchorType.BottomLeft,
            "bottom right" => AnchorType.BottomRight,
            "center" => AnchorType.Center,
            "top quarter" => AnchorType.TopQuarter,
            "left quarter" => AnchorType.LeftQuarter,
            "bottom quarter" => AnchorType.BottomQuarter,
            "right quarter" => AnchorType.RightQuarter,
            _ => AnchorType.TopLeft,
        };
    }
}


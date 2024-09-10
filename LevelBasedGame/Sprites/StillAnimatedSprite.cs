﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class StillAnimatedSprite : ISprite
{
    private int currentFrame;
    private int totalFrames;
    private int frameClock;
    private int currentframeClock;
    public Texture2D Texture { get; set; }
    public int Columns { get; set; }

    public StillAnimatedSprite(Texture2D texture, int columns, int frameClock)
    {
        Texture = texture;
        Columns = columns;
        currentFrame = 0;
        totalFrames = columns;
        this.frameClock = frameClock;
        currentframeClock = frameClock;
    }

    public void Update()
    {
        currentframeClock--;
        if (currentframeClock == 0)
        {
            currentframeClock = frameClock;
            currentFrame++;
            if (currentFrame == totalFrames)
                currentFrame = 0;
        }
        
    }
    public void Draw(SpriteBatch spriteBatch, Vector2 location)
    {
        int width = Texture.Width / Columns;
        int height = Texture.Height;
        int column = currentFrame % Columns;

        Rectangle sourceRectangle = new Rectangle(width * column, 0, width, height);
        Rectangle destinationRectangle = new Rectangle((int)location.X, (int)location.Y, width, height);

        spriteBatch.Draw(Texture, destinationRectangle, sourceRectangle, Color.White);
    }
}

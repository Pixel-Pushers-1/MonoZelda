﻿using Microsoft.VisualBasic.FileIO;
using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace MonoZelda.Sprites;

internal static class SpriteSheetParser
{
    public static Dictionary<string, Sprite> Parse(string csvName)
    {
        Dictionary<string, Sprite> dict = new();

        //set up text parser
        using TextFieldParser textFieldParser = new(csvName);
        textFieldParser.TextFieldType = FieldType.Delimited;
        textFieldParser.SetDelimiters(",");

        //throw out header row
        textFieldParser.ReadFields();

        //loop through csv file
        while (!textFieldParser.EndOfData)
        {
            string[] fields = textFieldParser.ReadFields();
            dict.Add(fields[0], ParseSprite(fields));
        }
        return dict;
    }

    private static Sprite ParseSprite(string[] fields)
    {
        int x = int.Parse(fields[1]);
        int y = int.Parse(fields[2]);
        int width = int.Parse(fields[3]);
        int height = int.Parse(fields[4]);
        int frameCount = int.Parse(fields[5]);
        Sprite.AnchorType anchor = Sprite.StringToAnchorType(fields[6]);
        float fps = float.Parse(fields[7]);
        Rectangle sourceRect = new(x, y, width, height);
        return new Sprite(sourceRect, anchor, frameCount, fps);
    }
}


using Microsoft.VisualBasic.FileIO;
using Microsoft.Xna.Framework;

namespace MonoZelda.Sprites;

internal static class SpriteSheetParser
{
    public static void Parse(SpriteDict toPopulate, string csvName)
    {
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
            toPopulate.Add(ParseSprite(fields), fields[0]);
        }
    }

    private static Sprite ParseSprite(string[] fields)
    {
        //scale up source rect data by 4 since image is upscaled by 4
        int x = int.Parse(fields[1]) * 4;
        int y = int.Parse(fields[2]) * 4;
        int width = int.Parse(fields[3]) * 4;
        int height = int.Parse(fields[4]) * 4;
        int frameCount = int.Parse(fields[5]);
        Sprite.AnchorType anchor = Sprite.StringToAnchorType(fields[6]);
        Rectangle sourceRect = new(x, y, width, height);
        return new Sprite(sourceRect, anchor, frameCount);
    }
}


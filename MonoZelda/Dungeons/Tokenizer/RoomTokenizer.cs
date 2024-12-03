using Microsoft.VisualBasic.FileIO;
using MonoZelda.Dungeons.Loader;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonoZelda.Dungeons.Parser
{
    internal class RoomTokenizer : IRoomTokenizer
    {
        public RoomFile Tokenize(Stream stream)
        {
            
            
            using var streamReader = new StreamReader(stream);
            using TextFieldParser textFieldParser = new TextFieldParser(streamReader);
            textFieldParser.TextFieldType = FieldType.Delimited;
            textFieldParser.SetDelimiters(",");

            if (textFieldParser.EndOfData) return null;

            var file = new RoomFile();

            TokenizeHeader(textFieldParser, file);
            TokenizeContent(textFieldParser, file);

            return file;
        }

        private static void TokenizeHeader(TextFieldParser textFieldParser, RoomFile file)
        {
            var fields = textFieldParser.ReadFields();

            file.RoomSprite = fields[0];
            file.NorthDoor = fields[1];
            file.EastDoor = fields[2];
            file.SouthDoor = fields[3];
            file.WestDoor = fields[4];

            // field 5 may be blank
            if (fields.Length > 5 && !string.IsNullOrEmpty(fields[5]))
            {
                file.IsLit = fields[5].ToUpper() == "LIT";
            }

        }

        private static void TokenizeContent(TextFieldParser textFieldParser, RoomFile file)
        {
            var content = new List<List<string>>();

            while (!textFieldParser.EndOfData)
            {
                var row = textFieldParser.ReadFields().ToList();
                content.Add(row);
            }

            file.Content = content;
        }
    }
}

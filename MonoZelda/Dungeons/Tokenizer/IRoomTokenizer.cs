using Microsoft.VisualBasic.FileIO;
using MonoZelda.Dungeons.Loader;
using System.IO;

namespace MonoZelda.Dungeons.Parser
{
    public interface IRoomTokenizer
    {
        public RoomFile Tokenize(Stream stream);
    }
}

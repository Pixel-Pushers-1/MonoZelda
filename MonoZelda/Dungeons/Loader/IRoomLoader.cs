
using Microsoft.VisualBasic.FileIO;
using MonoZelda.Dungeons.Parser;
using System.IO;

namespace MonoZelda.Dungeons.Loader
{
    public interface IRoomLoader
    {
        Stream Load(string roomName);
    }
}

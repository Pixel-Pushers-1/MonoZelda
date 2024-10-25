using Microsoft.Xna.Framework;
using MonoZelda.Dungeons.Loader;

namespace MonoZelda.Dungeons.Parser
{
    internal interface ICellParser
    {
        void Parse(string cell, Point position, DungeonRoom room);
    }
}

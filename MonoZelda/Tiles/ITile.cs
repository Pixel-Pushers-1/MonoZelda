using Microsoft.Xna.Framework;

namespace MonoZelda.Tiles;

internal interface ITile
{
    Point Position { get; set; }
    bool IsPassable { get; set; }
    void SetSprite(string name);
}

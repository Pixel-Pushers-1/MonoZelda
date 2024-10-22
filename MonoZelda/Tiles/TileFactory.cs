using Microsoft.Xna.Framework;
using MonoZelda.Sprites;

namespace MonoZelda.Tiles;

internal static class TileFactory 
{
    public static ITile CreateTile<T>(SpriteDict spriteDictionary, BlockType sprite, Point position) where T : TileBase, new()
    {
        ITile tile = new T()
        {
            Position = position
        };

        if (spriteDictionary != null)
        {
            tile.UsesSpriteDictionary(spriteDictionary);
            tile.SetSprite(sprite.ToString());
        }

        return tile;
    }

    public static ITile UsesSpriteDictionary(this ITile tile, SpriteDict d)
    {
        if (tile is TileBase b)
        {
            b.SetSpriteDict(d);
        }
        return tile;
    }
}
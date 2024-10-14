using PixelPushers.MonoZelda.Sprites;
using Microsoft.Xna.Framework;

namespace PixelPushers.MonoZelda.Items.ItemClasses;

public interface IItem
{
    public bool ItemPickedUp { get; set; }
    void itemSpawn(SpriteDict itemDict, Point  spawnPosition);
}


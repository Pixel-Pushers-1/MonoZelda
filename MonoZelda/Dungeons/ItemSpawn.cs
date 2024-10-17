using Microsoft.Xna.Framework;
using MonoZelda.Items;

namespace MonoZelda.Dungeons
{
    public class ItemSpawn
    {
        public Point Position { get; set; }
        public ItemList ItemType { get; set; }
        
        public ItemSpawn(Point position, ItemList itemType)
        {
            Position = position;
            ItemType = itemType;
        }

    }
}

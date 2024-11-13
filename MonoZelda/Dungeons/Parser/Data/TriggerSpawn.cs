using Microsoft.Xna.Framework;
using MonoZelda.Items;
using MonoZelda.Trigger;

namespace MonoZelda.Dungeons
{
    public class TriggerSpawn : RoomContent<TriggerType>
    {
        public TriggerSpawn(Point position, TriggerType type) : base(position, type)
        {
        }
    }
}

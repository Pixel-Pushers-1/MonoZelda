using Microsoft.Xna.Framework;
using MonoZelda.Trigger;
using System;

namespace MonoZelda.Dungeons.Parser
{
    internal class TriggerCellParser : ICellParser
    {
        public void Parse(string cell, Point position, DungeonRoom room)
        {
            if (Enum.TryParse(cell, out TriggerType trigger))
            {
                var triggerSpawn = new TriggerSpawn(position, trigger);
                room.AddTrigger(triggerSpawn);
            }
        }
    }
}

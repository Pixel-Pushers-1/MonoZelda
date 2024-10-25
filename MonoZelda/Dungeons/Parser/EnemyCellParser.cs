using Microsoft.Xna.Framework;
using MonoZelda.Enemies;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonoZelda.Dungeons.Parser
{
    internal class EnemyCellParser : ICellParser
    {
        public void Parse(string cell, Point position, DungeonRoom room)
        {
            if (Enum.TryParse(cell, out EnemyList enemy))
            {
                var enemySpawn = new EnemySpawn(position, enemy);
                room.AddEnemySpawn(enemySpawn);
            }
        }
    }
}

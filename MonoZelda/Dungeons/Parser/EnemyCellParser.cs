using Microsoft.Xna.Framework;
using MonoZelda.Dungeons.Parser.Data;
using MonoZelda.Enemies;
using System;
using System.Text.RegularExpressions;

namespace MonoZelda.Dungeons.Parser
{
    internal class EnemyCellParser : ICellParser
    {
        public void Parse(string cell, Point position, DungeonRoom room)
        {
            if (string.IsNullOrEmpty(cell)) return;

            var hasKey = false;
            var enemyRegex = new Regex(@"Enemy\((\w+)(?:,\s*(true|false))?\)");
            var match = enemyRegex.Match(cell);

            if (match.Success)
            {
                cell = match.Groups[1].Value;
                hasKey = match.Groups[2].Success && bool.Parse(match.Groups[2].Value);
            }

            if (!Enum.TryParse(cell, out EnemyList enemy)) return;

            var enemySpawn = new EnemySpawn(position, enemy, hasKey);
            var nonColliderSpawn = new NonColliderSpawn(position);
            room.AddEnemySpawn(enemySpawn);
            room.AddNonColliderSpawn(nonColliderSpawn);
        }
    }
}

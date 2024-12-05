using Microsoft.Xna.Framework;
using MonoZelda.Enemies;
using System;

namespace MonoZelda.Dungeons
{
    [Serializable]
    public class EnemySpawn
    {
        public Point Position { get; set; }
        public EnemyList EnemyType { get; set; }
        public bool HasKey { get; set; }

        public EnemySpawn(Point position, EnemyList enemyType, bool hasKey = false)
        {
            Position = position;
            EnemyType = enemyType;
            HasKey = hasKey;
        }
    }
}

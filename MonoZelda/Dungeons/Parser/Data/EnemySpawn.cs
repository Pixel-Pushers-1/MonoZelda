using Microsoft.Xna.Framework;
using MonoZelda.Enemies;

namespace MonoZelda.Dungeons
{
    public class EnemySpawn
    {
        public Point Position { get; set; }
        public EnemyList EnemyType { get; set; }
        public bool HasKey { get; set; }

        public EnemySpawn(Point position, EnemyList enemyType, bool key = false)
        {
            Position = position;
            EnemyType = enemyType;
            HasKey = key;
        }
    }
}

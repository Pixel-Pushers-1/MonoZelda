

using Microsoft.Xna.Framework;
using MonoZelda.Enemies;

namespace MonoZelda.Dungeons
{
    public class EnemySpawn
    {
        public Point Position { get; set; }
        public EnemyList EnemyType { get; set; }

        public EnemySpawn(Point position, EnemyList enemyType)
        {
            Position = position;
            EnemyType = enemyType;
        }
    }
}

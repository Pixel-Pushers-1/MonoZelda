using MonoZelda.Enemies;
using System.ComponentModel;

namespace MonoZelda.Dungeons.InfiniteMode;

public class RoomEnemyGenerator
{
    private const int EASY_ENEMY_POOL_LENGTH = 2;
    private const int MEDIUM_ENEMY_POOL_LENGTH = 3;
    private const int HARD_ENEMY_POOL_LENGTH = 1;
    private static readonly EnemyList[] EasyEnemyList = { EnemyList.Keese, EnemyList.Gel };
    private static readonly EnemyList[] MediumEnemyList = { EnemyList.Goriya, EnemyList.Stalfos, EnemyList.Dodongo, EnemyList.Trap };
    private static readonly EnemyList[] HardEnemyList = { EnemyList.Aquamentus };
}

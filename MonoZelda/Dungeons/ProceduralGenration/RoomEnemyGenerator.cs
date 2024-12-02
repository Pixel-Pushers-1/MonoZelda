using MonoZelda.Enemies;
using System.Collections.Generic;
using System;

namespace MonoZelda.Dungeons.InfiniteMode;

public class RoomEnemyGenerator
{
    // constants
    private const int EASY_ENEMY_WEIGHT = 40;
    private const int MEDIUM_ENEMY_WEIGHT = 30;
    private const int HARD_ENEMY_WEIGHT = 20;
    private const int EASY_ENEMY_POOL_LENGTH = 2;
    private const int MEDIUM_ENEMY_POOL_LENGTH = 1;
    private const int HARD_ENEMY_POOL_LENGTH = 1;
    private const int BOSS_ENEMY_POOL_LENGTH = 1;
    private static readonly EnemyList[] EasyEnemyList = { EnemyList.Keese, EnemyList.Gel };
    private static readonly EnemyList[] MediumEnemyList = { EnemyList.Stalfos};
    private static readonly EnemyList[] HardEnemyList = { EnemyList.Goriya };
    private static readonly EnemyList[] BossEnemyList = { EnemyList.Aquamentus };

    // variables
    

    public RoomEnemyGenerator()
    {
        // empty
    }

    public List<EnemyList> GenerateEnemiesForRoom(int roomNumber, int playerLevel, int playerHealth)
    {
        List<EnemyList> enemies = new List<EnemyList>();
        Random random = new Random();

        // Calculate the total number of enemies
        int baseEnemyCount = 3;
        int totalEnemies = baseEnemyCount + (roomNumber / 2) + (playerLevel / 3);

        // Adjust for low health (fewer enemies for health <= 2)
        if (playerHealth <= 2)
        {
            totalEnemies = Math.Max(2, totalEnemies - 1); // Reduce enemies slightly
        }

        // Calculate weights for each difficulty level
        int easyWeight = 40;
        int mediumWeight = 40;
        int hardWeight = 20;

        // Adjust weights based on health
        if (playerHealth <= 2)
        {
            easyWeight += 20; 
            mediumWeight -= 10;
            hardWeight = 0;   
        }
        else if (playerHealth >= 5)
        {
            mediumWeight += 10; 
            hardWeight += 10;
        }

        int totalWeight = easyWeight + mediumWeight + hardWeight;

        // Generate enemies
        for (int i = 0; i < totalEnemies; i++)
        {
            int roll = random.Next(totalWeight);

            if (roll < easyWeight)
            {
                // Spawn an easy enemy
                enemies.Add(EasyEnemyList[random.Next(EASY_ENEMY_POOL_LENGTH)]);
            }
            else if (roll < easyWeight + mediumWeight)
            {
                // Spawn a medium enemy
                enemies.Add(MediumEnemyList[random.Next(MEDIUM_ENEMY_POOL_LENGTH)]);
            }
            else
            {
                // Spawn a hard enemy
                enemies.Add(HardEnemyList[random.Next(HARD_ENEMY_POOL_LENGTH)]);
            }
        }

        return enemies;
    }
}

using MonoZelda.Enemies;
using System.Collections.Generic;
using System;

namespace MonoZelda.Dungeons.InfiniteMode;

public class RoomEnemyGenerator
{
    private const int EASY_ENEMY_POOL_LENGTH = 2;
    private const int MEDIUM_ENEMY_POOL_LENGTH = 1;
    private const int HARD_ENEMY_POOL_LENGTH = 1;
    private static readonly EnemyList[] EasyEnemyList = { EnemyList.Keese, EnemyList.Gel };
    private static readonly EnemyList[] MediumEnemyList = { EnemyList.Stalfos};
    private static readonly EnemyList[] HardEnemyList = { EnemyList.Goriya };

    public RoomEnemyGenerator()
    {
        // empty
    }

    public List<EnemyList> GenerateEnemiesForRoom(int roomNumber, int playerLevel, int playerHealth)
    {
        List<EnemyList> enemies = new List<EnemyList>();
        Random random = new Random();

        // 1. Calculate the total number of enemies
        int baseEnemyCount = 3;
        int totalEnemies = baseEnemyCount + (roomNumber / 2) + (playerLevel / 3);

        // Adjust for low health (fewer enemies for health <= 2)
        if (playerHealth <= 2)
        {
            totalEnemies = Math.Max(2, totalEnemies - 1); // Reduce enemies slightly
        }

        // 2. Calculate weights for each difficulty level
        int easyWeight = 40;
        int mediumWeight = 40;
        int hardWeight = 20;

        // Adjust weights based on health
        if (playerHealth <= 2)
        {
            easyWeight += 20; // Favor easy enemies
            mediumWeight -= 10;
            hardWeight = 0;   // No hard enemies
        }
        else if (playerHealth >= 5)
        {
            mediumWeight += 10; // Favor medium enemies
            hardWeight += 10;   // Introduce more hard enemies
        }

        int totalWeight = easyWeight + mediumWeight + hardWeight;

        // 3. Generate enemies
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

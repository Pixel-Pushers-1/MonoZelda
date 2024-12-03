using MonoZelda.Enemies;
using System.Collections.Generic;
using System;
using MonoZelda.Link;

namespace MonoZelda.Dungeons.InfiniteMode;

public class RoomEnemyGenerator
{
    // constants
    private const int EASY_ENEMY_POOL_LENGTH = 2;
    private const int MEDIUM_ENEMY_POOL_LENGTH = 1;
    private const int HARD_ENEMY_POOL_LENGTH = 1;
    private const int ROOM_LEVEL_UP_COUNT = 5;
    private static readonly EnemyList[] EasyEnemyList = { EnemyList.Keese, EnemyList.Gel };
    private static readonly EnemyList[] MediumEnemyList = { EnemyList.Stalfos};
    private static readonly EnemyList[] HardEnemyList = { EnemyList.Goriya };

    // variables
    private int easyWeight;
    private int mediumWeight;
    private int hardWeight;
    private Random random;

    public RoomEnemyGenerator()
    {
        easyWeight = 40;
        mediumWeight = 30;
        hardWeight = 20;
        random = new Random();
    }

    public List<EnemyList> GenerateEnemiesForRoom(int roomNumber, int playerLevel, int playerHealth)
    {
        List<EnemyList> enemies = new List<EnemyList>();

        // get initial room enemy count
        int totalEnemies = GetRoomEnemyCount(roomNumber, playerLevel, playerHealth);

        // Adjust weights based on health and calculate total enemy weight
        AdjustEnemyWeights(playerHealth, playerLevel);
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

    private int GetRoomEnemyCount(int roomNumber, int playerLevel, int playerHealth)
    {
        // initial count for each is three
        int baseEnemyCount = 3;
        int totalEnemies = baseEnemyCount + (roomNumber / 2) + (playerLevel / 3);

        // adjust based on player health
        if(playerHealth > (PlayerState.MaxHealth / 2))
        {
            totalEnemies += random.Next(3);
        }
        else
        {
            totalEnemies -= random.Next(3);
        }

        return totalEnemies;
    }

    private void AdjustEnemyWeights(int playerHealth, int playerLevel)
    {
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
    }

    private void LevelUpEnemies(int roomNumber)
    {
        if(roomNumber % ROOM_LEVEL_UP_COUNT == 0)
        {
            // Level Up enemies
        }
    }
}

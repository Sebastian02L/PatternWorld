using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace ObjectPoolMinigame
{
    public class EnemiesManager
    {
        //Game logic variables
        GameObject playerHead;
        ObjectPool enemiesPool;
        List<Transform> spawnPoints;
        ObjectPoolRoundData minigameData;

        //Number of each enemy type in the match
        int numberOfWeaks;
        int numberOfStandards;
        int numberOfStrongs;

        //Auxiliar variables
        int defeatedEnemies = 0;
        Action<int> onEnemyDefeated;

        public EnemiesManager(ObjectPool enemiesPool, List<Transform> spawnPoints, ObjectPoolRoundData minigameData, Action<int> onEnemyDefeated)
        {
            this.enemiesPool = enemiesPool;
            this.spawnPoints = spawnPoints;
            this.minigameData = minigameData;
            this.onEnemyDefeated = onEnemyDefeated;

            //Calculates the number of each enemy type using the proportion and total enemies number
            numberOfWeaks = Mathf.FloorToInt((float)minigameData.numberOfEnemies * minigameData.weakEnemyProportion);
            numberOfStandards = Mathf.FloorToInt((float)minigameData.numberOfEnemies * minigameData.standardEnemyProportion);
            numberOfStrongs = Mathf.FloorToInt((float)minigameData.numberOfEnemies * minigameData.strongEnemyProportion);

            playerHead = GameObject.FindWithTag("Player").transform.Find("Head").gameObject;
        }

        //Spawns the first wave of enemies at the beggining of the round
        public void FirstEnemiesSpawn()
        {
            for (int i = 0; i < numberOfWeaks; i++) 
            {
                IPoolableObject enemy = enemiesPool.Get();
                FirstEnemySetUp(enemy, minigameData.weakEnemyData);
            }
            for (int i = 0; i < numberOfStandards; i++)
            {
                IPoolableObject enemy = enemiesPool.Get();
                FirstEnemySetUp(enemy, minigameData.standardEnemyData);
            }
            for (int i = 0; i < numberOfStrongs; i++)
            {
                IPoolableObject enemy = enemiesPool.Get();
                FirstEnemySetUp(enemy, minigameData.strongEnemyData);
            }
        }
        //First enemy set up, passing the enemy data, enemy pool and EnemiesManager references
        void FirstEnemySetUp(IPoolableObject enemy, EnemyData enemyData)
        {
            (enemy as EnemyBrain).SetEnemyData(enemyData);
            (enemy as EnemyBrain).SetEnemiesPool(enemiesPool);
            (enemy as EnemyBrain).SetEnemiesManager(this);
            enemy.GetGameObject().transform.position = ChooseSpawnPoint();
            enemy.GetGameObject().SetActive(true);
        }

        //Spawn enemies during the duration of the round. Called when the player eliminates an enemy
        public void SpawnEnemy(EnemyData lastEnemyData)
        {
            defeatedEnemies += 1;
            onEnemyDefeated(defeatedEnemies);

            IPoolableObject enemy = enemiesPool.Get();
            ResetEnemy(enemy, lastEnemyData);
            (enemy as EnemyBrain).SetUpBehaviour();
        }
        //SetUp the spawned enemy with the data of the eliminated one.
        void ResetEnemy(IPoolableObject enemy, EnemyData enemyData)
        {
            (enemy as EnemyBrain).SetEnemyData(enemyData);
            enemy.GetGameObject().transform.position = ChooseSpawnPoint();
            enemy.GetGameObject().SetActive(true);
        }

        //Calculates the spawn poitn of the enemy
        Vector3 ChooseSpawnPoint()
        {
            Transform spawnPoint = null;
            Vector3 playerSpawnVector;
            bool found = false;

            //Search an spawn point that cant be seen by the player or is far away of him
            while (!found) 
            {
                spawnPoint = spawnPoints[UnityEngine.Random.Range(0, spawnPoints.Count)];
                playerSpawnVector = (playerHead.transform.position - spawnPoint.transform.position);
                if (Vector3.Angle(playerHead.transform.forward, playerSpawnVector) > 180f || playerSpawnVector.magnitude > 10f) 
                {
                    found = true;
                }
            }
            return spawnPoint.position;
        }
    }
}

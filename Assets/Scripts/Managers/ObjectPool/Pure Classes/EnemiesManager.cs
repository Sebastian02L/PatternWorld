using System;
using System.Collections.Generic;
using UnityEngine;

namespace ObjectPoolMinigame
{
    public class EnemiesManager
    {
        GameObject playerHead;
        ObjectPool enemiesPool;
        List<Transform> spawnPoints;
        ObjectPoolRoundData minigameData;

        int numberOfWeaks;
        int numberOfStandards;
        int numberOfStrongs;
        int defeatedEnemies;
        Action<int> onEnemyDefeated;

        public EnemiesManager(ObjectPool enemiesPool, List<Transform> spawnPoints, ObjectPoolRoundData minigameData, Action<int> onEnemyDefeated)
        {
            this.enemiesPool = enemiesPool;
            this.spawnPoints = spawnPoints;
            this.minigameData = minigameData;
            this.onEnemyDefeated = onEnemyDefeated;

            numberOfWeaks = Mathf.FloorToInt((float)minigameData.numberOfEnemies * minigameData.weakEnemyProportion);
            numberOfStandards = Mathf.FloorToInt((float)minigameData.numberOfEnemies * minigameData.standardEnemyProportion);
            numberOfStrongs = Mathf.FloorToInt((float)minigameData.numberOfEnemies * minigameData.strongEnemyProportion);

            playerHead = GameObject.FindWithTag("Player").transform.Find("Head").gameObject;
            defeatedEnemies = 0;

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
        void FirstEnemySetUp(IPoolableObject enemy, EnemyData enemyData)
        {
            (enemy as EnemyBrain).SetEnemyData(enemyData);
            SetEnemiesPool((enemy as EnemyBrain));
            SetEnemiesManager((enemy as EnemyBrain));
            enemy.GetGameObject().transform.position = ChooseSpawnPoint();
            enemy.GetGameObject().SetActive(true);
        }

        //Spawn enemies during the duration of the round
        public void SpawnEnemy(EnemyData lastEnemyData)
        {
            defeatedEnemies += 1;
            onEnemyDefeated(defeatedEnemies);
            IPoolableObject enemy = enemiesPool.Get();
            ResetEnemy(enemy, lastEnemyData);
            (enemy as EnemyBrain).SetUpBehaviour();
        }
        void ResetEnemy(IPoolableObject enemy, EnemyData enemyData)
        {
            (enemy as EnemyBrain).SetEnemyData(enemyData);
            enemy.GetGameObject().transform.position = ChooseSpawnPoint();
            enemy.GetGameObject().SetActive(true);
        }

        void SetEnemiesPool(EnemyBrain enemy)
        {
            enemy.SetEnemiesPool(enemiesPool);
        }
        void SetEnemiesManager(EnemyBrain enemy)
        {
            enemy.SetEnemiesManager(this);
        }

        Vector3 ChooseSpawnPoint()
        {
            Transform spawnPoint = null;
            Vector3 playerSpawnVector;
            bool found = false;

            while (!found) 
            {
                spawnPoint = spawnPoints[UnityEngine.Random.Range(0, spawnPoints.Count)];
                playerSpawnVector = (playerHead.transform.position - spawnPoint.transform.position);
                if (Vector3.Angle(playerHead.transform.forward, playerSpawnVector) > 90f || playerSpawnVector.magnitude > 5f) 
                {
                    found = true;
                }
            }
            return spawnPoint.position;
        }
    }
}

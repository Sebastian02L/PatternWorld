using System.Collections.Generic;
using NUnit.Framework;
using Unity.VisualScripting;
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


        public EnemiesManager(ObjectPool enemiesPool, List<Transform> spawnPoints, ObjectPoolRoundData minigameData)
        {
            this.enemiesPool = enemiesPool;
            this.spawnPoints = spawnPoints;
            this.minigameData = minigameData;

            numberOfWeaks = Mathf.FloorToInt((float)minigameData.numberOfEnemies * minigameData.weakEnemyProportion);
            numberOfStandards = Mathf.FloorToInt((float)minigameData.numberOfEnemies * minigameData.standardEnemyProportion);
            numberOfStrongs = Mathf.FloorToInt((float)minigameData.numberOfEnemies * minigameData.strongEnemyProportion);

            playerHead = GameObject.FindWithTag("Player").transform.Find("Head").gameObject;

        }
        public void FirstEnemiesSpawn()
        {
            for (int i = 0; i < numberOfWeaks; i++) 
            {
                IPoolableObject enemy = enemiesPool.Get();
                SetUpEnemy(enemy, minigameData.weakEnemyData);
            }

            for (int i = 0; i < numberOfStandards; i++)
            {
                IPoolableObject enemy = enemiesPool.Get();
                SetUpEnemy(enemy, minigameData.standardEnemyData);
            }

            for (int i = 0; i < numberOfStrongs; i++)
            {
                IPoolableObject enemy = enemiesPool.Get();
                SetUpEnemy(enemy, minigameData.strongEnemyData);
            }
        }

        void SetUpEnemy(IPoolableObject enemy, EnemyData enemyData)
        {
            (enemy as EnemyBrain).SetEnemyData(enemyData);
            (enemy as EnemyBrain).SetEnemiesPool(this.enemiesPool);
            (enemy as EnemyBrain).SetEnemiesManager(this);
            enemy.GetGameObject().transform.position = ChooseSpawnPoint();
            enemy.GetGameObject().SetActive(true);
        }

        Vector3 ChooseSpawnPoint()
        {
            Transform spawnPoint = null;
            Vector3 playerSpawnVector;
            bool found = false;

            while (!found) 
            {
                spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Count)];
                playerSpawnVector = (playerHead.transform.position - spawnPoint.transform.position);
                if (Vector3.Angle(playerHead.transform.forward, playerSpawnVector) > 90f || playerSpawnVector.magnitude > 5f) 
                {
                    found = true;
                }
            }
            return spawnPoint.position;
        }
        public void SpawnEnemy(EnemyData lastEnemyData)
        {
            IPoolableObject enemy = enemiesPool.Get();
            SetUpEnemy(enemy, lastEnemyData);
            (enemy as EnemyBrain).SetUpBehaviour();
        }
    }
}

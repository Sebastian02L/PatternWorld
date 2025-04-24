using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace ObjectPoolMinigame
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField] List<Transform> spawnPoints;
        ObjectPoolRoundData minigameData;
        ObjectPool bulletsPool;
        ObjectPool enemiesPool;

        int currentRound = 1;

        WeaponManager weaponManager;
        EnemiesManager enemiesManager;

        bool firstTime = true;

        void Awake()
        {
            //Determinates the current round of the minigame
            List<bool> minigameRounds = PlayerDataManager.Instance.GetMinigameRounds()[2];
            foreach (bool succededRound in minigameRounds) if (succededRound) currentRound += 1;
            if (currentRound > 3) currentRound = 3;

            //Loads the round configuration
            minigameData = Resources.Load<ObjectPoolRoundData>(SceneManager.GetActiveScene().name + "/" + currentRound);

            weaponManager = GetComponent<WeaponManager>();
            weaponManager.SetWeaponsData(minigameData.weaponsData);

            CreateBulletPool();
            CreateEnemiesPool();

            enemiesManager = new EnemiesManager(enemiesPool, spawnPoints, minigameData);
        }

        void CreateBulletPool()
        {
            int numberOfBullets = 0;

            foreach (WeaponData playerWeapon in minigameData.weaponsData)
            {
                if(playerWeapon.objectPoolRequired) numberOfBullets += playerWeapon.maxAmmo;
            }

            numberOfBullets += minigameData.weakEnemyData.weapon.maxAmmo * Mathf.CeilToInt((float)(minigameData.numberOfEnemies * minigameData.weakEnemyProportion));
            if (minigameData.standardEnemyData != null) numberOfBullets += minigameData.standardEnemyData.weapon.maxAmmo * Mathf.CeilToInt((float)(minigameData.numberOfEnemies * minigameData.standardEnemyProportion));
            if (minigameData.strongEnemyData != null) numberOfBullets += minigameData.strongEnemyData.weapon.maxAmmo * Mathf.CeilToInt((float)(minigameData.numberOfEnemies * minigameData.strongEnemyProportion));
            
            bulletsPool = new ObjectPool(numberOfBullets, minigameData.genericBullet);
        }

        void CreateEnemiesPool()
        {
            int numberOfEnemies = Mathf.CeilToInt((float)minigameData.numberOfEnemies + (minigameData.numberOfEnemies / 2));
            enemiesPool = new ObjectPool(numberOfEnemies, minigameData.genericEnemy);  
        }

        // Update is called once per frame
        void Update()
        {
            if (firstTime)
            {
                enemiesManager.FirstEnemiesSpawn();
                firstTime = false;
            }
        }

        public ObjectPool GetBulletsPool()
        {
            return bulletsPool;
        }
    }
}

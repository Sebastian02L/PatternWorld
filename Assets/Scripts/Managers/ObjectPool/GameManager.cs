using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace ObjectPoolMinigame
{
    public class GameManager : MonoBehaviour
    {
        ObjectPoolRoundData minigameData;
        ObjectPool bulletsPool;
        ObjectPool enemiesPool;

        int currentRound = 1;

        WeaponManager weaponManager;

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
            //CreateEnemiesPool();
            
        }

        void CreateBulletPool()
        {
            int numberOfBullets = 0;

            foreach (WeaponData playerWeapon in minigameData.weaponsData)
            {
                numberOfBullets += playerWeapon.maxAmmo;
            }

            numberOfBullets += minigameData.weakEnemyData.weapon.maxAmmo * Mathf.CeilToInt((float)(minigameData.numberOfEnemies * minigameData.weakEnemyProportion));
            if (minigameData.standardEnemyData != null) numberOfBullets += minigameData.standardEnemyData.weapon.maxAmmo * Mathf.CeilToInt((float)(minigameData.numberOfEnemies * minigameData.standardEnemyProportion));
            if (minigameData.strongEnemyData != null) numberOfBullets += minigameData.strongEnemyData.weapon.maxAmmo * Mathf.CeilToInt((float)(minigameData.numberOfEnemies * minigameData.strongEnemyProportion));
            
            bulletsPool = new ObjectPool(numberOfBullets, minigameData.genericBullet);
        }

        // Update is called once per frame
        void Update()
        {

        }

        public ObjectPool GetBulletsPool()
        {
            return bulletsPool;
        }
    }
}

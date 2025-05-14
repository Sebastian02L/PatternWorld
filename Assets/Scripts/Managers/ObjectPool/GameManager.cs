using System;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

namespace ObjectPoolMinigame
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField] List<Transform> spawnPoints;
        ObjectPoolRoundData minigameData;

        //References to other Managers
        WeaponManager playerWeaponManager;
        EnemiesManager enemiesManager;
        HealthManager healthManager;
        PlayerInput playerInput;

        //Auxiliar variables
        public event Action<int, int> OnEnemyDefeated;
        int currentRound = 1;
        bool firstTime = true;
        ObjectPool bulletsPool;
        ObjectPool enemiesPool;

        void Awake()
        {
            //Determinates the current round of the minigame
            List<bool> minigameRounds = PlayerDataManager.Instance.GetMinigameRounds()[2];
            foreach (bool succededRound in minigameRounds) if (succededRound) currentRound += 1;
            if (currentRound > 3) currentRound = 3;

            //Loads the round configuration
            minigameData = Resources.Load<ObjectPoolRoundData>(SceneManager.GetActiveScene().name + "/" + currentRound);

            //Send the player weapons data to WeaponManager
            playerWeaponManager = GetComponent<WeaponManager>();
            playerWeaponManager.SetWeaponsData(minigameData.weaponsData);

            //Setup the player's HealthManager
            healthManager = GetComponent<HealthManager>();
            healthManager.OnGetDamage += OnPlayerDamage;
            healthManager.SetMaxHeahlt(100);

            //Creates the enemy pool and setup the EnemiesManager
            CreateEnemiesPool();
            enemiesManager = new EnemiesManager(enemiesPool, spawnPoints, minigameData, EnemyDefeated);

            playerInput = GetComponent<PlayerInput>();
            CreateBulletPool();
        }

        //Creates the generic bullets pool to be used by the player and the enemies
        void CreateBulletPool()
        {
            int numberOfBullets = 0;

            foreach (WeaponData playerWeapon in minigameData.weaponsData)
            {
                if(playerWeapon.objectPoolRequired) numberOfBullets += playerWeapon.maxAmmo;
            }

            if (minigameData.weakEnemyData != null) numberOfBullets += minigameData.weakEnemyData.weapon.maxAmmo * Mathf.CeilToInt((float)(minigameData.numberOfEnemies * minigameData.weakEnemyProportion));
            if (minigameData.standardEnemyData != null) numberOfBullets += minigameData.standardEnemyData.weapon.maxAmmo * Mathf.CeilToInt((float)(minigameData.numberOfEnemies * minigameData.standardEnemyProportion));
            if (minigameData.strongEnemyData != null) numberOfBullets += minigameData.strongEnemyData.weapon.maxAmmo * Mathf.CeilToInt((float)(minigameData.numberOfEnemies * minigameData.strongEnemyProportion));
            
            bulletsPool = new ObjectPool(numberOfBullets, minigameData.genericBullet);
        }

        //Creates the generic enemie pool to be used by the EnemiesManager
        void CreateEnemiesPool()
        {
            int numberOfEnemies = Mathf.CeilToInt((float)minigameData.numberOfEnemies + (minigameData.numberOfEnemies / 2));
            enemiesPool = new ObjectPool(numberOfEnemies, minigameData.genericEnemy);  
        }

        //In the first frame, the enemies are Spawned and the PlayerCanvas is updated showing "0 / X enemies to eliminate"
        void Update()
        {
            if (firstTime)
            {
                enemiesManager.FirstEnemiesSpawn();
                firstTime = false;
                OnEnemyDefeated?.Invoke(0, minigameData.enemiesToEliminate);
            }
        }

        public ObjectPool GetBulletsPool()
        {
            return bulletsPool;
        }

        //Called when the player gets damage.
        void OnPlayerDamage(bool isPlayerEliminated)
        {
            if (isPlayerEliminated)
            {
                GameOver(false);
            }
            else 
            {
                AudioManager.Instance.PlaySoundEffect(GetComponent<AudioSource>(), "OPM_PlayerHitted", 0.5f);
            }
        }

        //Called when one enemy is defeated by the player.
        void EnemyDefeated(int defeatedEnemies)
        {
            OnEnemyDefeated?.Invoke(defeatedEnemies, minigameData.enemiesToEliminate);

            if (defeatedEnemies == (minigameData.enemiesToEliminate / 2)) playerWeaponManager.ChangeWeapon(playerInput);
            if (defeatedEnemies == minigameData.enemiesToEliminate) GameOver(true);
        }

        //Called when the players wins or lose the round
        void GameOver(bool hasWon)
        {
            //Deactivate players actions and movements
            playerWeaponManager.CancelShoot();
            playerInput.actions.Disable();
            gameObject.GetComponent<CharacterController>().enabled = false;

            if (hasWon) 
            {
                List<bool> newMinigameData = new List<bool>();
                for (int i = 0; i < 3; i++)
                {
                    if (i < currentRound) newMinigameData.Add(true);
                    else newMinigameData.Add(false);
                }
                //Save data
                PlayerDataManager.Instance.SetMinigameRound(2, newMinigameData);
            }

            GameObject.FindAnyObjectByType<EndGameController>().EnablePanel(hasWon, currentRound);
            CursorVisibility.ShowCursor();
        }
    }
}

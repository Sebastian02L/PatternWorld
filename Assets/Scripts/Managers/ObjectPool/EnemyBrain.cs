using System.Security.Cryptography;
using ObserverMinigame;
using Unity.AI.Navigation;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

namespace ObjectPoolMinigame 
{
    public class EnemyBrain : MonoBehaviour, IContext, IPoolableObject
    {
        EnemyData enemyData;
        [SerializeField] MeshRenderer gunRenderer;
        [SerializeField] EnemyGunManager gunManager;
        [SerializeField] GameObject enemyHead;
        HealthManager healthManager;
        IState currentState;

        GameObject player;
        GameObject playerHead;
        bool settedUp = false;

        ObjectPool enemiesPool;
        EnemiesManager enemiesManager;

        public bool IsDirty { get; set; }

        public IState GetState()
        {
            return currentState;
        }

        public void SetState(IState state)
        {
            if (currentState != null)
            {
                currentState.Exit();
                currentState = state;
                currentState.Enter();
            }
            else
            {
                currentState = state;
                currentState.Enter();
            }
        }

        void Start()
        {
            player = GameObject.FindWithTag("Player");
            playerHead = player.transform.Find("Head").gameObject;
            GetComponent<WaypointsManager>().SetWaypoints(GameObject.FindAnyObjectByType<NavMeshSurface>().GetComponent<NavMeshWaypointManager>().GetWaypoints());
            healthManager = GetComponent<HealthManager>();
            healthManager.OnHealthChange += OnGetDamaged;
            TutorialController.OnTutorialClosed += SetUpBehaviour;
            gameObject.SetActive(false);
        }

        private void OnDestroy()
        {
            healthManager.OnHealthChange -= OnGetDamaged;
            TutorialController.OnTutorialClosed -= SetUpBehaviour;
        }

        public void SetUpBehaviour()
        {
            if (!gameObject.activeSelf) return;
            SetState(new WanderState(this, player, gameObject, enemyData, GetComponent<Animator>(), playerHead, enemyHead, gunManager));
            healthManager.SetMaxHeahlt(enemyData.maxHealht);
            Material material = new Material(gunRenderer.material);
            material.mainTexture = enemyData.gunTexture;
            gunRenderer.material = material;
            settedUp = true;
        }

        void Update()
        {
            if (!settedUp) return;
            currentState.Update();
        }

        private void FixedUpdate()
        {
            if (!settedUp) return;
            currentState.FixedUpdate();
        }

        void OnGetDamaged(int state)
        {
            if (state == 0)
            {
                Debug.Log("ENEMIGO MUERTOOOO");
                Release();
            }
            else if (currentState.GetType() != typeof(CombatState) && currentState.GetType() != typeof(EscapeState))
            {
                SetState(new CombatState(this, enemyData, player, gameObject, GetComponent<Animator>(), playerHead, enemyHead, gunManager));
            }
        }

        public GameObject GetGameObject()
        {
            return gameObject;
        }

        public void Release()
        {
            gameObject.SetActive(false);
            enemiesPool.Release(this);
            enemiesManager.SpawnEnemy(enemyData);
        }

        public void SetEnemyData(EnemyData enemyData)
        {
            this.enemyData = enemyData;
            gunManager.SetWeaponData(enemyData.weapon);
            settedUp = false;
        }

        public void SetEnemiesPool(ObjectPool enemiesPool)
        {
            this.enemiesPool = enemiesPool;
        }

        public void SetEnemiesManager(EnemiesManager enemiesManager)
        {
            this.enemiesManager = enemiesManager;
        }
    }
}


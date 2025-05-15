using System.Collections.Generic;
using System.Linq;
using ObserverMinigame;
using Unity.AI.Navigation;
using UnityEngine;

namespace ObjectPoolMinigame 
{
    public class EnemyBrain : MonoBehaviour, IContext, IPoolableObject
    {
        [SerializeField] MeshRenderer gunRenderer;
        [SerializeField] EnemyGunManager gunManager;
        [SerializeField] GameObject enemyEyes;

        //Enemy behaviour logic variables
        EnemyData enemyData;
        HealthManager healthManager;
        ObjectPool enemiesPool;
        EnemiesManager enemiesManager;

        //Enemies awareness variables
        List<EnemyBrain> nearbyAllies = new List<EnemyBrain>();
        int nearbyAttentionDistance = 3;

        //ObjectPool and State related variables
        public bool IsDirty { get; set; }
        IState currentState;

        //Auxiliar variables
        GameObject player;
        GameObject playerHead;
        bool settedUp = false;

        //Returns the current state of the enemy FSM
        public IState GetState()
        {
            return currentState;
        }

        //Sets the next state of the enemy FSM
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

        //Send the enemy to the enemies pool and order a new one to spawn
        public void Release()
        {
            gameObject.SetActive(false);
            enemiesPool.Release(this);
            enemiesManager.SpawnEnemy(enemyData);
        }


        void Start()
        {
            player = GameObject.FindWithTag("Player");
            playerHead = player.transform.Find("Head").gameObject;

            //Search the Waypoints and send them to his WaypointManager
            GetComponent<WaypointsManager>().SetWaypoints(GameObject.FindAnyObjectByType<NavMeshSurface>().GetComponent<NavMeshWaypointManager>().GetWaypoints());
            
            //Setup the enemy HealthManager
            healthManager = GetComponent<HealthManager>();
            healthManager.OnGetDamage += OnGetDamaged;
            
            //When the tutorial get closed, the enemies start the behaviour
            TutorialController.OnTutorialClosed += SetUpBehaviour;
            gameObject.SetActive(false);
        }

        private void OnDestroy()
        {
            healthManager.OnGetDamage -= OnGetDamaged;
            TutorialController.OnTutorialClosed -= SetUpBehaviour;
        }

        //Setters
        public void SetEnemyData(EnemyData enemyData)
        {
            this.enemyData = enemyData;
            //Send the weapon data to the enemy GunManager
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

        //Set ups the enemy behaviour creating the frist state of the FSM
        public void SetUpBehaviour()
        {
            if (!gameObject.activeSelf) return;

            SetState(new WanderState(this, enemyData, GetComponent<Animator>(), playerHead, enemyEyes));
            healthManager.SetMaxHeahlt(enemyData.maxHealht);

            //Changes the enemies gun material
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

        //Called when the enemy gets damage
        void OnGetDamaged(bool isAgentEliminated)
        {
            AlertNearbyAllies();
            if (isAgentEliminated)
            {
                Release();
            }
            else if (currentState.GetType() != typeof(CombatState) && currentState.GetType() != typeof(EscapeState))
            {
                EnterCombatState();
            }
        }

        //Makes the enemy FSM to enter in CombatState
        public void EnterCombatState()
        {
            SetState(new CombatState(this, enemyData, GetComponent<Animator>(), playerHead, enemyEyes));
        }

        public GameObject GetGameObject()
        {
            return gameObject;
        }

        //Called when an enemy gets damage, calcultes which allies are nearby and make them enter in combat state.
        public void AlertNearbyAllies()
        {
            nearbyAllies.Clear();
            nearbyAllies = GameObject.FindObjectsByType<EnemyBrain>(FindObjectsInactive.Exclude, FindObjectsSortMode.None).ToList<EnemyBrain>();

            float distance = float.MaxValue;
            foreach (EnemyBrain allie in nearbyAllies)
            {
                if (allie != this && allie.GetState().GetType() != typeof(CombatState) && allie.GetState().GetType() != typeof(EscapeState))
                {
                    distance = (allie.transform.position - gameObject.transform.position).magnitude;
                    if (distance <= nearbyAttentionDistance) allie.EnterCombatState();
                    distance = float.MaxValue;
                }
            }
        }
    }
}


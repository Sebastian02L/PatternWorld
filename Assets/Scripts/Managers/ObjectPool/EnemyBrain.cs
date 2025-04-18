using ObserverMinigame;
using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.AI;

namespace ObjectPoolMinigame 
{
    public class EnemyBrain : MonoBehaviour, IContext
    {
        [SerializeField] EnemyData enemyData;
        [SerializeField] MeshRenderer gunRenderer;
        [SerializeField] EnemyGunManager gunManager;
        [SerializeField] GameObject enemyHead;
        IState currentState;

        GameObject player;
        GameObject playerHead;
        bool settedUp = false;

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
            TutorialController.OnTutorialClosed += SetUpBehaviour;
        }

        private void OnDestroy()
        {
            TutorialController.OnTutorialClosed -= SetUpBehaviour;
        }

        void SetUpBehaviour()
        {
            //List<EnemyData> behaviours = GameObject.FindAnyObjectByType<GameManager>().GetRoundData().floorDroneData;
            //droneData = behaviours[Random.Range(0, behaviours.Count)];
            Material material = new Material(gunRenderer.material);
            material.mainTexture = enemyData.gunTexture;
            gunRenderer.material = material;
            SetState(new WanderState(this, player, gameObject, enemyData, GetComponent<Animator>(), playerHead, enemyHead, gunManager));
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
    }
}


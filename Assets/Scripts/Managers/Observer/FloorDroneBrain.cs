using UnityEngine;

namespace ObserverMinigame 
{
    public class FloorDroneBrain : MonoBehaviour, IContext
    {
        [SerializeField] EnemyData droneData;
        Light sensor;
        IState currentState;

        GameObject player;

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

            SetState(new IdleState(this, droneData, player, gameObject));
            sensor = GetComponentInChildren<Light>();
            sensor.innerSpotAngle = droneData.FOV;
            sensor.spotAngle = droneData.FOV;
            sensor.range = droneData.visionDistance;
        }

        void Update()
        {
            currentState.Update();
        }

        private void FixedUpdate()
        {
            currentState.FixedUpdate();
        }
    }
}


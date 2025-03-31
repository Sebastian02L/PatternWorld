using UnityEngine;
using UnityEngine.AI;

namespace ObserverMinigame 
{
    public class TurretBrain : MonoBehaviour, IContext
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

        public void EvaluatePostMoveTransition()
        {
            float probability = Random.Range(0f, 1f);
            if (probability <= droneData.idleProbability)
            {
                SetState(new IdleState(this, droneData, player, gameObject));
            }
            else
            {
                SetState(new RotateState(this, droneData, player, gameObject));
            }
        }

        public void EvaluateInterruptionTransition()
        {
            float probability = Random.Range(0f, 1f);
            if (probability <= droneData.restartProbability)
            {
                SetState(new RestartState(this, droneData, player, gameObject));
            }
        }
    }
}


using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

namespace ObserverMinigame 
{
    public class SentinelBrain : MonoBehaviour, IContext
    {
        EnemyData droneData;
        Light sensor;
        IState currentState;

        GameObject player;
        public static event Action<int> OnPlayerInSight;
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
            TutorialController.OnTutorialClosed += SetUpBehaviour;
        }

        private void OnDestroy()
        {
            TutorialController.OnTutorialClosed -= SetUpBehaviour;
        }

        void SetUpBehaviour()
        {
            List<EnemyData> behaviours = GameObject.FindAnyObjectByType<GameManager>().GetRoundData().sentinelData;
            droneData = behaviours[Random.Range(0, behaviours.Count)];

            sensor = GetComponentInChildren<Light>();
            sensor.innerSpotAngle = droneData.FOV;
            sensor.spotAngle = droneData.FOV;
            sensor.range = droneData.visionDistance;
            SetState(new IdleState(this, droneData, player, gameObject, Notify));
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

        public void EvaluatePostMoveTransition()
        {
            float probability = Random.Range(0f, 1f);
            if (probability <= droneData.idleProbability)
            {
                SetState(new IdleState(this, droneData, player, gameObject, Notify));
            }
            else
            {
                SetState(new CheckHideSpotsState(this, droneData, player, gameObject, Notify));
            }
        }

        public void EvaluateInterruptionTransition()
        {
        }

        public void Notify(int barkState)
        {
            OnPlayerInSight?.Invoke(barkState);
        }
    }
}


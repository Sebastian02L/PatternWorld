using System;
using System.Collections;
using System.IO;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

namespace ObserverMinigame
{
    public class MoveState : AState
    {
        WaypointsManager waypointsManager;
        NavMeshAgent agent;
        float timer;


        public MoveState(IContext context, EnemyData agentData, GameObject player, GameObject agentGO, Action<int> notify) : base(context, player, agentGO, agentData, notify)
        {
            agent = agentGO.GetComponent<NavMeshAgent>();
            agent.angularSpeed = agentData.rotationSpeed;
            agent.speed = agentData.moveSpeed;
            waypointsManager = agentGameObject.GetComponent<WaypointsManager>();
            timer = 0f;
        }
        public override void Enter()
        {
            agent.isStopped = false;
            if (agentData.enemyType == EnemyData.EnemyType.Sentinel)
            {
                waypointsManager.CalculateRandomIndex();
                agent.SetDestination(waypointsManager.GetNextWaypoint().position);
            }
            else
            {
                agent.SetDestination(waypointsManager.GetNextWaypoint().position);
            }
            agentGameObject.GetComponent<SoundEffectsController>().Movement(false);
        }

        public override void Update()
        {
            if (CheckPlayerInFOV(agentData.FOV, agentData.visionDistance))
            {
                agent.isStopped = true;
                agentGameObject.GetComponent<SoundEffectsController>().Movement(true);
                context.SetState(new ShootPlayerState(context, agentData, player, agentGameObject, notify));
            }
            else
            {
                if (agent.pathPending) return;

                if (WaypointReached())
                {
                    agent.isStopped = true;
                    if (agentData.enemyType != EnemyData.EnemyType.Sentinel) waypointsManager.CalculateNextIndex();
                    RandomizeTransition();
                }
                else
                {
                    timer += Time.deltaTime;
                    if (timer >= 3f)
                    {
                        timer = 0f;
                        EvaluateInterruption();
                    }
                }
            }
        }

        bool WaypointReached()
        {
            return agent.remainingDistance <= agent.stoppingDistance;
        }

        void RandomizeTransition()
        {
            context.EvaluatePostMoveTransition();
        }

        void EvaluateInterruption()
        {
            context.EvaluateInterruptionTransition();
        }

        public override void Exit()
        {
        }
        public override void FixedUpdate()
        {
        }
    }
}

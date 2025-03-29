using UnityEngine;
using UnityEngine.AI;

namespace ObserverMinigame
{
    public class MoveState : AState
    {
        WaypointsManager waypointsManager;
        NavMeshAgent agent;
        float timer;


        public MoveState(IContext context, EnemyData agentData, GameObject player, GameObject agentGO) : base(context, player, agentGO, agentData)
        {
            agent = agentGO.GetComponent<NavMeshAgent>();
            agent.speed = agentData.moveSpeed;
            waypointsManager = agentGameObject.GetComponent<WaypointsManager>();
            timer = 0f;
        }
        public override void Enter()
        {
            agent.isStopped = false;
            agent.SetDestination(waypointsManager.GetNextWaypoint().position);
        }

        public override void Update()
        {
            if (CheckPlayerInFOV(agentData.FOV, agentData.visionDistance))
            {
                agent.isStopped = true;
                context.SetState(new ShootPlayerState(context, agentData, player, agentGameObject));
            }
            else
            {
                if (WaypointReached())
                {
                    waypointsManager.CalculateNextIndex();
                    RandomizeTransition();
                }
                else
                {
                    timer += Time.deltaTime;
                    if (timer >= 1f)
                    {
                        timer = 0f;
                        EvaluateTurnAroundTransition();
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
            float probability = Random.Range(0f, 1f);
            if (probability <= 0.25f)
            {
                context.SetState(new IdleState(context, agentData, player, agentGameObject));
            }
            else
            {
                context.SetState(new MoveState(context, agentData, player, agentGameObject));
            }
        }

        void EvaluateTurnAroundTransition()
        {
            float probability = Random.Range(0f, 1f);
            if (probability <= 0.25f)
            {
                agent.isStopped = true;
                context.SetState(new TurnAroundState(context, agentData, player, agentGameObject));
            }
        }

        public override void Exit()
        {
        }
        public override void FixedUpdate()
        {
        }
    }
}

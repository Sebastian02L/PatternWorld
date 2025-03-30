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

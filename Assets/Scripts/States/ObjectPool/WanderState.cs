using ObserverMinigame;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;
using UnityEngine.AI;

namespace ObjectPoolMinigame
{
    public class WanderState : AState
    {
        WaypointsManager waypointsManager;
        NavMeshAgent navMeshAgent;

        public WanderState(IContext context, GameObject player, GameObject agent, EnemyData agentData, Animator animator, GameObject playerHead, GameObject agentHead, EnemyGunManager gunManager) 
            :base(context, player, agent, agentData, animator, playerHead, agentHead, gunManager)
        {
            navMeshAgent = agentGameObject.GetComponent<NavMeshAgent>();
            navMeshAgent.speed = agentData.moveSpeed;
            navMeshAgent.angularSpeed = 360f;
            waypointsManager = agentGameObject.GetComponent<WaypointsManager>();   
        }

        public override void Enter()
        {
            navMeshAgent.isStopped = false;
            animator.SetTrigger("WanderState");
            waypointsManager.CalculateRandomIndex();
            Vector3 destiny = ExtractRandomPointFromSphere(waypointsManager.GetNextWaypoint().GetComponent<SphereCollider>());
            navMeshAgent.SetDestination(destiny);
        }

        Vector3 ExtractRandomPointFromSphere(SphereCollider collider)
        {
            Vector3 direccion = Random.insideUnitSphere; 
            return collider.transform.position + direccion * collider.radius * collider.transform.lossyScale.x;
        }

        bool PointReached()
        {
            return navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance;
        }

        public override void Update()
        {
            if (navMeshAgent.pathPending) return;

            if (CheckPlayerInFOV())
            {
                //agentGameObject.GetComponent<SoundEffectsController>().Movement(true);
                context.SetState(new CombatState(context, agentData, player, agentGameObject, animator, playerHead, agentHead, gunManager));
            }
            else
            {
                if (PointReached())
                {
                    context.SetState(new IdleState(context, agentData, player, agentGameObject, animator, playerHead, agentHead, gunManager));
                }
            }
            
        }

        public override void Exit()
        {
            navMeshAgent.isStopped = true;
        }

        public override void FixedUpdate()
        {
        }
    }
}

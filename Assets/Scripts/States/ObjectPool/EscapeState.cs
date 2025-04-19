using ObserverMinigame;
using UnityEngine;
using UnityEngine.AI;

namespace ObjectPoolMinigame
{
    public class EscapeState : AState
    {
        Transform[] waypoints;
        NavMeshAgent navMeshAgent;
        public EscapeState(IContext context, EnemyData agentData, GameObject player, GameObject agent, Animator animator, GameObject playerHead, GameObject agentHead, EnemyGunManager gunManager)
            : base(context, player, agent, agentData, animator, playerHead, agentHead, gunManager)
        {
        }

        public override void Enter()
        {
            Debug.Log("entrando al estado de huir");
            navMeshAgent = agentGameObject.GetComponent<NavMeshAgent>();
            navMeshAgent.isStopped = false;
            navMeshAgent.speed *= 2; 
            waypoints = agentGameObject.GetComponent<WaypointsManager>().GetWaypoints();

            Transform destiny = CalculateFardestTransform();
            navMeshAgent.SetDestination(destiny.transform.position);
            Debug.Log("destino fijado");

        }

        public override void Update()
        {
            if (navMeshAgent.pathPending) return;

            if (navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance)
            {
                context.SetState(new WanderState(context, player, agentGameObject, agentData, animator, playerHead, agentHead, gunManager));
            }
        }

        Transform CalculateFardestTransform()
        {
            Transform tranform = null;
            float fardestDistance = float.MinValue;

            foreach (Transform t in waypoints) 
            { 
                float distance = (player.transform.position - t.position).magnitude;
                if (distance > fardestDistance) 
                { 
                    tranform = t;
                    fardestDistance = distance;
                }
            }

            return tranform;
        }

        public override void Exit()
        {
            navMeshAgent.isStopped = true;
            navMeshAgent.speed /= 2;
        }

        public override void FixedUpdate()
        {
        }
    }
}

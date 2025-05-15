using ObserverMinigame;
using UnityEngine;
using UnityEngine.AI;

namespace ObjectPoolMinigame
{
    public class WanderState : AState
    {
        WaypointsManager waypointsManager;
        NavMeshAgent navMeshAgent;

        public WanderState(IContext context, EnemyData agentData, Animator animator, GameObject playerHead, GameObject agentEyes) 
            :base(context, agentData, animator, playerHead, agentEyes)
        {
            navMeshAgent = context.GetGameObject().GetComponent<NavMeshAgent>();
            navMeshAgent.speed = agentData.moveSpeed;
            navMeshAgent.angularSpeed = 360f;
            waypointsManager = context.GetGameObject().GetComponent<WaypointsManager>();   
        }

        //Calculates a random waypoint and start to walk to his area
        public override void Enter()
        {
            AudioManager.Instance.PlaySoundEffect(context.GetGameObject().GetComponent<AudioSource>(), "OPM_EnemyMoving", 1, true, true);
            navMeshAgent.isStopped = false;
            animator.SetTrigger("WanderState");
            waypointsManager.CalculateRandomIndex();
            Vector3 destiny = ExtractRandomPointFromSphere(waypointsManager.GetNextWaypoint().GetComponent<SphereCollider>());
            navMeshAgent.SetDestination(destiny);
        }

        //Calculates and return a random point inside the waypoint sphere collider
        Vector3 ExtractRandomPointFromSphere(SphereCollider collider)
        {
            Vector3 direccion = Random.insideUnitSphere; 
            return collider.transform.position + direccion * collider.radius * collider.transform.lossyScale.x;
        }

        //Returns if the current destiny has been reached
        bool PointReached()
        {
            return navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance;
        }

        public override void Update()
        {
            if (navMeshAgent.pathPending) return;

            if (CheckPlayerInFOV())
            {
                context.SetState(new CombatState(context, agentData, animator, playerHead, agentEyes));
            }
            else
            {
                if (PointReached())
                {
                    context.SetState(new IdleState(context, agentData, animator, playerHead, agentEyes));
                }
            }
        }

        public override void Exit()
        {
            navMeshAgent.isStopped = true;
            AudioManager.Instance.StopAudioSource(context.GetGameObject().GetComponent<AudioSource>());
        }

        public override void FixedUpdate()
        {
        }
    }
}

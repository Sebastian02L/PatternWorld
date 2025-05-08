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
            AudioManager.Instance.PlaySoundEffect(agentGameObject.GetComponent<AudioSource>(), "OPM_EnemyMoving", 1, false, true);
            Debug.Log("entrando al estado de huir");
            navMeshAgent = agentGameObject.GetComponent<NavMeshAgent>();
            navMeshAgent.isStopped = false;
            navMeshAgent.speed = agentData.runSpeed; 
            waypoints = agentGameObject.GetComponent<WaypointsManager>().GetWaypoints();

            Transform destinationZone = CalculateFardestTransform();
            Vector3 destiny = ExtractRandomPointFromSphere(destinationZone.GetComponent<SphereCollider>());
            navMeshAgent.SetDestination(destiny);
            Debug.Log("destino fijado");

        }
        Vector3 ExtractRandomPointFromSphere(SphereCollider collider)
        {
            Vector3 direccion = Random.insideUnitSphere;
            return collider.transform.position + direccion * collider.radius * collider.transform.lossyScale.x;
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
            AudioManager.Instance.StopAudioSource(agentGameObject.GetComponent<AudioSource>());
            navMeshAgent.isStopped = true;
            navMeshAgent.speed = agentData.moveSpeed;
        }

        public override void FixedUpdate()
        {
        }
    }
}

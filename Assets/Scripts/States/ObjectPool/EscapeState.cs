using ObserverMinigame;
using UnityEngine;
using UnityEngine.AI;

namespace ObjectPoolMinigame
{
    public class EscapeState : AState
    {
        Transform[] waypoints;
        NavMeshAgent navMeshAgent;

        public EscapeState(IContext context, EnemyData agentData, Animator animator, GameObject playerHead, GameObject agentEyes)
            : base(context, agentData, animator, playerHead, agentEyes)
        {
        }

        //The velocity of the agent increase and starts to calculate the fardest waypoint from the player
        public override void Enter()
        {
            AudioManager.Instance.PlaySoundEffect(context.GetGameObject().GetComponent<AudioSource>(), "OPM_EnemyMoving", 1, false, true);

            waypoints = context.GetGameObject().GetComponent<WaypointsManager>().GetWaypoints();
            navMeshAgent = context.GetGameObject().GetComponent<NavMeshAgent>();
            navMeshAgent.isStopped = false;
            navMeshAgent.speed = agentData.runSpeed; 

            Transform destinationZone = CalculateFardestTransform();
            Vector3 destiny = ExtractRandomPointFromSphere(destinationZone.GetComponent<SphereCollider>());
            navMeshAgent.SetDestination(destiny);
        }

        //Calculates and return a random point inside the waypoint sphere collider
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
                context.SetState(new WanderState(context, agentData, animator, playerHead, agentEyes));
            }
        }

        //Search the fardest waypoint from player position
        Transform CalculateFardestTransform()
        {
            Transform tranform = null;
            float fardestDistance = float.MinValue;

            foreach (Transform t in waypoints) 
            { 
                float distance = (playerHead.transform.parent.transform.position - t.position).magnitude;
                if (distance > fardestDistance) 
                { 
                    tranform = t;
                    fardestDistance = distance;
                }
            }

            return tranform;
        }

        //Sets the velocity of the agent to the normal one
        public override void Exit()
        {
            AudioManager.Instance.StopAudioSource(context.GetGameObject().GetComponent<AudioSource>());
            navMeshAgent.isStopped = true;
            navMeshAgent.speed = agentData.moveSpeed;
        }

        public override void FixedUpdate()
        {
        }
    }
}

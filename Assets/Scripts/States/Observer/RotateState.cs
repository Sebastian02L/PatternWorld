using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace ObserverMinigame
{
    public class RotateState : AState
    {
        WaypointsManager waypointsManager;
        bool rotationCompleted = false;
        float rotationSpeed;
        Quaternion nextLookDirection;
        float timer = 0f;

        public RotateState(IContext context, EnemyData agentData, GameObject player, GameObject agent, Action<int> notify) : base(context, player, agent, agentData, notify)
        {
            waypointsManager = agent.GetComponent<WaypointsManager>();
            rotationSpeed = agentData.rotationSpeed;

            Vector3 waypointTranform = waypointsManager.GetNextWaypoint().position;
            waypointTranform.y = 0;

            Vector3 agentTransform = agent.transform.position;
            agentTransform.y = 0;

            nextLookDirection = Quaternion.LookRotation(waypointTranform - agentTransform);
        }

        public override void Update()
        {
            if (CheckPlayerInFOV(agentData.FOV, agentData.visionDistance))
            {
                context.SetState(new ShootPlayerState(context, agentData, player, agentGameObject, notify));
            }
            else
            {
                if (!rotationCompleted)
                {
                    agentGameObject.transform.rotation = Quaternion.RotateTowards(agentGameObject.transform.rotation, nextLookDirection, rotationSpeed * Time.deltaTime);

                    float angle = Quaternion.Angle(agentGameObject.transform.rotation, nextLookDirection);
                    if (angle < 0.1f)
                    {
                        rotationCompleted = true;
                        waypointsManager.CalculateNextIndex();
                        context.EvaluatePostMoveTransition();
                    }
                }

                timer += Time.deltaTime;
                if (timer >= 1f)
                {
                    timer = 0f;
                    context.EvaluateInterruptionTransition();
                }
                
            }
        }

        public override void Enter()
        {
        }

        public override void Exit()
        {
        }

        public override void FixedUpdate()
        {
        }
    }
}

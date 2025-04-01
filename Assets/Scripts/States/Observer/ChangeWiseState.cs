using System;
using UnityEngine;

namespace ObserverMinigame
{
    public class ChangeWiseState : AState
    {
        WaypointsManager waypointsManager;
        Quaternion oppositeDirection;
        bool rotationCompleted = false;
        float rotationSpeed;

        public ChangeWiseState(IContext context, EnemyData agentData, GameObject player, GameObject agentGO, Action<int> notify) : base(context, player, agentGO, agentData, notify)
        {
            waypointsManager = agentGameObject.GetComponent<WaypointsManager>();
            oppositeDirection = Quaternion.LookRotation(-agentGameObject.transform.forward);
            rotationSpeed = agentData.rotationSpeed;
        }

        public override void Enter()
        {
            waypointsManager.ChangeWise();

        }

        public override void Exit()
        {
        }

        public override void FixedUpdate()
        {
        }

        public override void Update()
        {
            if (!rotationCompleted)
            {
                agentGameObject.transform.rotation = Quaternion.RotateTowards(agentGameObject.transform.rotation, oppositeDirection, rotationSpeed * Time.deltaTime);

                float angle = Quaternion.Angle(agentGameObject.transform.rotation, oppositeDirection);
                if (angle < 0.1f)
                {
                    rotationCompleted = true;
                    context.SetState(new MoveState(context, agentData, player, agentGameObject, notify));
                }
            }
        }
    }
}

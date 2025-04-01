using System;
using Unity.Collections;
using UnityEngine;
using Random = UnityEngine.Random;
namespace ObserverMinigame
{
    public class TurnAroundState : AState
    {
        Quaternion oppositeDirection;

        float rotationSpeed;
        bool rotationCompleted = false;
        int phase;

        float maxLookDurationTime;
        float timer;


        public TurnAroundState(IContext context, EnemyData agentData, GameObject player, GameObject agent, Action<int> notify) : base(context, player, agent, agentData, notify)
        {
            rotationSpeed = agentData.rotationSpeed;
            timer = 0f;
            phase = 0;

            maxLookDurationTime = Random.Range(1, agentData.turnedAroundLookTime);
            oppositeDirection = Quaternion.LookRotation(-agentGameObject.transform.forward);
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
                    agentGameObject.transform.rotation = Quaternion.RotateTowards(agentGameObject.transform.rotation, oppositeDirection, rotationSpeed * Time.deltaTime);

                    float angle = Quaternion.Angle(agentGameObject.transform.rotation, oppositeDirection);
                    if (angle < 0.1f)
                    {
                        rotationCompleted = true;
                        phase++;
                    }
                }
                else
                {
                    if (phase == 2) context.SetState(new MoveState(context, agentData, player, agentGameObject, notify));

                    timer += Time.deltaTime;
                    if (timer >= maxLookDurationTime)
                    {
                        rotationCompleted = false;
                        oppositeDirection = Quaternion.LookRotation(-agentGameObject.transform.forward);
                    }
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

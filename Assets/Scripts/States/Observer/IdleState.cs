using UnityEngine;

namespace ObserverMinigame
{
    public class IdleState : AState
    {
        float maxIdleDurationTime;
        float timer;


        public IdleState(IContext context, EnemyData agentData, GameObject player, GameObject agent) : base(context, player, agent, agentData)
        {
            maxIdleDurationTime = Random.Range(1, agentData.idleMaxTime);
            timer = 0f;
        }

        public override void Update()
        {
            if(CheckPlayerInFOV(agentData.FOV, agentData.visionDistance))
            {
                context.SetState(new ShootPlayerState(context, agentData, player, agentGameObject));
            }
            else
            {
                timer += Time.deltaTime;
                if (timer >= maxIdleDurationTime)
                {
                    if(agentData.enemyType == EnemyData.EnemyType.Turret) context.SetState(new RotateState(context, agentData, player, agentGameObject));
                    else context.SetState(new MoveState(context, agentData, player, agentGameObject));
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

using System;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

namespace ObjectPoolMinigame
{
    public class IdleState : AState
    {
        float maxIdleDurationTime;
        float timer;


        public IdleState(IContext context, EnemyData agentData, Animator animator, GameObject playerHead, GameObject agentEyes) 
            : base(context, agentData, animator, playerHead, agentEyes)
        {
            maxIdleDurationTime = agentData.idleTime;
            timer = 0f;
        }

        public override void Update()
        {
            if (CheckPlayerInFOV())
            {
                context.SetState(new CombatState(context, agentData, animator, playerHead, agentEyes));
            }
            else
            {
                timer += Time.deltaTime;
                if (timer >= maxIdleDurationTime)
                {
                    context.SetState(new WanderState(context, agentData, animator, playerHead, agentEyes));
                }
            }
        }

        public override void Enter()
        {
            animator.SetTrigger("IdleState");
        }
        public override void Exit()
        {
        }
        public override void FixedUpdate()
        {
        }
    }
}

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


        public IdleState(IContext context, EnemyData agentData, GameObject player, GameObject agent, Animator animator, GameObject playerHead, GameObject agentHead, EnemyGunManager gunManager) 
            : base(context, player, agent, agentData, animator, playerHead, agentHead, gunManager)
        {
            maxIdleDurationTime = agentData.idleTime;
            timer = 0f;
        }

        public override void Update()
        {
            if (CheckPlayerInFOV())
            {
                //agentGameObject.GetComponent<SoundEffectsController>().Movement(true);
                context.SetState(new CombatState(context, agentData, player, agentGameObject, animator, playerHead, agentHead, gunManager));
            }
            else
            {
                timer += Time.deltaTime;
                if (timer >= maxIdleDurationTime)
                {
                    //if(agentData.enemyType == EnemyData.EnemyType.Turret) context.SetState(new RotateState(context, agentData, player, agentGameObject, notify));
                    context.SetState(new WanderState(context, player, agentGameObject, agentData, animator, playerHead, agentHead, gunManager));
                }
            }
        }

        public override void Enter()
        {
            Debug.Log("Entrando al estado de idle");
            //animator.SetTrigger("IdleState");
        }
        public override void Exit()
        {
            Debug.Log("Saliendo del estado de idle");
        }
        public override void FixedUpdate()
        {
        }
    }
}

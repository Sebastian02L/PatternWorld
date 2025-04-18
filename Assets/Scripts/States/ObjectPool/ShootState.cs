using UnityEngine;

namespace ObjectPoolMinigame
{
    public class ShootState : AState
    {
        bool alingment = false;
        Vector3 playerDirection;
        float timer = 0f;

        public ShootState(IContext context, EnemyData agentData, GameObject player, GameObject agent, Animator animator, GameObject playerHead, GameObject agentHead, EnemyGunManager gunManager)
            : base(context, player, agent, agentData, animator, playerHead, agentHead, gunManager)
        {

        }

        public override void Enter()
        {
            Debug.Log("Entrando al estado de disparar");
            animator.SetTrigger("Shoot");
            playerDirection = playerHead.transform.position - agentHead.transform.position;
            playerDirection.y = 0;
            agentGameObject.transform.forward = playerDirection.normalized;
        }

        public override void Exit()
        {
        }

        public override void FixedUpdate()
        {
        }

        public override void Update()
        {
            if (CheckPlayerInFOV()) 
            {
                playerDirection = playerHead.transform.position - agentHead.transform.position;
                playerDirection.y = 0;
                agentGameObject.transform.forward = playerDirection.normalized;
                gunManager.Shoot();
                timer = 0f;
            }
            else
            {
                timer += Time.deltaTime;
                if(timer > 3)
                {
                    context.SetState(new WanderState(context, player, agentGameObject, agentData, animator, playerHead, agentHead, gunManager));
                }
            }
        }
    }
}

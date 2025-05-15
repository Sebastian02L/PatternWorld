using UnityEngine;

namespace ObjectPoolMinigame
{
    public class CombatState : AState
    {
        HealthManager healthManager;
        Vector3 playerDirection;
        float timer = 0f;
        float lostViewTimer = 1.5f;
        EnemyGunManager gunManager;

        public CombatState(IContext context, EnemyData agentData, Animator animator, GameObject playerHead, GameObject agentEyes)
            : base(context, agentData, animator, playerHead, agentEyes)
        {
        }

        //Look into player's direction
        public override void Enter()
        {
            animator.SetTrigger("Shoot");
            gunManager = context.GetGameObject().GetComponentInChildren<EnemyGunManager>();
            AudioManager.Instance.PlaySoundEffect(context.GetGameObject().GetComponent<AudioSource>(), "OPM_PlayerSpotted", 0.5f);
            playerDirection = playerHead.transform.position - agentEyes.transform.position;
            playerDirection.y = 0;
            context.GetGameObject().transform.forward = playerDirection.normalized;

            healthManager = context.GetGameObject().GetComponent<HealthManager>();
        }

        public override void Update()
        {
            //When the enemy haves low health
            if (healthManager.GetHealth < (healthManager.GetMaxHealth / 3))
            {
                context.SetState(new EscapeState(context, agentData, animator, playerHead, agentEyes));
            }
            else
            {
                if (CheckPlayerInFOV()) //If teh enemy sees the player, he can shoot him
                {
                    playerDirection = playerHead.transform.position - agentEyes.transform.position;
                    playerDirection.y = 0;
                    context.GetGameObject().transform.forward = playerDirection.normalized;
                    gunManager.Shoot();
                    timer = 0f;
                }
                else
                {
                    //If the enemy looses the player
                    timer += Time.deltaTime;
                    if (timer > lostViewTimer)
                    {
                        context.SetState(new WanderState(context, agentData, animator, playerHead, agentEyes));
                    }
                }
            }
            
        }

        public override void Exit()
        {
        }
        public override void FixedUpdate()
        {
        }
    }
}
